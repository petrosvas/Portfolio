using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class General : MonoBehaviour
{

    Canvas c;
    Text t, val, min, ab, allValid;
    Board b;
    InputField i, j, mi, mj, vali, valj;
    public UnityPiece[] arr;
    float timetook;

    // Start is called before the first frame update
    void Start()
    {
        b = new Board();
        /*c = GameObject.Find("Canvas").GetComponent<Canvas>();
        t = GameObject.Find("Text").GetComponent<Text>();
        i = GameObject.Find("i").GetComponent<InputField>();
        j = GameObject.Find("j").GetComponent<InputField>();
        mi = GameObject.Find("mi").GetComponent<InputField>();
        mj = GameObject.Find("mj").GetComponent<InputField>();
        val = GameObject.Find("Valid Moves").GetComponent<Text>();
        vali = GameObject.Find("validI").GetComponent<InputField>();
        valj = GameObject.Find("validJ").GetComponent<InputField>();
        min = GameObject.Find("Min").GetComponent<Text>();
        ab = GameObject.Find("AB").GetComponent<Text>();
        allValid = GameObject.Find("AllValid").GetComponent<Text>();*/
    }
    
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            MonoBehaviour.print(b.GetPiecesInfo());
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            ShowPieces();
        }*/
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(Cor());
            Min();
            StopCoroutine(Cor());
            print(timetook);
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Cor());
            AB();
            StopCoroutine(Cor());
            print(timetook);
        }
    }
    /*
    // Update is called once per frame
    public void Print()
    {
        t.text = b.Print();
    }

    public void Move()
    {
        b.Move(int.Parse(i.text), int.Parse(j.text), int.Parse(mi.text), int.Parse(mj.text));
        Print();
    }

    public void AllValidMoves()
    {
        allValid.text = b.GetAllValidMoves();
    }

    public void Generate()
    {
        b.GenerateAllValidMoves();
    }

    public void ValidMoes()
    {
        val.text = "Valid Moves for a piece:\n" + b.ShowValidMoves(int.Parse(vali.text), int.Parse(valj.text));
    }

    public void ShowPieces()
    {
        string str = "";
        for(int i=0; i<32; i++)
        {
            str += b.pieces[i].color.ToString() + " " + b.pieces[i].pieceType + ", id = " + b.pieces[i].id  + "\n";
        }
        MonoBehaviour.print(str);
    }
    */

    IEnumerator Cor()
    {
        timetook += Time.deltaTime;
        yield return new WaitForEndOfFrame();
    }

    public void Min()
    {
        int[] m = Minimax.GetBestMove(b, 3, true);
        //min.text = a[0] + ", " + a[1] + " to " + a[2] + ", " + a[3] + " with score " + a[4];
        arr[b.board[m[0], m[1]].id].Move(m[0], m[1], m[2], m[3]);
        b.Move(m[0], m[1], m[2], m[3]);
        //Print();
    }

    public void AB()
    {
        int[] a = AlphaBeta.GetBestMove(b, 3, true, AlphaBeta.Alpha, AlphaBeta.Beta);
        //ab.text = a[0] + ", " + a[1] + " to " + a[2] + ", " + a[3] + " with score " + a[4];
        arr[b.board[a[0], a[1]].id].Move(a[0], a[1], a[2], a[3]);
        b.Move(a[0], a[1], a[2], a[3]);
        //Print();
    }

    IEnumerator M()
    {
        int[] m = Minimax.GetBestMove(b, 3, true);
        //min.text = a[0] + ", " + a[1] + " to " + a[2] + ", " + a[3] + " with score " + a[4];
        arr[b.board[m[0], m[1]].id].Move(m[0], m[1], m[2], m[3]);
        b.Move(m[0], m[1], m[2], m[3]);
        //Print();
        yield return null;
    }

    IEnumerator A()
    {
        int[] a = AlphaBeta.GetBestMove(b, 3, true, AlphaBeta.Alpha, AlphaBeta.Beta);
        //ab.text = a[0] + ", " + a[1] + " to " + a[2] + ", " + a[3] + " with score " + a[4];
        arr[b.board[a[0], a[1]].id].Move(a[0], a[1], a[2], a[3]);
        b.Move(a[0], a[1], a[2], a[3]);
        //Print();
        yield return null;
    }
}
