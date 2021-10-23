using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
using System;

public class Tutorial : MonoBehaviour
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
    Text message;
    Board b;

    //sbyte[] savedGame = new sbyte[88];

    public int idToPromote = -1;

    GameObject promotioncan;

    List<TutorialLevel> originalList = new List<TutorialLevel>();

    int tutorialIndex = -1;

    bool promotionPossible = false;

    Button next, previous;

    Sprite[] sprites;

    Music m;

    // Start is called before the first frame update
    void Start()
    {
        m = FindObjectOfType<Music>();
        promotioncan = GameObject.Find("Promotion Canvas");
        promotioncan.SetActive(false);

        originalList = ToturialInitialize();
        canvas = GameObject.Find("Canvas");
        message = GameObject.Find("Message").GetComponent<Text>();

        next = GameObject.Find("Next").GetComponent<Button>();
        previous = GameObject.Find("Previous").GetComponent<Button>();
        previous.interactable = false;

        CreateSprites();

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

        NextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("White King: " + b.kingcastled + ". Black King: " + b.kingcastledblack);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            print(b.GetAllValidMoves());
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Minimax.DeepTime(b);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            string table = "";
            sbyte[,] state = b.CurrentBoard();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    table += (int)state[i, j];
                }
                table += "\n";
            }
            print(table);
            Board board = new Board(state);
            print(board.Print());
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            string table = "";
            sbyte[] state = b.FinalState();
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    table += state[index];
                    index++;
                }
                table += "\n";
            }
            for (int i = 0; i < 16; i++)
            {
                table += state[index] + ", ";
                index++;
            }
            table += '\n';
            for (int i = 0; i < 8; i++)
            {
                table += state[index] + ", ";
                index++;
            }
            print(table);
            Board board = new Board(state);
            print(board.Print());
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            //Board b2 = new Board(b.CurrentBoard());
            print(b.Print());
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            print(GetId(0, 0));
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
            print("King moving from " + i + ", " + j + " to " + mi + ", " + mj);
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
            else if (b.board[i, j].color == Color.white && mi == 7 && promotionPossible)
            {
                uPieces[b.board[i, j].id].GetComponent<UnityPiece>().Move(i, j, mi, mj);
                b.Move(i, j, mi, mj);
                idToPromote = b.board[mi, mj].id;
                promotioncan.SetActive(true);
                return;
            }
        }

        uPieces[b.board[i, j].id].GetComponent<UnityPiece>().Move(i, j, mi, mj);
        b.Move(i, j, mi, mj);

        b.GenerateAllValidMoves();

        /*
        if (b.whiteLost)
        {
            Lost(true);
        }
        else if (b.blackLost)
        {
            Lost(false);
        }

        if(b.whiteCheck)
        {
            print("White in Check!!");
            print("checker has: id " + b.pieces[b.checker].id + " and piece type " + b.pieces[b.checker].pieceType);
        }
        else if (b.blackCheck)
        {
            print("Black in Check!!");
        }
        */
    }

    public void Move(int i, int j, int mi, int mj)
    {
        // Player turn to play
        UpdateGraphics(i, j, mi, mj);
    }

    public List<int> GetMoves(int i, int j)
    {
        List<int> li = b.pieces[b.board[i, j].id].validMoves;
        return li;
    }

    public int GetId(int i, int j)
    {
        return b.board[i, j].id;
    }

    public void SynchronizeBoardWithGraphics()
    {
        for (int i = 0; i < 16; i++)
        {
            if (b.pieces[i].id != -1)
            {
                if (uPieces[i] == null)
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
                if (uPieces[i] == null)
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

    public void Promote(int[] array)
    {
        b.Promote(idToPromote, array);

        print("pawn with id " + idToPromote + " became a " + array[0] + " with " + array[1] + " points");

        SynchronizeBoardWithGraphics();

        b.GenerateAllValidMoves();

        if (b.whiteLost)
        {
            print("White Lost!!!!");
        }
        else if (b.blackLost)
        {
            print("Black Lost!!!!");
        }

        promotioncan.SetActive(false);
    }

    public List<TutorialLevel> ToturialInitialize()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/Tutorials.json");
        TutorialOriginalLevels original = JsonConvert.DeserializeObject<TutorialOriginalLevels>(jsonString);
        return original.levels;
    }

    public void NextTutorial()
    {
        m.PlayMenuClick();
        tutorialIndex++;
        //DestroyPieces();
        b = new Board(originalList[tutorialIndex].array);
        message.text = originalList[tutorialIndex].text;
        promotionPossible = originalList[tutorialIndex].promotion;
        if (tutorialIndex == originalList.Count - 1)
        {
            next.interactable = false;
            next.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1, 0.2f);
        }
        else
        {
            next.interactable = true;
            next.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1);
        }

        if (tutorialIndex == 0)
        {
            previous.interactable = false;
            previous.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1, 0.2f);
        }
        else
        {
            previous.interactable = true;
            previous.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1);
        }

        SynchronizeBoardWithGraphics();
    }

    public void PreviousTutorial()
    {
        m.PlayMenuClick();
        tutorialIndex--;
        //DestroyPieces();
        b = new Board(originalList[tutorialIndex].array);
        message.text = originalList[tutorialIndex].text;
        promotionPossible = originalList[tutorialIndex].promotion;
        if (tutorialIndex == 0)
        {
            previous.interactable = false;
            previous.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1, 0.2f);
        }
        else
        {
            previous.interactable = true;
            previous.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1);
        }

        if (tutorialIndex == originalList.Count - 1)
        {
            next.interactable = false;
            next.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1, 0.2f);
        }
        else
        {
            next.interactable = true;
            next.GetComponentInChildren<Text>().color = new UnityEngine.Color(1, 1, 1);
        }

        SynchronizeBoardWithGraphics();
    }

    public void BackToMenu()
    {
        m.PlayMenuClick();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
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

}

[System.Serializable]
public class TutorialLevel
{
    public sbyte[] array;
    public string text;
    public bool promotion;
}

[System.Serializable]
public class TutorialOriginalLevels
{
    public List<TutorialLevel> levels;
}