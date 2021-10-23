using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class Mono : MonoBehaviour
{

    public GameObject blackBg;
    public GameObject whiteBg;
    public GameObject greenValid;
    Sprite whiteRook, whiteBishop, whitePawn, whiteKing, whiteQueen, whiteKnight;
    Sprite blackRook, blackBishop, blackPawn, blackKing, blackQueen, blackKnight;
    public GameObject starting;
    public GameObject[] uPieces = new GameObject[32];
    public GameObject[,] BGBoard = new GameObject[8, 8];
    GameObject canvas;
    Board b;

    public List<sbyte[]> moveHistory = new List<sbyte[]>();

    public bool openings = true;

    public int idToPromote = -1;

    GameObject promotioncan;
    bool promotionInProgress = false;

    GameObject hourglass;

    Text gameOver;

    public bool playerCanMove = true;

    Diffs dif;
    int difficulty;

    Sprite[] sprites;

    Music m;

    // Start is called before the first frame update
    void Start()
    {
        m = FindObjectOfType<Music>();
        m.PlayGameTheme();
        promotioncan = GameObject.Find("Promotion Canvas");
        promotioncan.SetActive(false);

        /*
        i = GameObject.Find("i").GetComponent<InputField>();
        j = GameObject.Find("j").GetComponent<InputField>();
        mi = GameObject.Find("mi").GetComponent<InputField>();
        mj = GameObject.Find("mj").GetComponent<InputField>();
        vi = GameObject.Find("vi").GetComponent<InputField>();
        vj = GameObject.Find("vj").GetComponent<InputField>();
        ii = GameObject.Find("infoI").GetComponent<InputField>();
        ij = GameObject.Find("infoJ").GetComponent<InputField>();
        info = GameObject.Find("Info").GetComponent<Text>();
        */

        CreateSprites();

        b = new Board(true);
        for(int i=0; i<32; i++)
        {
            uPieces[i] = Instantiate(starting);
        }

        dif = FindObjectOfType<Diffs>();
        difficulty = dif.GetDif();
        if (difficulty != (int)Difficulties.Hard)
            openings = false;


        canvas = GameObject.Find("Canvas");
        hourglass = GameObject.Find("Glass");
        hourglass.SetActive(false);

        gameOver = GameObject.Find("Game Over").GetComponent<Text>();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                BGBoard[i, j] = Instantiate(((i + j) % 2 == 0) ? blackBg : whiteBg);
                BGBoard[i, j].transform.SetParent(canvas.transform);
                BGBoard[i, j].GetComponent<UnityPiece>().Movable = false;
                BGBoard[i, j].GetComponent<UnityPiece>().I = i;
                BGBoard[i, j].GetComponent<UnityPiece>().J = j;
                BGBoard[i, j].GetComponent<RectTransform>().anchoredPosition = new Vector2(j * 40, i * 40);
                BGBoard[i, j].transform.localScale = Vector3.one;
            }
        }

        SynchronizeBoardWithGraphics();

        moveHistory.Add(b.FinalState());

        Openings.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            print("White King: " + b.kingcastled + ". Black King: " + b.kingcastledblack);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            print(b.GetAllValidMoves());
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            Minimax.DeepTime(b);
        }
        else if(Input.GetKeyDown(KeyCode.B))
        {
            /*
            string s2 = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (b.board[i, j].pieceType != '*')
                        s2 += "Pieces[" + b.board[i, j].id + "]: " + b.pieces[b.board[i, j].id].pieceType + ". Board[i, j]: " + b.board[i, j].pieceType + "\n";
                }
            }
            print(s2);
            */
            print(b.Print());
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {
            sbyte[,] state = b.CurrentBoard();
            string str = "";
            for(int i=7; i>=0; i--)
            {
                for(int j=0; j<8; j++)
                {
                    str += state[i, j] + "|";
                }
                str += "\n";
            }
            print(str);
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            sbyte[] arr = b.FinalState();
            string state = "";
            int index = 0;

            for(int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    state += arr[index] + "|";
                    index++;
                }
                state += "\n";
            }
            print(state);

        }
        
    }

    private void UpdateGraphics(int i, int j, int mi, int mj)
    {
        m.PlayPieceClick();
        if (b.board[mi, mj].id != -1)
        {
            Destroy(uPieces[b.board[mi, mj].id]);
        }

        if (b.board[i, j].pieceType == 'k')
        {
            if (mj - j == 2)
            {
                if (b.board[i, j].color == Color.white)
                {
                    uPieces[b.board[0, 7].id].GetComponent<UnityPiece>().Move(0, 7, 0, 5);
                }
                else
                {
                    uPieces[b.board[7, 7].id].GetComponent<UnityPiece>().Move(7, 7, 7, 5);
                }
            }
            else if (mj - j == -2)
            {
                if (b.board[i, j].color == Color.white)
                {
                    uPieces[b.board[0, 0].id].GetComponent<UnityPiece>().Move(0, 0, 0, 3);
                }
                else
                {
                    uPieces[b.board[7, 0].id].GetComponent<UnityPiece>().Move(7, 0, 7, 3);
                }
            }
        }
        else if (b.board[i, j].pieceType == 'p')
        {
            if (b.board[i, j].color == Color.black && mi == 0)
            {
                uPieces[b.board[i, j].id].GetComponent<Image>().sprite = blackQueen;
            }
            else if (b.board[i, j].color == Color.white && mi == 7)
            {
                uPieces[b.board[i, j].id].GetComponent<UnityPiece>().Move(i, j, mi, mj);
                b.Move(i, j, mi, mj);
                idToPromote = b.board[mi, mj].id;
                promotionInProgress = true;
                promotioncan.SetActive(true);
                return;
            }
        }

        uPieces[b.board[i, j].id].GetComponent<UnityPiece>().Move(i, j, mi, mj);
        b.Move(i, j, mi, mj);
        moveHistory.Add(b.FinalState());

        b.GenerateAllValidMoves();

        if (b.whiteLost && !playerCanMove)
        {
            if (!b.whiteCheck)
            {
                Draw("Stalemate");
                return;
            }
            else
            {
                Lost(true);
                return;
            }
        }
        else if (b.blackLost && playerCanMove)
        {
            if (!b.blackCheck)
            {
                Draw("Stalemate");
                return;
            }
            else
            {
                Lost(false);
                return;
            }
        }
        else if (b.fiftyMoveRule == 0)
        {
            Draw("Fifty Move Rule");
            return;
        }
        else
        {
            if (moveHistory.Count > 9)
            {
                for (int a = 1; a < moveHistory.Count; a++)
                {
                    for (int b = a + 1; b < moveHistory.Count; b++)
                    {
                        for (int c = b + 1; c < moveHistory.Count; c++)
                        {
                            if (ThreefoldRepetition(moveHistory[a], moveHistory[b]) && ThreefoldRepetition(moveHistory[a], moveHistory[c]))
                            {
                                print("Threefold Repetition Draw!");
                                Draw("Threefold Repetition");
                                return;
                            }
                        }
                    }
                }
            }
            print(b.fiftyMoveRule);

            foreach (Piece p in b.pieces)
            {
                if (p.pieceType != 'k' && p.id != -1)
                {
                    return;
                }
            }
            Draw("Only Kings Remain");
            return;
        }
    }

    public void Move(int i, int j, int mi, int mj)
    {
        // Player turn to play
        UpdateGraphics(i, j, mi, mj);

        playerCanMove = false;

        if (openings)
        {
            int[] answer = Openings.NextMove(new int[] { i, j, mi, mj });
            if (answer == null)
            {
                if (difficulty == (int)Difficulties.Easy)
                    StartCoroutine(EasyMove());
                else
                    StartCoroutine(HardMove());
                openings = false;
                print("null was returned");
            }
            else if (answer[0] == -1)
            {
                if (difficulty == (int)Difficulties.Easy)
                    StartCoroutine(EasyMove());
                else
                    StartCoroutine(HardMove());
                openings = false;
                print("openings is now false");
            }
            else
            {
                StartCoroutine(OpeningsMove(answer[0], answer[1], answer[2], answer[3]));
                //print("openings used for move " + answer[0] + ", " + answer[1] + ", " + answer[2] + ", " + answer[3]);
            }
        }
        else if (gameOver.text == "")
        {
            if (difficulty == (int)Difficulties.Easy)
                StartCoroutine(EasyMove());
            else
                StartCoroutine(HardMove());
        }
    }

    public void Undo()
    {
        m.PlayMenuClick();

        if (!playerCanMove)
            return;

        if (moveHistory.Count > 0)
        {
            if (moveHistory.Count <= 3)
            {
                b = new Board(moveHistory[0]);
                moveHistory.Clear();
                moveHistory.Add(b.FinalState());
            }
            else
            {
                b = new Board(moveHistory[moveHistory.Count - 3]);
                moveHistory.RemoveAt(moveHistory.Count - 1);
                moveHistory.RemoveAt(moveHistory.Count - 1);
            }

            b.fiftyMoveRule++;
            SynchronizeBoardWithGraphics();
        }
        else
        {
            print("The move history list is already empty!!");
        }
    }

    IEnumerator HardMove()
    {
        while (promotionInProgress)
            yield return null;

        float time = Time.realtimeSinceStartup;

        hourglass.SetActive(true);

        int[] mnm = new int[5];
        Board tmp = new Board(b);
        // AI turn to play.
        Thread mm = new Thread(() =>
        {
            //mnm = Minimax.GetBestMove(b, 4, true);
            mnm = AlphaBeta.GetBestMove(tmp, 3, true, AlphaBeta.Alpha, AlphaBeta.Beta);
        });
        mm.Start();

        while (mm.IsAlive || Time.realtimeSinceStartup - time < 0.75)
        {
            yield return null;
        }

        UpdateGraphics(mnm[0], mnm[1], mnm[2], mnm[3]);

        hourglass.SetActive(false);

        playerCanMove = true;
    }

    IEnumerator OpeningsMove(int i, int j, int mi, int mj)
    {
        hourglass.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hourglass.SetActive(false);
        playerCanMove = true;
        UpdateGraphics(i, j, mi, mj);
    }

    IEnumerator EasyMove()
    {
        while (promotionInProgress)
            yield return null;

        float time = Time.realtimeSinceStartup;

        hourglass.SetActive(true);

        int[] mnm = new int[5];
        Board tmp = new Board(b);
        // AI turn to play.
        Thread mm = new Thread(() =>
        {
            //mnm = Minimax.GetBestMove(b, 4, true);
            mnm = AlphaBeta.GetBestMove(tmp, 2, true, AlphaBeta.Alpha, AlphaBeta.Beta);
        });
        mm.Start();

        while (mm.IsAlive || Time.realtimeSinceStartup - time < 0.5)
        {
            yield return null;
        }

        UpdateGraphics(mnm[0], mnm[1], mnm[2], mnm[3]);

        hourglass.SetActive(false);

        playerCanMove = true;
    }

    public List<int> getMoves(int i, int j)
    {
        List<int> li = b.pieces[b.board[i, j].id].validMoves;
        return li;
    }

    public int getId(int i, int j)
    {
        return b.board[i, j].id;
    }

    public void Save()
    {
        m.PlayMenuClick();

        if (!playerCanMove)
            return;
        JSONSave savedGame = new JSONSave();
        savedGame.array = moveHistory[moveHistory.Count - 1];
        string jsonString = JsonConvert.SerializeObject(savedGame);
        print(jsonString);

        File.WriteAllText(Application.dataPath + "/Resources/Save.json", jsonString);
    }

    public void Load()
    {
        m.PlayMenuClick();

        if (!playerCanMove)
            return;
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/Save.json");
        JSONSave savedGame = JsonConvert.DeserializeObject<JSONSave>(jsonString);
        b = new Board(savedGame.array);

        SynchronizeBoardWithGraphics();

        if (b.whiteLost && !playerCanMove)
        {
            if(!b.whiteCheck)
                Draw("Stalemate");
            else
                Lost(true);
        }
        else if (b.blackLost && playerCanMove)
        {
            if (!b.blackCheck)
                Draw("Stalemate");
            else
                Lost(false);
        }
        else
        {
            moveHistory.Clear();
            moveHistory.Add(b.FinalState());
        }
    }

    public void SynchronizeBoardWithGraphics()
    {
        for (int i = 0; i < 16; i++)
        {
            if (b.pieces[i].id != -1)
            {
                if(uPieces[i] == null)
                {
                    uPieces[i] = Instantiate(starting);
                }

                //GameObject created;
                if (b.pieces[i].color == Color.white)
                {
                    if (b.pieces[i].pieceType == 'p')
                    {
                        //created = Instantiate(whitePawn);
                        uPieces[i].GetComponent<Image>().sprite = whitePawn;
                    }
                    else if (b.pieces[i].pieceType == 'r')
                    {
                        //created = Instantiate(whiteRook);
                        uPieces[i].GetComponent<Image>().sprite = whiteRook;
                    }
                    else if (b.pieces[i].pieceType == 'n')
                    {
                        //created = Instantiate(whiteKnight);
                        uPieces[i].GetComponent<Image>().sprite = whiteKnight;
                    }
                    else if (b.pieces[i].pieceType == 'b')
                    {
                        //created = Instantiate(whiteBishop);
                        uPieces[i].GetComponent<Image>().sprite = whiteBishop;
                    }
                    else if (b.pieces[i].pieceType == 'q')
                    {
                        //created = Instantiate(whiteQueen);
                        uPieces[i].GetComponent<Image>().sprite = whiteQueen;
                    }
                    else
                    {
                        //created = Instantiate(whiteKing);
                        uPieces[i].GetComponent<Image>().sprite = whiteKing;
                    }
                }
                else
                {
                    if (b.pieces[i].pieceType == 'p')
                    {
                        //created = Instantiate(blackPawn);
                        uPieces[i].GetComponent<Image>().sprite = blackPawn;
                    }
                    else if (b.pieces[i].pieceType == 'r')
                    {
                        //created = Instantiate(blackRook);
                        uPieces[i].GetComponent<Image>().sprite = blackRook;
                    }
                    else if (b.pieces[i].pieceType == 'n')
                    {
                        //created = Instantiate(blackKnight);
                        uPieces[i].GetComponent<Image>().sprite = blackKnight;
                    }
                    else if (b.pieces[i].pieceType == 'b')
                    {
                        //created = Instantiate(blackBishop);
                        uPieces[i].GetComponent<Image>().sprite = blackBishop;
                    }
                    else if (b.pieces[i].pieceType == 'q')
                    {
                        //created = Instantiate(blackQueen);
                        uPieces[i].GetComponent<Image>().sprite = blackQueen;
                    }
                    else
                    {
                        //created = Instantiate(blackKing);
                        uPieces[i].GetComponent<Image>().sprite = blackKing;
                    }
                }
                uPieces[i].GetComponent<UnityPiece>().I = b.pieces[i].i;
                uPieces[i].GetComponent<UnityPiece>().J = b.pieces[i].j;
                uPieces[i].transform.SetParent(canvas.transform);
                uPieces[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(uPieces[i].GetComponent<UnityPiece>().J * 40, uPieces[i].GetComponent<UnityPiece>().I * 40);
                uPieces[i].transform.localScale = Vector3.one;
            }
            else
            {
                Destroy(uPieces[i]);
            }
        }
        for (int i = 16; i < 32; i++)
        {
            if (b.pieces[i].id != -1)
            {
                if(uPieces[i] == null)
                {
                    uPieces[i] = Instantiate(starting);
                }

                //GameObject created;
                if (b.pieces[i].color == Color.white)
                {
                    if (b.pieces[i].pieceType == 'p')
                    {
                        //created = Instantiate(whitePawn);
                        uPieces[i].GetComponent<Image>().sprite = whitePawn;
                    }
                    else if (b.pieces[i].pieceType == 'r')
                    {
                        //created = Instantiate(whiteRook);
                        uPieces[i].GetComponent<Image>().sprite = whiteRook;
                    }
                    else if (b.pieces[i].pieceType == 'n')
                    {
                        //created = Instantiate(whiteKnight);
                        uPieces[i].GetComponent<Image>().sprite = whiteKnight;
                    }
                    else if (b.pieces[i].pieceType == 'b')
                    {
                        //created = Instantiate(whiteBishop);
                        uPieces[i].GetComponent<Image>().sprite = whiteBishop;
                    }
                    else if (b.pieces[i].pieceType == 'q')
                    {
                        //created = Instantiate(whiteQueen);
                        uPieces[i].GetComponent<Image>().sprite = whiteQueen;
                    }
                    else
                    {
                        //created = Instantiate(whiteKing);
                        uPieces[i].GetComponent<Image>().sprite = whiteKing;
                    }
                }
                else
                {
                    if (b.pieces[i].pieceType == 'p')
                    {
                        //created = Instantiate(blackPawn);
                        uPieces[i].GetComponent<Image>().sprite = blackPawn;
                    }
                    else if (b.pieces[i].pieceType == 'r')
                    {
                        //created = Instantiate(blackRook);
                        uPieces[i].GetComponent<Image>().sprite = blackRook;
                    }
                    else if (b.pieces[i].pieceType == 'n')
                    {
                        //created = Instantiate(blackKnight);
                        uPieces[i].GetComponent<Image>().sprite = blackKnight;
                    }
                    else if (b.pieces[i].pieceType == 'b')
                    {
                        //created = Instantiate(blackBishop);
                        uPieces[i].GetComponent<Image>().sprite = blackBishop;
                    }
                    else if (b.pieces[i].pieceType == 'q')
                    {
                        //created = Instantiate(blackQueen);
                        uPieces[i].GetComponent<Image>().sprite = blackQueen;
                    }
                    else
                    {
                        //created = Instantiate(blackKing);
                        uPieces[i].GetComponent<Image>().sprite = blackKing;
                    }
                }
                uPieces[i].GetComponent<UnityPiece>().I = b.pieces[i].i;
                uPieces[i].GetComponent<UnityPiece>().J = b.pieces[i].j;
                uPieces[i].transform.SetParent(canvas.transform);
                uPieces[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(uPieces[i].GetComponent<UnityPiece>().J * 40, uPieces[i].GetComponent<UnityPiece>().I * 40);
                uPieces[i].transform.localScale = Vector3.one;
                Destroy(uPieces[i].GetComponent<OnPiece>());
            }
            else
            {
                Destroy(uPieces[i]);
            }
        }
    }

    public void Lost(bool whitePlayer)
    {
        Destroy(GameObject.Find("Glass"));
        if (whitePlayer)
        {
            m.PlayDefeat();
            gameOver.text = "You \n Lose...";
        }
        else
        {
            m.PlayVictory();
            gameOver.text = "You \n Win!!!!";
        }
        hourglass.SetActive(false);
        GameObject.Find("Undo").SetActive(false);
        GameObject.Find("Save").SetActive(false);
        GameObject.Find("Load").SetActive(false);

        for(int i=0; i<16; i++)
        {
            if(uPieces[i] != null)
                Destroy(uPieces[i].GetComponent<OnPiece>());
        }

    }

    public void DisableOpenings()
    {
        openings = false;
    }

    public void Promote(int[] array)
    {
        b.Promote(idToPromote, array);

        print("pawn with id " + idToPromote + " became a " + array[0] + " with " + array[1] + " points");

        SynchronizeBoardWithGraphics();

        moveHistory.Add(b.FinalState());

        b.GenerateAllValidMoves();

        if (b.whiteLost && !playerCanMove)
        {
            if (!b.whiteCheck)
            {
                Draw("Stalemate");
                return;
            }
            else
            {
                Lost(true);
                return;
            }
        }
        else if (b.blackLost && playerCanMove)
        {
            if (!b.blackCheck)
            {
                Draw("Stalemate");
                return;
            }
            else
            {
                Lost(false);
                return;
            }
        }

        promotioncan.SetActive(false);
        promotionInProgress = false;
    }

    public void Back()
    {
        m.PlayMenuClick();

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void Draw(string reason)
    {
        gameOver.text = "Draw! \n " + reason;
        hourglass.SetActive(false);
        GameObject.Find("Undo").SetActive(false);
        GameObject.Find("Save").SetActive(false);
        GameObject.Find("Load").SetActive(false);
        Destroy(GameObject.Find("Glass"));

        for (int i = 0; i < 16; i++)
        {
            if(uPieces[i] != null)
                Destroy(uPieces[i].GetComponent<OnPiece>());
        }
    }

    public void CreateSprites()
    {
        sprites = Resources.LoadAll<Sprite>("pcqrGKzLi");
        foreach (Sprite sp in sprites)
        {
            if (sp.name == "White Queen")
                whiteQueen = sp;
            else if (sp.name == "White King")
                whiteKing = sp;
            else if (sp.name == "White Bishop")
                whiteBishop = sp;
            else if (sp.name == "White Pawn")
                whitePawn = sp;
            else if (sp.name == "White Knight")
                whiteKnight = sp;
            else if (sp.name == "White Rook")
                whiteRook = sp;
            else if (sp.name == "Black Rook")
                blackRook = sp;
            else if (sp.name == "Black King")
                blackKing = sp;
            else if (sp.name == "Black Queen")
                blackQueen = sp;
            else if (sp.name == "Black Bishop")
                blackBishop = sp;
            else if (sp.name == "Black Pawn")
                blackPawn = sp;
            else if (sp.name == "Black Knight")
                blackKnight = sp;
        }
    }

    public bool ThreefoldRepetition(sbyte[] a, sbyte[] b)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            if (a[i] != b[i])
                return false;
        }
        return true;
    }

}

[System.Serializable]
public class JSONSave
{
    public sbyte[] array;
}
