using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board
{
    public Piece[] pieces = new Piece[32]; // 0 - 15 whites, 16 - 31 blacks
    public Piece[,] board = new Piece[8, 8];
    public bool gameOver = false;
    public bool whiteCheck = false;
    public bool blackCheck = false;
    public sbyte checker = -1;
    public sbyte checker2 = -1;
    public bool kingcastling = true;
    public bool r1castling = true;
    public bool r2castling = true;
    public bool kingcastlingblack = true;
    public bool r1castlingblack = true;
    public bool r2castlingblack = true;
    public bool kingcastled = false;
    public bool kingcastledblack = false;
    public bool whiteLost = true;
    public bool blackLost = true;
    public bool calledByAi = false;
    public int fiftyMoveRule = 50;

    public static int WhiteKingID = 4;
    public static int BlackKingID = 28;

    public Board()
    {
        sbyte ids = 0;
        for (int i = 2; i < 6; i++)
            for (int j = 0; j < 8; j++)
            {
                board[i, j].pieceType = '*';
                board[i, j].color = Color.none;
                board[i, j].i = i;
                board[i, j].j = j;
                board[i, j].id = -1;
            }
        for (int j = 0; j < 8; j++)
        {
            board[1, j].pieceType = 'p';
            board[6, j].pieceType = 'p';
            board[1, j].points = (int)PiecePoints.pawn;
            board[6, j].points = (int)PiecePoints.pawn;
            board[1, j].color = Color.white;
            board[0, j].color = Color.white;
            board[6, j].color = Color.black;
            board[7, j].color = Color.black;
        }
        board[0, 0].pieceType = board[0, 7].pieceType = board[7, 0].pieceType = board[7, 7].pieceType = 'r';
        board[0, 0].points = board[0, 7].points = board[7, 0].points = board[7, 7].points = (int)PiecePoints.rook;

        board[0, 1].pieceType = board[0, 6].pieceType = board[7, 1].pieceType = board[7, 6].pieceType = 'n';
        board[0, 1].points = board[0, 6].points = board[7, 1].points = board[7, 6].points = (int)PiecePoints.knight;

        board[0, 2].pieceType = board[0, 5].pieceType = board[7, 2].pieceType = board[7, 5].pieceType = 'b';
        board[0, 2].points = board[0, 5].points = board[7, 2].points = board[7, 5].points = (int)PiecePoints.bishop;

        board[0, 4].pieceType = board[7, 3].pieceType = 'k';
        board[0, 3].pieceType = board[7, 4].pieceType = 'q';
        board[0, 3].points = board[7, 4].points = (int)PiecePoints.queen;

        for (int j = 0; j < 8; j++)
        {
            board[0, j].id = ids;
            pieces[ids] = board[0, j];
            pieces[ids].i = 0;
            pieces[ids].id = ids;
            pieces[ids].j = j;
            board[0, j].i = 0;
            board[0, j].j = j;
            ids++;
        }

        for (int j = 0; j < 8; j++)
        {
            board[1, j].id = ids;
            pieces[ids] = board[1, j];
            pieces[ids].i = 1;
            pieces[ids].id = ids;
            pieces[ids].j = j;
            ids++;
            board[1, j].i = 1;
            board[1, j].j = j;
        }

        for (int j = 0; j < 8; j++)
        {
            board[6, j].id = ids;
            pieces[ids] = board[6, j];
            pieces[ids].i = 6;
            pieces[ids].id = ids;
            pieces[ids].j = j;
            ids++;
            board[6, j].i = 6;
            board[6, j].j = j;
        }

        for (int j = 0; j < 8; j++)
        {
            board[7, j].id = ids;
            pieces[ids] = board[7, j];
            pieces[ids].i = 7;
            pieces[ids].id = ids;
            pieces[ids].j = j;
            ids++;
            board[7, j].i = 7;
            board[7, j].j = j;
        }

        for (int i = 0; i < 32; i++)
        {
            pieces[i].validMoves = new List<int>();
        }
        GenerateAllValidMoves();
    }

    public Board(bool b)
    {
        calledByAi = false;
        CreatePieces();

        for (int i = 0; i < 8; i++)
        {
            pieces[i].i = 0;
            pieces[i].j = i;

            pieces[i + 8].i = 1;
            pieces[i + 8].j = i;

            pieces[i + 16].i = 6;
            pieces[i + 16].j = i;

            pieces[i + 24].i = 7;
            pieces[i + 24].j = i;
        }

        for (int i = 2; i < 6; i++)
            for (int j = 0; j < 8; j++)
            {
                board[i, j].pieceType = '*';
                board[i, j].color = Color.none;
                board[i, j].i = i;
                board[i, j].j = j;
                board[i, j].id = -1;
                board[i, j].validMoves = null;
                board[i, j].points = 0;
            }

        for (int j = 0; j < 8; j++)
        {
            board[0, j] = pieces[j];
            board[0, j].validMoves = null;

            board[1, j] = pieces[j + 8];
            board[1, j].validMoves = null;

            board[6, j] = pieces[j + 16];
            board[6, j].validMoves = null;

            board[7, j] = pieces[j + 24];
            board[7, j].validMoves = null;
        }

        GenerateAllValidMoves();
    }

    public Board(Board b)
    {
        gameOver = b.gameOver;
        whiteCheck = b.whiteCheck;
        blackCheck = b.blackCheck;
        checker = b.checker;
        checker2 = b.checker2;
        r1castling = b.r1castling;
        r1castlingblack = b.r1castlingblack;
        r2castling = b.r2castling;
        r2castlingblack = b.r2castlingblack;
        kingcastled = b.kingcastled;
        kingcastledblack = b.kingcastledblack;
        kingcastling = b.kingcastling;
        kingcastlingblack = b.kingcastlingblack;
        whiteLost = b.whiteLost;
        blackLost = b.blackLost;
        calledByAi = true;

        for (int i = 0; i < 32; i++)
        {
            pieces[i] = b.pieces[i];
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = b.board[i, j];
            }
        }

        GenerateAllValidMoves();
    }

    public Board(ref Board b)
    {
        gameOver = b.gameOver;
        whiteCheck = b.whiteCheck;
        blackCheck = b.blackCheck;
        checker = b.checker;
        checker2 = b.checker2;
        r1castling = b.r1castling;
        r1castlingblack = b.r1castlingblack;
        r2castling = b.r2castling;
        r2castlingblack = b.r2castlingblack;
        kingcastled = b.kingcastled;
        kingcastledblack = b.kingcastledblack;
        kingcastling = b.kingcastling;
        kingcastlingblack = b.kingcastlingblack;
        whiteLost = b.whiteLost;
        blackLost = b.blackLost;
        calledByAi = true;

        for (int i = 0; i < 32; i++)
        {
            pieces[i] = b.pieces[i];
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = b.board[i, j];
            }
        }
    }

    public Board(sbyte[] state)
    {
        calledByAi = false;
        CreatePieces();

        for (int i = 0; i < 32; i++)
            pieces[i].id = -1;

        int index = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (state[index] == -1)
                {
                    board[i, j].pieceType = '*';
                    board[i, j].color = Color.none;
                    board[i, j].id = -1;
                }
                else
                {
                    sbyte id = state[index];
                    board[i, j].id = id;
                    board[i, j].pieceType = pieces[id].pieceType;
                    board[i, j].color = pieces[id].color;
                    pieces[id].i = i;
                    pieces[id].j = j;
                    pieces[id].id = id;
                }
                index++;
            }
        }

        for(int i=8; i<24; i++)
        {
            if(pieces[i].id != -1)
            {
                if (state[index] == 'p')
                {
                    pieces[i].pieceType = 'p';
                    pieces[i].points = (int)PiecePoints.pawn;
                    board[pieces[i].i, pieces[i].j].pieceType = 'p';
                }
                else if(state[index] == 'b')
                {
                    pieces[i].pieceType = 'b';
                    pieces[i].points = (int)PiecePoints.bishop;
                    board[pieces[i].i, pieces[i].j].pieceType = 'b';
                }
                else if (state[index] == 'q')
                {
                    pieces[i].pieceType = 'q';
                    pieces[i].points = (int)PiecePoints.queen;
                    board[pieces[i].i, pieces[i].j].pieceType = 'q';
                }
                else if (state[index] == 'n')
                {
                    pieces[i].pieceType = 'n';
                    pieces[i].points = (int)PiecePoints.knight;
                    board[pieces[i].i, pieces[i].j].pieceType = 'n';
                }
                else if (state[index] == 'r')
                {
                    pieces[i].pieceType = 'r';
                    pieces[i].points = (int)PiecePoints.rook;
                    board[pieces[i].i, pieces[i].j].pieceType = 'r';
                }
            }
            index++;
        }
        
        kingcastling = state[index] == 't' ? true : false;
        index++;
        r1castling = state[index] == 't' ? true : false;
        index++;
        r2castling = state[index] == 't' ? true : false;
        index++;
        kingcastled = state[index] == 't' ? true : false;
        index++;
        kingcastlingblack = state[index] == 't' ? true : false;
        index++;
        r1castlingblack = state[index] == 't' ? true : false;
        index++;
        r2castlingblack = state[index] == 't' ? true : false;
        index++;
        kingcastledblack = state[index] == 't' ? true : false;
        index++;
        fiftyMoveRule = state[index];

        GenerateAllValidMoves();
    }

    public Board(sbyte[,] state)
    {
        calledByAi = false;
        CreatePieces();

        for (int i = 0; i < 32; i++)
        {
            pieces[i].id = -1;
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (state[i, j] == -1)
                {
                    board[i, j].pieceType = '*';
                    board[i, j].color = Color.none;
                    board[i, j].id = -1;
                }
                else
                {
                    sbyte id = state[i, j];
                    board[i, j].id = id;
                    board[i, j].pieceType = pieces[id].pieceType;
                    board[i, j].color = pieces[id].color;
                    pieces[id].i = i;
                    pieces[id].j = j;
                    pieces[id].id = id;
                }
            }
        }
        GenerateAllValidMoves();
    }

    public string Print()
    {
        string pr = "";
        pr += " | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 |\n";
        for (int i = 7; i >= 0; i--)
        {
            pr += i;
            for (int j = 0; j < 8; j++)
                pr += " | " + board[i, j].pieceType;
            pr += (" | " + "\n");
        }

        return pr;
    }

    public void Move(int i, int j, int mi, int mj)
    {
        fiftyMoveRule--;
        if (board[i, j].pieceType == 'p')
        {
            if (board[i, j].color == Color.black && mi == 0)
            {
                board[i, j].pieceType = 'q';
                pieces[board[i, j].id].pieceType = 'q';
                pieces[board[i, j].id].points = (int)PiecePoints.queen;
            }
            else if (board[i, j].color == Color.white && mi == 7 && calledByAi)
            {
                board[i, j].pieceType = 'q';
                pieces[board[i, j].id].pieceType = 'q';
                pieces[board[i, j].id].points = (int)PiecePoints.queen;
            }
            fiftyMoveRule = 50;
        }
        else if (board[i, j].pieceType == 'k')
        {
            if (mj - j == 2)
            {
                if (board[i, j].color == Color.white)
                {
                    Move(0, 7, 0, 5);
                    kingcastled = true;
                    kingcastling = false;
                }
                else
                {
                    Move(7, 7, 7, 5);
                    kingcastledblack = true;
                    kingcastlingblack = false;
                }
            }
            else if (mj - j == -2)
            {
                if (board[i, j].color == Color.white)
                {
                    Move(0, 0, 0, 3);
                    kingcastled = true;
                    kingcastling = false;
                }
                else
                {
                    Move(7, 0, 7, 3);
                    kingcastledblack = true;
                    kingcastlingblack = false;
                }
            }
            else
            {
                if(board[i, j].color == Color.white)
                {
                    kingcastling = false;
                }
                else
                {
                    kingcastlingblack = false;
                }
            }
        }
        else if (board[i, j].pieceType == 'r')
        {
            if (board[i, j].id == 0)
                r1castling = false;
            else if (board[i, j].id == 7)
                r2castling = false;
            else if (board[i, j].id == 24)
                r1castlingblack = false;
            else if (board[i, j].id == 31)
                r2castlingblack = false;
        }

        if (board[mi, mj].pieceType == '*')
        {
            pieces[board[i, j].id].i = mi;
            pieces[board[i, j].id].j = mj;

            board[mi, mj].pieceType = board[i, j].pieceType;
            board[mi, mj].color = board[i, j].color;
            board[mi, mj].id = board[i, j].id;

            board[i, j].pieceType = '*';
            board[i, j].color = Color.none;
            board[i, j].id = -1;
        }
        else
        {
            if(board[mi, mj].pieceType == 'r')
            {
                if (board[mi, mj].id == 0)
                    r1castling = false;
                else if (board[mi, mj].id == 7)
                    r2castling = false;
                else if (board[mi, mj].id == 24)
                    r1castlingblack = false;
                else if (board[mi, mj].id == 31)
                    r2castlingblack = false;
            }

            pieces[board[mi, mj].id].id = -1;

            pieces[board[i, j].id].i = mi;
            pieces[board[i, j].id].j = mj;

            board[mi, mj].pieceType = board[i, j].pieceType;
            board[mi, mj].color = board[i, j].color;
            board[mi, mj].id = board[i, j].id;

            board[i, j].pieceType = '*';
            board[i, j].color = Color.none;
            board[i, j].id = -1;
            fiftyMoveRule = 50;
        }
    }

    public string GetAllValidMoves()
    {
        string str;
        int moves = 0;
        str = "Whites:\n";
        for (int i = 0; i < 16; i++)
        {
            if (pieces[i].id != -1)
                for (int j = 0; j < pieces[i].validMoves.Count; j += 2)
                {
                    moves++;
                    str += "From " + pieces[i].i + ", " + pieces[i].j + " to " + pieces[i].validMoves[j] + ", " + pieces[i].validMoves[j + 1] + "\n";
                }
        }
        str += "White Moves: " + moves + "\n";
        moves = 0;
        str += "Blacks:\n";
        for (int i = 16; i < 32; i++)
        {
            if (pieces[i].id != -1)
                for (int j = 0; j < pieces[i].validMoves.Count; j += 2)
                {
                    moves++;
                    str += "From " + pieces[i].i + ", " + pieces[i].j + " to " + pieces[i].validMoves[j] + ", " + pieces[i].validMoves[j + 1] + "\n";
                }
        }
        str += "Black Moves: " + moves + "\n";
        return str;
    }

    public void GenerateAllValidMoves()
    {
        whiteCheck = false;
        blackCheck = false;

        for (int a = 0; a < 32; a++)
        {
            if (pieces[a].id != -1 && pieces[a].pieceType != 'k')
            {
                pieces[a].validMoves = MovesofPieces(pieces[a].i, pieces[a].j);

                /*
                string moves = "";
                for (int i = 0; i < pieces[a].validMoves.Count; i += 2)
                {
                    moves += pieces[a].validMoves[i] + ", " + pieces[a].validMoves[i + 1] + "\n";
                }
                MonoBehaviour.print(moves);
                */

            }
        }
        if (whiteCheck)
        {
            int num = NumberOfChechers(WhiteKingID);
            if (num == 1)
            {
                Check(WhiteKingID);
            }
            else if (num == 2)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (pieces[i].id != -1 && pieces[i].pieceType != 'k')
                    {
                        pieces[i].validMoves.Clear();
                    }
                }
            }
        }
        pieces[WhiteKingID].validMoves = MovesofKing(WhiteKingID);






        if (blackCheck)
        {
            int num = NumberOfChechers(BlackKingID);
            if (num == 1)
            {
                Check(BlackKingID);
            }
            else if (num == 2)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (pieces[i].id != -1 && pieces[i].pieceType != 'k')
                    {
                        pieces[i].validMoves.Clear();
                    }
                }
            }
        }
        pieces[BlackKingID].validMoves = MovesofKing(BlackKingID);

        whiteLost = true;
        blackLost = true;

        for (int a = 0; a < 16; a++)
        {
            if (pieces[a].id != -1 && pieces[a].validMoves.Count > 0)
            {
                whiteLost = false;
                break;
            }
        }

        for (int a = 16; a < 32; a++)
        {
            if (pieces[a].id != -1 && pieces[a].validMoves.Count > 0)
            {
                blackLost = false;
                break;
            }
        }

    }

    public void Check(int kingId)
    {
        if (pieces[checker].pieceType == 'p' || pieces[checker].pieceType == 'n')
        {
            if (pieces[kingId].color == Color.white)
            {
                int i = pieces[checker].i;
                int j = pieces[checker].j;
                for (int k = 0; k < 16; k++)
                {
                    if (pieces[k].id != -1 && pieces[k].pieceType != 'k')
                    {
                        if (pieces[k].pieceType == 'p')
                        {
                            pieces[k].validMoves.Clear();
                            if (pieces[k].i + 1 == i && pieces[k].j + 1 == j && pieces[k].i + 1 >= 0 && pieces[k].j + 1 < 8)
                            {
                                pieces[k].validMoves.Add(pieces[k].i + 1);
                                pieces[k].validMoves.Add(pieces[k].j + 1);
                            }
                            else if (pieces[k].i + 1 == i && pieces[k].j - 1 == j && pieces[k].i + 1 >= 0 && pieces[k].j - 1 >= 0)
                            {
                                pieces[k].validMoves.Add(pieces[k].i + 1);
                                pieces[k].validMoves.Add(pieces[k].j - 1);
                            }
                        }
                        else if (pieces[k].pieceType == 'n')
                        {
                            List<int> tmp = new List<int>();
                            for (int l = 0; l < pieces[k].validMoves.Count; l += 2)
                            {
                                if (pieces[k].validMoves[l] == pieces[checker].i && pieces[k].validMoves[l + 1] == pieces[checker].j)
                                {
                                    tmp.Add(pieces[k].validMoves[l]);
                                    tmp.Add(pieces[k].validMoves[l] + 1);
                                }
                            }
                            pieces[k].validMoves = tmp;
                        }
                        else
                        {
                            List<int> v = new List<int>();
                            for (int a = 0; a < pieces[k].validMoves.Count; a += 2)
                            {
                                if (pieces[k].validMoves[a] == i && pieces[k].validMoves[a + 1] == j)
                                {
                                    v.Add(pieces[k].validMoves[a]);
                                    v.Add(pieces[k].validMoves[a + 1]);
                                }
                            }
                            pieces[k].validMoves = v;
                        }
                    }
                }
            }
            else
            {
                int i = pieces[checker].i;
                int j = pieces[checker].j;
                for (int k = 16; k < 32; k++)
                {
                    if (pieces[k].id != -1 && pieces[k].pieceType != 'k')
                    {
                        if (pieces[k].pieceType == 'p')
                        {
                            pieces[k].validMoves.Clear();
                            if (pieces[k].i - 1 == i && pieces[k].j + 1 == j && pieces[k].i - 1 >= 0 && pieces[k].j + 1 < 8)
                            {
                                pieces[k].validMoves.Add(pieces[k].i - 1);
                                pieces[k].validMoves.Add(pieces[k].j + 1);
                            }
                            else if (pieces[k].i - 1 == i && pieces[k].j - 1 == j && pieces[k].i - 1 >= 0 && pieces[k].j - 1 >= 0)
                            {
                                pieces[k].validMoves.Add(pieces[k].i - 1);
                                pieces[k].validMoves.Add(pieces[k].j - 1);
                            }
                        }
                        else if (pieces[k].pieceType == 'n')
                        {
                            List<int> tmp = new List<int>();
                            for (int l = 0; l < pieces[k].validMoves.Count; l += 2)
                            {
                                if (pieces[k].validMoves[l] == pieces[checker].i && pieces[k].validMoves[l + 1] == pieces[checker].j)
                                {
                                    tmp.Add(pieces[k].validMoves[l]);
                                    tmp.Add(pieces[k].validMoves[l] + 1);
                                }
                            }
                            pieces[k].validMoves = tmp;
                        }
                        else
                        {
                            List<int> v = new List<int>();
                            for (int a = 0; a < pieces[k].validMoves.Count; a += 2)
                            {
                                if (pieces[k].validMoves[a] == i && pieces[k].validMoves[a + 1] == j)
                                {
                                    v.Add(pieces[k].validMoves[a]);
                                    v.Add(pieces[k].validMoves[a + 1]);
                                }
                            }
                            pieces[k].validMoves = v;
                        }
                    }
                }
            }
        }
        else
        {


            
            // create ray between checker and king
            int i = pieces[checker].i;
            int j = pieces[checker].j;
            List<int> ray = new List<int>();
            if (pieces[checker].pieceType == 'r')
            {
                for (int a = 7; a >= 0; a--)
                {
                    if (i + a < 8 && i + a == pieces[kingId].i && j == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i + a << ", " << j << endl;
                            ray.Add(i + a);
                            ray.Add(j);
                            a--;
                        }
                        break;
                    }
                    if (i - a >= 0 && i - a == pieces[kingId].i && j == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i - a << ", " << j << endl;
                            ray.Add(i - a);
                            ray.Add(j);
                            a--;
                        }
                        break;
                    }
                    if (j + a < 8 && i == pieces[kingId].i && j + a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i << ", " << j + a << endl;
                            ray.Add(i);
                            ray.Add(j + a);
                            a--;
                        }
                        break;
                    }
                    if (j - a >= 0 && i == pieces[kingId].i && j - a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i << ", " << j - a << endl;
                            ray.Add(i);
                            ray.Add(j - a);
                            a--;
                        }
                        break;
                    }
                }
            }
            else if (pieces[checker].pieceType == 'b')
            {

                for (int a = 7; a >= 0; a--)
                {
                    if (i + a < 8 && j + a < 8 && i + a == pieces[kingId].i && j + a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i + a << ", " << j + a << endl;
                            ray.Add(i + a);
                            ray.Add(j + a);
                            a--;
                        }
                        break;
                    }
                    if (i - a >= 0 && j - a >= 0 && i - a == pieces[kingId].i && j - a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i - a << ", " << j - a << endl;
                            ray.Add(i - a);
                            ray.Add(j - a);
                            a--;
                        }
                        break;
                    }
                    if (j + a < 8 && i - a >= 0 && i - a == pieces[kingId].i && j + a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i - a << ", " << j + a << endl;
                            ray.Add(i - a);
                            ray.Add(j + a);
                            a--;
                        }
                        break;
                    }
                    if (j - a >= 0 && i + a < 8 && i + a == pieces[kingId].i && j - a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i + a << ", " << j - a << endl;
                            ray.Add(i + a);
                            ray.Add(j - a);
                            a--;
                        }
                        break;
                    }
                }

            }
            else if (pieces[checker].pieceType == 'q')
            {
                for (int a = 7; a >= 0; a--)
                {
                    if (i + a < 8 && i + a == pieces[kingId].i && j == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i + a << ", " << j << endl;
                            ray.Add(i + a);
                            ray.Add(j);
                            a--;
                        }
                        break;
                    }
                    if (i - a >= 0 && i - a == pieces[kingId].i && j == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i - a << ", " << j << endl;
                            ray.Add(i - a);
                            ray.Add(j);
                            a--;
                        }
                        break;
                    }
                    if (j + a < 8 && i == pieces[kingId].i && j + a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i << ", " << j + a << endl;
                            ray.Add(i);
                            ray.Add(j + a);
                            a--;
                        }
                        break;
                    }
                    if (j - a >= 0 && i == pieces[kingId].i && j - a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i << ", " << j - a << endl;
                            ray.Add(i);
                            ray.Add(j - a);
                            a--;
                        }
                        break;
                    }

                    if (i + a < 8 && j + a < 8 && i + a == pieces[kingId].i && j + a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i + a << ", " << j + a << endl;
                            ray.Add(i + a);
                            ray.Add(j + a);
                            a--;
                        }
                        break;
                    }
                    if (i - a >= 0 && j - a >= 0 && i - a == pieces[kingId].i && j - a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i - a << ", " << j - a << endl;
                            ray.Add(i - a);
                            ray.Add(j - a);
                            a--;
                        }
                        break;
                    }
                    if (j + a < 8 && i - a >= 0 && i - a == pieces[kingId].i && j + a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i - a << ", " << j + a << endl;
                            ray.Add(i - a);
                            ray.Add(j + a);
                            a--;
                        }
                        break;
                    }
                    if (j - a >= 0 && i + a < 8 && i + a == pieces[kingId].i && j - a == pieces[kingId].j)
                    {
                        a--;
                        while (a > 0)
                        {
                            //cout << "in ray: " << i + a << ", " << j - a << endl;
                            ray.Add(i + a);
                            ray.Add(j - a);
                            a--;
                        }
                        break;
                    }
                }
            }
            // then find all moves that pass through ray tiles to protect the king
            if (pieces[kingId].color == Color.white)
            {
                for (int a = 0; a < 16; a++)
                {
                    List<int> n = new List<int>();
                    for (int b = 0; b < pieces[a].validMoves.Count; b += 2)
                    {
                        for (int c = 0; c < ray.Count; c += 2)
                        {
                            if (pieces[a].validMoves[b] == ray[c] && pieces[a].validMoves[b + 1] == ray[c + 1])
                            {
                                n.Add(pieces[a].validMoves[b]);
                                n.Add(pieces[a].validMoves[b + 1]);
                            }
                        }

                        if (pieces[a].validMoves[b] == pieces[checker].i && pieces[a].validMoves[b + 1] == pieces[checker].j)
                        {
                            //cout << "pieces Type: " << pieces[a].pieceType << ". true for coords\n";
                            n.Add(pieces[a].validMoves[b]);
                            n.Add(pieces[a].validMoves[b + 1]);
                        }
                    }
                    pieces[a].validMoves = n;
                }
            }
            else
            {
                for (int a = 16; a < 32; a++)
                {
                    List<int> n = new List<int>();
                    for (int b = 0; b < pieces[a].validMoves.Count; b += 2)
                    {
                        for (int c = 0; c < ray.Count; c += 2)
                        {
                            if (pieces[a].validMoves[b] == ray[c] && pieces[a].validMoves[b + 1] == ray[c + 1])
                            {
                                n.Add(pieces[a].validMoves[b]);
                                n.Add(pieces[a].validMoves[b + 1]);
                            }
                        }

                        if (pieces[a].validMoves[b] == pieces[checker].i && pieces[a].validMoves[b + 1] == pieces[checker].j)
                        {
                            //cout << "pieces Type: " << pieces[a].pieceType << ". true for coords\n";
                            n.Add(pieces[a].validMoves[b]);
                            n.Add(pieces[a].validMoves[b + 1]);
                        }
                    }
                    pieces[a].validMoves = n;
                }
            }
        }

    }

    public List<int> MovesofPieces(int i, int j)
    {
        pieces[board[i, j].id].validMoves.Clear();
        List<int> m = new List<int>();
        List<int> canMove = CanMove(i, j);
        if (canMove[0] == (int)Movement.none)
            return m;
        else if(canMove[0] == (int)Movement.limited)
        {
            for(int a=1; a<canMove.Count; a++)
            {
                m.Add(canMove[a]);
            }
            return m;
        }

        char c = board[i,j].pieceType;
        if (c == 'p')
        {
            if (board[i,j].color == Color.black)
            {
                if (i == 6)
                {
                    if (j + 1 < 8 && i - 1 >= 0 && board[i - 1,j + 1].color != board[i,j].color && board[i - 1,j + 1].color != Color.none)
                    {
                        if (board[i - 1,j + 1].pieceType == 'k')
                        {
                            whiteCheck = true;
                            checker = board[i,j].id;
                        }
                        else
                        {
                            m.Add(i - 1);
                            m.Add(j + 1);
                        }
                    }
                    if (j - 1 >= 0 && i - 1 >= 0 && board[i - 1,j - 1].color != board[i,j].color && board[i - 1,j - 1].color != Color.none)
                    {
                        if (board[i - 1,j - 1].pieceType == 'k')
                        {
                            whiteCheck = true;
                            checker = board[i,j].id;
                        }
                        else
                        {
                            m.Add(i - 1);
                            m.Add(j - 1);
                        }
                    }
                    if (i - 1 >= 0 && board[i - 1,j].pieceType == '*')
                    {
                        m.Add(i - 1);
                        m.Add(j);
                        if (i - 2 >= 0 && board[i - 2,j].pieceType == '*')
                        {
                            m.Add(i - 2);
                            m.Add(j);
                        }
                    }
                }
                else
                {
                    if (j + 1 < 8 && i - 1 >= 0 && board[i - 1,j + 1].color != board[i,j].color && board[i - 1,j + 1].color != Color.none)
                    {
                        if (board[i - 1,j + 1].pieceType == 'k')
                        {
                            whiteCheck = true;
                            checker = board[i, j].id;
                        }
                        else
                        {
                            m.Add(i - 1);
                            m.Add(j + 1);
                        }
                    }
                    if (j - 1 >= 0 && i - 1 >= 0 && board[i - 1,j - 1].color != board[i,j].color && board[i - 1,j - 1].color != Color.none)
                    {
                        if (board[i - 1, j - 1].pieceType == 'k')
                        {
                            whiteCheck = true;
                            checker = board[i, j].id;
                        }
                        else
                        {
                            m.Add(i - 1);
                            m.Add(j - 1);
                        }
                    }
                    if (i - 1 >= 0 && board[i - 1,j].pieceType == '*')
                    {
                        m.Add(i - 1);
                        m.Add(j);
                    }
                }
            }
            else
            {
                if (i == 1)
                {
                    if (j + 1 < 8 && i + 1 < 8 && board[i + 1,j + 1].color != board[i,j].color && board[i + 1,j + 1].color != Color.none)
                    {
                        if (board[i + 1,j + 1].pieceType == 'k')
                        {
                            blackCheck = true;
                            checker = board[i,j].id;
                        }
                        else
                        {
                            m.Add(i + 1);
                            m.Add(j + 1);
                        }
                    }
                    if (j - 1 >= 0 && i + 1 < 8 && board[i + 1,j - 1].color != board[i,j].color && board[i + 1,j - 1].color != Color.none)
                    {
                        if (board[i + 1,j - 1].pieceType == 'k')
                        {
                            blackCheck = true;
                            checker = board[i, j].id;
                        }
                        else
                        {
                            m.Add(i + 1);
                            m.Add(j - 1);
                        }
                    }
                    if (i + 1 < 8 && board[i + 1,j].pieceType == '*')
                    {
                        m.Add(i + 1);
                        m.Add(j);
                        if (i + 2 < 8 && board[i + 2,j].pieceType == '*')
                        {
                            m.Add(i + 2);
                            m.Add(j);
                        }
                    }
                }
                else
                {
                    if (j + 1 < 8 && i + 1 < 8 && board[i + 1,j + 1].color != board[i,j].color && board[i + 1,j + 1].color != Color.none)
                    {
                        if (board[i + 1,j + 1].pieceType == 'k')
                        {
                            blackCheck = true;
                            checker = board[i, j].id;
                        }
                        else
                        {
                            m.Add(i + 1);
                            m.Add(j + 1);
                        }
                    }
                    if (j - 1 >= 0 && i + 1 < 8 && board[i + 1,j - 1].color != board[i,j].color && board[i + 1,j - 1].color != Color.none)
                    {
                        if (board[i + 1,j - 1].pieceType == 'k')
                        {
                            blackCheck = true;
                            checker = board[i, j].id;
                        }
                        else
                        {
                            m.Add(i + 1);
                            m.Add(j - 1);
                        }
                    }
                    if (i + 1 < 8 && board[i + 1,j].pieceType == '*')
                    {
                        m.Add(i + 1);
                        m.Add(j);
                    }
                }
            }
        }
        else if (c == 'r')
        {
            for (int a = 1; a < 8; a++)
            {
                if (i + a < 8 && board[i + a,j].color != board[i,j].color)
                {
                    if (board[i + a,j].color != Color.none)
                    {
                        if (board[i + a, j].pieceType == 'k')
                        {
                            if (board[i + a, j].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i + a);
                            m.Add(j);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i + a);
                        m.Add(j);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i - a >= 0 && board[i - a,j].color != board[i,j].color)
                {
                    if (board[i - a,j].color != Color.none)
                    {
                        if (board[i - a, j].pieceType == 'k')
                        {
                            if (board[i - a, j].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i - a);
                            m.Add(j);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i - a);
                        m.Add(j);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (j - a >= 0 && board[i,j - a].color != board[i,j].color)
                {
                    if (board[i,j - a].color != Color.none)
                    {
                        if (board[i, j - a].pieceType == 'k')
                        {
                            if (board[i, j - a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i);
                            m.Add(j - a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i);
                        m.Add(j - a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (j + a < 8 && board[i,j + a].color != board[i,j].color)
                {
                    if (board[i,j + a].color != Color.none)
                    {
                        if (board[i, j + a].pieceType == 'k')
                        {
                            if (board[i, j + a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i);
                            m.Add(j + a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i);
                        m.Add(j + a);
                    }
                }
                else
                    break;
            }
        }
        else if (c == 'n')
        {
            if (i + 2 < 8 && j + 1 < 8 && board[i + 2,j + 1].color != board[i,j].color)
            {
                if (board[i + 2,j + 1].pieceType == 'k')
                {
                    if (board[i + 2,j + 1].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i + 2);
                    m.Add(j + 1);
                }
            }
            if (i + 2 < 8 && j - 1 >= 0 && board[i + 2,j - 1].color != board[i,j].color)
            {
                if (board[i + 2,j - 1].pieceType == 'k')
                {
                    if (board[i + 2,j - 1].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i + 2);
                    m.Add(j - 1);
                }
            }
            if (i - 2 >= 0 && j + 1 < 8 && board[i - 2,j + 1].color != board[i,j].color)
            {
                if (board[i - 2,j + 1].pieceType == 'k')
                {
                    if (board[i - 2,j + 1].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i - 2);
                    m.Add(j + 1);
                }
            }
            if (i - 2 >= 0 && j - 1 >= 0 && board[i - 2,j - 1].color != board[i,j].color)
            {
                if (board[i - 2,j - 1].pieceType == 'k')
                {
                    if (board[i - 2,j - 1].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i - 2);
                    m.Add(j - 1);
                }
            }
            if (i + 1 < 8 && j + 2 < 8 && board[i + 1,j + 2].color != board[i,j].color)
            {
                if (board[i + 1,j + 2].pieceType == 'k')
                {
                    if (board[i + 1,j + 2].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i + 1);
                    m.Add(j + 2);
                }
            }
            if (i + 1 < 8 && j - 2 >= 0 && board[i + 1,j - 2].color != board[i,j].color)
            {
                if (board[i + 1,j - 2].pieceType == 'k')
                {
                    if (board[i + 1,j - 2].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i + 1);
                    m.Add(j - 2);
                }
            }
            if (i - 1 >= 0 && j + 2 < 8 && board[i - 1,j + 2].color != board[i,j].color)
            {
                if (board[i - 1,j + 2].pieceType == 'k')
                {
                    if (board[i - 1,j + 2].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i - 1);
                    m.Add(j + 2);
                }
            }
            if (i - 1 >= 0 && j - 2 >= 0 && board[i - 1,j - 2].color != board[i,j].color)
            {
                if (board[i - 1,j - 2].pieceType == 'k')
                {
                    if (board[i - 1,j - 2].color == Color.white)
                        whiteCheck = true;
                    else
                        blackCheck = true;
                    checker = board[i,j].id;
                }
                else
                {
                    m.Add(i - 1);
                    m.Add(j - 2);
                }
            }
        }
        else if (c == 'b')
        {
            for (int a = 1; a < 8; a++)
            {
                if (i + a < 8 && j + a < 8 && board[i + a,j + a].color != board[i,j].color)
                {
                    if (board[i + a,j + a].color != Color.none)
                    {
                        if (board[i + a, j + a].pieceType == 'k')
                        {
                            if (board[i + a, j + a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i + a);
                            m.Add(j + a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i + a);
                        m.Add(j + a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i - a >= 0 && j - a >= 0 && board[i - a,j - a].color != board[i,j].color)
                {
                    if (board[i - a,j - a].color != Color.none)
                    {
                        if (board[i - a, j - a].pieceType == 'k')
                        {
                            if (board[i - a, j - a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i - a);
                            m.Add(j - a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i - a);
                        m.Add(j - a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i + a < 8 && j - a >= 0 && board[i + a,j - a].color != board[i,j].color)
                {
                    if (board[i + a,j - a].color != Color.none)
                    {
                        if (board[i + a, j - a].pieceType == 'k')
                        {
                            if (board[i + a, j - a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i + a);
                            m.Add(j - a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i + a);
                        m.Add(j - a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i - a >= 0 && j + a < 8 && board[i - a,j + a].color != board[i,j].color)
                {
                    if (board[i - a,j + a].color != Color.none)
                    {
                        if (board[i - a, j + a].pieceType == 'k')
                        {
                            if (board[i - a, j + a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i - a);
                            m.Add(j + a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i - a);
                        m.Add(j + a);
                    }
                }
                else
                    break;
            }
        }
        else if (c == 'q')
        {
            for (int a = 1; a < 8; a++)
            {
                if (i + a < 8 && board[i + a, j].color != board[i, j].color)
                {
                    if (board[i + a, j].color != Color.none)
                    {
                        if (board[i + a, j].pieceType == 'k')
                        {
                            if (board[i + a, j].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i + a);
                            m.Add(j);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i + a);
                        m.Add(j);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i - a >= 0 && board[i - a, j].color != board[i, j].color)
                {
                    if (board[i - a, j].color != Color.none)
                    {
                        if (board[i - a, j].pieceType == 'k')
                        {
                            if (board[i - a, j].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i - a);
                            m.Add(j);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i - a);
                        m.Add(j);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (j - a >= 0 && board[i, j - a].color != board[i, j].color)
                {
                    if (board[i, j - a].color != Color.none)
                    {
                        if (board[i, j - a].pieceType == 'k')
                        {
                            if (board[i, j - a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i);
                            m.Add(j - a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i);
                        m.Add(j - a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (j + a < 8 && board[i, j + a].color != board[i, j].color)
                {
                    if (board[i, j + a].color != Color.none)
                    {
                        if (board[i, j + a].pieceType == 'k')
                        {
                            if (board[i, j + a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i);
                            m.Add(j + a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i);
                        m.Add(j + a);
                    }
                }
                else
                    break;
            }


            for (int a = 1; a < 8; a++)
            {
                if (i + a < 8 && j + a < 8 && board[i + a, j + a].color != board[i, j].color)
                {
                    if (board[i + a, j + a].color != Color.none)
                    {
                        if (board[i + a, j + a].pieceType == 'k')
                        {
                            if (board[i + a, j + a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i + a);
                            m.Add(j + a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i + a);
                        m.Add(j + a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i - a >= 0 && j - a >= 0 && board[i - a, j - a].color != board[i, j].color)
                {
                    if (board[i - a, j - a].color != Color.none)
                    {
                        if (board[i - a, j - a].pieceType == 'k')
                        {
                            if (board[i - a, j - a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i - a);
                            m.Add(j - a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i - a);
                        m.Add(j - a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i + a < 8 && j - a >= 0 && board[i + a, j - a].color != board[i, j].color)
                {
                    if (board[i + a, j - a].color != Color.none)
                    {
                        if (board[i + a, j - a].pieceType == 'k')
                        {
                            if (board[i + a, j - a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i + a);
                            m.Add(j - a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i + a);
                        m.Add(j - a);
                    }
                }
                else
                    break;
            }
            for (int a = 1; a < 8; a++)
            {
                if (i - a >= 0 && j + a < 8 && board[i - a, j + a].color != board[i, j].color)
                {
                    if (board[i - a, j + a].color != Color.none)
                    {
                        if (board[i - a, j + a].pieceType == 'k')
                        {
                            if (board[i - a, j + a].color == Color.white)
                                whiteCheck = true;
                            else
                                blackCheck = true;
                            checker = board[i, j].id;
                            break;
                        }
                        else
                        {
                            m.Add(i - a);
                            m.Add(j + a);
                            break;
                        }
                    }
                    else
                    {
                        m.Add(i - a);
                        m.Add(j + a);
                    }
                }
                else
                    break;
            }
        }

        return m;
    }

    public string ShowValidMoves(int i, int j)
    {
        string str = "";
        for (int a = 0; a < pieces[board[i,j].id].validMoves.Count; a += 2)
        {
           str +=  "From " + i + ", " + j + " to " + pieces[board[i,j].id].validMoves[a] + ", " + pieces[board[i,j].id].validMoves[a + 1] + "\n";
        }
        return str;
    }

    public List<int> MovesofKing(int kingid)
    {
        pieces[kingid].validMoves.Clear();
        List<int> m = new List<int>();
        int i = pieces[kingid].i;
        int j = pieces[kingid].j;

        if (j + 1 < 8 && CheckIfTile(kingid, i, j + 1))
        {
            m.Add(i);
            m.Add(j + 1);
        }

        if (j - 1 >= 0 && CheckIfTile(kingid, i, j - 1))
        {
            m.Add(i);
            m.Add(j - 1);
        }

        if (i + 1 < 8 && CheckIfTile(kingid, i + 1, j))
        {
            m.Add(i + 1);
            m.Add(j);
        }

        if (i - 1 >= 0 && CheckIfTile(kingid, i - 1, j))
        {
            m.Add(i - 1);
            m.Add(j);
        }

        if (i + 1 < 8 && j + 1 < 8 && CheckIfTile(kingid, i + 1, j + 1))
        {
            m.Add(i + 1);
            m.Add(j + 1);
            //
        }

        if (i - 1 >= 0 && j + 1 < 8 && CheckIfTile(kingid, i - 1, j + 1))
        {
            m.Add(i - 1);
            m.Add(j + 1);
        }

        if (i + 1 < 8 && j - 1 >= 0 && CheckIfTile(kingid, i + 1, j - 1))
        {
            m.Add(i + 1);
            m.Add(j - 1);
        }

        if (i - 1 >= 0 && j - 1 >= 0 && CheckIfTile(kingid, i - 1, j - 1))
        {
            m.Add(i - 1);
            m.Add(j - 1);
        }

        if(pieces[kingid].color == Color.white && kingcastling && !whiteCheck)
        {
            if(board[0,1].pieceType == '*' && board[0,2].pieceType == '*' && board[0, 3].pieceType == '*' && r1castling)
            {
                if(CheckIfTile(kingid, 0, 2) && CheckIfTile(kingid, 0, 3))
                {
                    m.Add(0);
                    m.Add(2);
                }
            }
            if(board[0, 5].pieceType == '*' && board[0, 6].pieceType == '*' && r2castling)
            {
                if (CheckIfTile(kingid, 0, 5) && CheckIfTile(kingid, 0, 6))
                {
                    m.Add(0);
                    m.Add(6);
                }
            }
        }
        else if (pieces[kingid].color == Color.black && kingcastlingblack && !blackCheck)
        {
            if (board[7, 1].pieceType == '*' && board[7, 2].pieceType == '*' && board[7, 3].pieceType == '*' && r1castlingblack)
            {
                if (CheckIfTile(kingid, 7, 1) && CheckIfTile(kingid, 7, 2) && CheckIfTile(kingid, 7, 3))
                {
                    m.Add(7);
                    m.Add(2);
                }
            }
            if (board[7, 5].pieceType == '*' && board[7, 6].pieceType == '*' && r2castlingblack)
            {
                if (CheckIfTile(kingid, 7, 5) && CheckIfTile(kingid, 7, 6))
                {
                    m.Add(7);
                    m.Add(6);
                }
            }
        }
        return m;
    }

    public bool CheckIfTile(int kingId, int i, int j)
    {
        if (pieces[kingId].color == Color.white)
        {
            if (board[i, j].color == Color.white)
                return false;
            if (Mathf.Abs(pieces[BlackKingID].i - i) < 2 && Mathf.Abs(pieces[BlackKingID].j - j) < 2)
                return false;

            // check if there is a rook or queen horizontally or vetrically
            for (int k = 1; k < 8; k++)
            {
                if (i + k <= 7)
                {
                    if (board[i + k, j].color != Color.none)
                    {
                        if (board[i + k, j].color == Color.black && (board[i + k, j].pieceType == 'r' || board[i + k, j].pieceType == 'q'))
                            return false;
                        else if(!(board[i + k, j].color == Color.white && board[i + k, j].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (i - k >= 0)
                {
                    if (board[i - k, j].color != Color.none)
                    {
                        if (board[i - k, j].color == Color.black && (board[i - k, j].pieceType == 'r' || board[i - k, j].pieceType == 'q'))
                            return false;
                        else if (!(board[i - k, j].color == Color.white && board[i - k, j].pieceType == 'k'))
                            break;
                    }
                }
            }

            for (int k = 1; k < 8; k++)
            {
                if (j + k <= 7)
                {
                    if (board[i, j + k].color != Color.none)
                    {
                        if (board[i, j + k].color == Color.black && (board[i, j + k].pieceType == 'r' || board[i, j + k].pieceType == 'q'))
                            return false;
                        else if (!(board[i, j + k].color == Color.white && board[i, j + k].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (j - k >= 0)
                {
                    if (board[i, j - k].color != Color.none)
                    {
                        if (board[i, j - k].color == Color.black && (board[i, j - k].pieceType == 'r' || board[i, j - k].pieceType == 'q'))
                            return false;
                        else if (!(board[i, j - k].color == Color.white && board[i, j - k].pieceType == 'k'))
                            break;
                    }
                }
            }

            // check if there is a bishop or qween diagonally
            for (int k = 1; k < 8; k++)
            {
                if (i + k <= 7 && j + k <= 7)
                {
                    if (board[i + k, j + k].color != Color.none)
                    {
                        if (board[i + k, j + k].color == Color.black && (board[i + k, j + k].pieceType == 'b' || board[i + k, j + k].pieceType == 'q'))
                            return false;
                        else if (!(board[i + k, j + k].color == Color.white && board[i + k, j + k].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (i - k >= 0 && j + k <= 7)
                {
                    if (board[i - k, j + k].color != Color.none)
                    {
                        if (board[i - k, j + k].color == Color.black && (board[i - k, j + k].pieceType == 'b' || board[i - k, j + k].pieceType == 'q'))
                            return false;
                        else if (!(board[i - k, j + k].color == Color.white && board[i - k, j + k].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (i + k <= 7 && j - k >= 0)
                {
                    if (board[i + k, j - k].color != Color.none)
                    {
                        if (board[i + k, j - k].color == Color.black && (board[i + k, j - k].pieceType == 'b' || board[i + k, j - k].pieceType == 'q'))
                            return false;
                        else if (!(board[i + k, j - k].color == Color.white && board[i + k, j - k].pieceType == 'k'))
                            break;
                    }
                }
            }
            
            for (int k = 1; k < 8; k++)
            {
                if (i - k >= 0 && j - k >= 0)
                {
                    if (board[i - k, j - k].color != Color.none)
                    {
                        if (board[i - k, j - k].color == Color.black && (board[i - k, j - k].pieceType == 'b' || board[i - k, j - k].pieceType == 'q'))
                            return false;
                        else if (!(board[i - k, j - k].color == Color.white && board[i - k, j - k].pieceType == 'k'))
                            break;
                    }
                }
            }

            // check if enemy pawn can move diagonally to tile
            if(i + 1 <= 7 && j + 1 <=7 && board[i + 1, j + 1].pieceType == 'p' && board[i + 1, j + 1].color == Color.black)
            {
                return false;
            }
            if (i + 1 <= 7 && j - 1 >= 0 && board[i + 1, j - 1].pieceType == 'p' && board[i + 1, j - 1].color == Color.black)
            {
                return false;
            }

            // check if enemy knight can move to tile
            if (i + 1 <= 7 && j + 2 <= 7 && board[i + 1, j + 2].pieceType == 'n' && board[i + 1, j + 2].color == Color.black)
            {
                return false;
            }
            if (i + 1 <= 7 && j - 2 >= 0 && board[i + 1, j - 2].pieceType == 'n' && board[i + 1, j - 2].color == Color.black)
            {
                return false;
            }
            if (i - 1 >= 0 && j + 2 <= 7 && board[i - 1, j + 2].pieceType == 'n' && board[i - 1, j + 2].color == Color.black)
            {
                return false;
            }
            if (i - 1 >= 0 && j - 2 >= 0 && board[i - 1, j - 2].pieceType == 'n' && board[i - 1, j - 2].color == Color.black)
            {
                return false;
            }

            if (i + 2 <= 7 && j + 1 <= 7 && board[i + 2, j + 1].pieceType == 'n' && board[i + 2, j + 1].color == Color.black)
            {
                return false;
            }
            if (i + 2 <= 7 && j - 1 >= 0 && board[i + 2, j - 1].pieceType == 'n' && board[i + 2, j - 1].color == Color.black)
            {
                return false;
            }
            if (i - 2 >= 0 && j + 1 <= 7 && board[i - 2, j + 1].pieceType == 'n' && board[i - 2, j + 1].color == Color.black)
            {
                return false;
            }
            if (i - 2 >= 0 && j - 1 >= 0 && board[i - 2, j - 1].pieceType == 'n' && board[i - 2, j - 1].color == Color.black)
            {
                return false;
            }


        }
        else
        {
            if (board[i, j].color == Color.black)
                return false;
            if (Mathf.Abs(pieces[WhiteKingID].i - i) < 2 && Mathf.Abs(pieces[WhiteKingID].j - j) < 2)
                return false;

            for (int k = 1; k < 8; k++)
            {
                if (i + k <= 7)
                {
                    if (board[i + k, j].color != Color.none)
                    {
                        if (board[i + k, j].color == Color.white && (board[i + k, j].pieceType == 'r' || board[i + k, j].pieceType == 'q'))
                            return false;
                        else if (!(board[i + k, j].color == Color.black && board[i + k, j].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (i - k >= 0)
                {
                    if (board[i - k, j].color != Color.none)
                    {
                        if (board[i - k, j].color == Color.white && (board[i - k, j].pieceType == 'r' || board[i - k, j].pieceType == 'q'))
                            return false;
                        else if (!(board[i - k, j].color == Color.black && board[i - k, j].pieceType == 'k'))
                            break;
                    }
                }
            }

            for (int k = 1; k < 8; k++)
            {
                if (j + k <= 7)
                {
                    if (board[i, j + k].color != Color.none)
                    {
                        if (board[i, j + k].color == Color.white && (board[i, j + k].pieceType == 'r' || board[i, j + k].pieceType == 'q'))
                            return false;
                        else if (!(board[i, j + k].color == Color.black && board[i, j + k].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (j - k >= 0)
                {
                    if (board[i, j - k].color != Color.none)
                    {
                        if (board[i, j - k].color == Color.white && (board[i, j - k].pieceType == 'r' || board[i, j - k].pieceType == 'q'))
                            return false;
                        else if (!(board[i, j - k].color == Color.black && board[i, j - k].pieceType == 'k'))
                            break;
                    }
                }
            }

            // check if there is a bishop or qween diagonally
            for (int k = 1; k < 8; k++)
            {
                if (i + k <= 7 && j + k <= 7)
                {
                    if (board[i + k, j + k].color != Color.none)
                    {
                        if (board[i + k, j + k].color == Color.white && (board[i + k, j + k].pieceType == 'b' || board[i + k, j + k].pieceType == 'q'))
                            return false;
                        else if (!(board[i + k, j + k].color == Color.black && board[i + k, j + k].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (i - k >= 0 && j + k <= 7)
                {
                    if (board[i - k, j + k].color != Color.none)
                    {
                        if (board[i - k, j + k].color == Color.white && (board[i - k, j + k].pieceType == 'b' || board[i - k, j + k].pieceType == 'q'))
                            return false;
                        else if (!(board[i - k, j + k].color == Color.black && board[i - k, j + k].pieceType == 'k'))
                            break;
                    }
                }
            }
            for (int k = 1; k < 8; k++)
            {
                if (i + k <= 7 && j - k >= 0)
                {
                    if (board[i + k, j - k].color != Color.none)
                    {
                        if (board[i + k, j - k].color == Color.white && (board[i + k, j - k].pieceType == 'b' || board[i + k, j - k].pieceType == 'q'))
                            return false;
                        else if (!(board[i + k, j - k].color == Color.black && board[i + k, j - k].pieceType == 'k'))
                            break;
                    }
                }
            }

            for (int k = 1; k < 8; k++)
            {
                if (i - k >= 0 && j - k >= 0)
                {
                    if (board[i - k, j - k].color != Color.none)
                    {
                        if (board[i - k, j - k].color == Color.white && (board[i - k, j - k].pieceType == 'b' || board[i - k, j - k].pieceType == 'q'))
                            return false;
                        else if (!(board[i - k, j - k].color == Color.black && board[i - k, j - k].pieceType == 'k'))
                            break;
                    }
                }
            }

            // check if enemy pawn can move diagonally to tile
            if (i - 1 >= 0 && j + 1 <= 7 && board[i - 1, j + 1].pieceType == 'p' && board[i - 1, j + 1].color == Color.white)
            {
                return false;
            }
            if (i - 1 >= 0 && j - 1 >= 0 && board[i - 1, j - 1].pieceType == 'p' && board[i - 1, j - 1].color == Color.white)
            {
                return false;
            }

            // check if enemy knight can move to tile
            if (i + 1 <= 7 && j + 2 <= 7 && board[i + 1, j + 2].pieceType == 'n' && board[i + 1, j + 2].color == Color.white)
            {
                return false;
            }
            if (i + 1 <= 7 && j - 2 >= 0 && board[i + 1, j - 2].pieceType == 'n' && board[i + 1, j - 2].color == Color.white)
            {
                return false;
            }
            if (i - 1 >= 0 && j + 2 <= 7 && board[i - 1, j + 2].pieceType == 'n' && board[i - 1, j + 2].color == Color.white)
            {
                return false;
            }
            if (i - 1 >= 0 && j - 2 >= 0 && board[i - 1, j - 2].pieceType == 'n' && board[i - 1, j - 2].color == Color.white)
            {
                return false;
            }

            if (i + 2 <= 7 && j + 1 <= 7 && board[i + 2, j + 1].pieceType == 'n' && board[i + 2, j + 1].color == Color.white)
            {
                return false;
            }
            if (i + 2 <= 7 && j - 1 >= 0 && board[i + 2, j - 1].pieceType == 'n' && board[i + 2, j - 1].color == Color.white)
            {
                return false;
            }
            if (i - 2 >= 0 && j + 1 <= 7 && board[i - 2, j + 1].pieceType == 'n' && board[i - 2, j + 1].color == Color.white)
            {
                return false;
            }
            if (i - 2 >= 0 && j - 1 >= 0 && board[i - 2, j - 1].pieceType == 'n' && board[i - 2, j - 1].color == Color.white)
            {
                return false;
            }
        }
        return true;
    }

    public string GetPiecesInfo()
    {
        string str;
        str = "Whites:\n";
        for(int i=0; i<16; i++)
        {
            str += pieces[i].i + ", " + pieces[i].j + "\n";
        }
        str += "Blacks:\n";
        for (int i = 16; i < 32; i++)
        {
            str += pieces[i].i + ", " + pieces[i].j + "\n";
        }
        return str;
    }

    public sbyte[,] CurrentBoard()
    {
        sbyte[,] state = new sbyte[8, 8];
        for(int i=0; i<8; i++)
        {
            for(int j=0; j<8; j++)
            {
                if (board[i, j].pieceType == '*')
                    state[i, j] = -1;
                else
                    state[i, j] = (sbyte) board[i, j].id;
            }
        }
        return state;
    }

    public sbyte[] FinalState()
    {
        sbyte[] array = new sbyte[89];
        int index = 0;

        // ids of all the pieces on the board
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                array[index] = board[i, j].id;
                index++;
            }
        }

        // piece type of all pawns of the game, incase a pawn has been promoted
        for (int i=8; i<24; i++)
        {
            
            array[index] = (sbyte)pieces[i].pieceType;
            index++;
        }

        // booleans (passed as signed bytes) representing the castling conditions
        array[index] = (sbyte)(kingcastling == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)(r1castling == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)(r2castling == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)(kingcastled == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)(kingcastlingblack == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)(r1castlingblack == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)(r2castlingblack == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)(kingcastledblack == true ? 't' : 'f');
        index++;
        array[index] = (sbyte)fiftyMoveRule;

        return array;
    }

    public void CreatePieces()
    {
        for(int i=8; i<16; i++)
        {
            pieces[i].pieceType = 'p';
            pieces[i].points = (int)PiecePoints.pawn;
        }

        pieces[0].pieceType = 'r';
        pieces[0].points= (int)PiecePoints.rook;

        pieces[1].pieceType = 'n';
        pieces[1].points = (int)PiecePoints.knight;

        pieces[2].pieceType = 'b';
        pieces[2].points = (int)PiecePoints.bishop;

        pieces[3].pieceType = 'q';
        pieces[3].points = (int)PiecePoints.queen;

        pieces[4].pieceType = 'k';
        pieces[4].points = 0;

        pieces[5].pieceType = 'b';
        pieces[5].points = (int)PiecePoints.bishop;

        pieces[6].pieceType = 'n';
        pieces[6].points = (int)PiecePoints.knight;

        pieces[7].pieceType = 'r';
        pieces[7].points = (int)PiecePoints.rook;

        for (int i = 16; i < 24; i++)
        {
            pieces[i].pieceType = 'p';
            pieces[i].points = (int)PiecePoints.pawn;
        }

        pieces[24].pieceType = 'r';
        pieces[24].points = (int)PiecePoints.rook;

        pieces[25].pieceType = 'n';
        pieces[25].points = (int)PiecePoints.knight;

        pieces[26].pieceType = 'b';
        pieces[26].points = (int)PiecePoints.bishop;

        pieces[27].pieceType = 'q';
        pieces[27].points = (int)PiecePoints.queen;

        pieces[28].pieceType = 'k';
        pieces[28].points = 0;

        pieces[29].pieceType = 'b';
        pieces[29].points = (int)PiecePoints.bishop;

        pieces[30].pieceType = 'n';
        pieces[30].points = (int)PiecePoints.knight;

        pieces[31].pieceType = 'r';
        pieces[31].points = (int)PiecePoints.rook;

        for (int i = 0; i < 16; i++)
        {
            pieces[i].color = Color.white;
        }
        for(int i=16; i<32; i++)
        {
            pieces[i].color = Color.black;
        }

        for(int i=0; i<32; i++)
        {
            pieces[i].id = (sbyte)i;
            pieces[i].validMoves = new List<int>();
        }

    }

    public List<int> CanMove(int i, int j)
    {
        char c = board[i, j].pieceType;
        List<int> m = new List<int>();
        if (board[i, j].color == Color.white)
        {
            int kingI = pieces[WhiteKingID].i;
            int kingJ = pieces[WhiteKingID].j;
            if (i == kingI)
            {
                if (j > kingJ)
                {
                    for (int k = 1; k < 8; k++)
                    {
                        if (j + k < 8 && board[i, j + k].color != Color.none)
                        {
                            if (board[i, j + k].color != board[i, j].color)
                            {
                                if (board[i, j + k].pieceType == 'r' || board[i, j + k].pieceType == 'q')
                                {
                                    for (int l = j - 1; l > kingJ; l--)
                                    {
                                        if (board[i, l].pieceType != '*')
                                        {
                                            m.Add((int)Movement.full);
                                            return m;
                                        }
                                    }

                                    if (c == 'r' || c == 'q')
                                    {
                                        m.Add((int)Movement.limited);

                                        for (int l = kingJ + 1; l <= j + k; l++)
                                        {
                                            if (l != j)
                                            {
                                                m.Add(i);
                                                m.Add(l);
                                            }
                                        }
                                        return m;
                                    }
                                    else
                                    {
                                        m.Add((int)Movement.none);
                                        return m;
                                    }
                                }
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else
                {
                    for (int k = 1; k < 8; k++)
                    {
                        if (j - k >= 0 && board[i, j - k].color != Color.none)
                        {
                            if (board[i, j - k].color != board[i, j].color)
                            {
                                if (board[i, j - k].pieceType == 'r' || board[i, j - k].pieceType == 'q')
                                {
                                    for (int l = j + 1; l < kingJ; l++)
                                    {
                                        if (board[i, l].pieceType != '*')
                                        {
                                            m.Add((int)Movement.full);
                                            return m;
                                        }
                                    }

                                    if (c == 'r' || c == 'q')
                                    {
                                        m.Add((int)Movement.limited);

                                        for (int l = kingJ - 1; l >= j - k; l--)
                                        {
                                            if (l != j)
                                            {
                                                m.Add(i);
                                                m.Add(l);
                                            }
                                        }
                                        return m;
                                    }
                                    else
                                    {
                                        m.Add((int)Movement.none);
                                        return m;
                                    }
                                }
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            else if (j == kingJ)
            {
                if (i > kingI)
                {
                    for (int k = 1; k < 8; k++)
                    {
                        if (i + k < 8 && board[i + k, j].color != Color.none)
                        {
                            if (board[i + k, j].color != board[i, j].color)
                            {
                                if (board[i + k, j].pieceType == 'r' || board[i + k, j].pieceType == 'q')
                                {
                                    for (int l = i - 1; l > kingI; l--)
                                    {
                                        if (board[l, j].pieceType != '*')
                                        {
                                            m.Add((int)Movement.full);
                                            return m;
                                        }
                                    }

                                    if (c == 'r' || c == 'q')
                                    {
                                        m.Add((int)Movement.limited);

                                        for (int l = kingI + 1; l <= i + k; l++)
                                        {
                                            if (l != i)
                                            {
                                                m.Add(l);
                                                m.Add(j);
                                            }
                                        }
                                        return m;
                                    }
                                    else if (c == 'p')
                                    {
                                        if (board[i + 1, j].pieceType == '*')
                                        {
                                            m.Add((int)Movement.limited);
                                            if (i == 1)
                                            {
                                                m.Add(i + 1);
                                                m.Add(j);
                                                if (board[i + 2, j].pieceType == '*')
                                                {
                                                    m.Add(i + 2);
                                                    m.Add(j);
                                                }
                                            }
                                            else
                                            {
                                                m.Add(i + 1);
                                                m.Add(j);
                                            }
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                        }
                                        return m;
                                    }
                                    else
                                    {
                                        m.Add((int)Movement.none);
                                        return m;
                                    }
                                }
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else
                {
                    for (int k = 1; k < 8; k++)
                    {
                        if (i - k >= 0 && board[i - k, j].color != Color.none)
                        {
                            if (board[i - k, j].color != board[i, j].color)
                            {
                                if (board[i - k, j].pieceType == 'r' || board[i - k, j].pieceType == 'q')
                                {
                                    for (int l = i + 1; l < kingI; l++)
                                    {
                                        if (board[l, j].pieceType != '*')
                                        {
                                            m.Add((int)Movement.full);
                                            return m;
                                        }
                                    }

                                    if (c == 'r' || c == 'q')
                                    {
                                        m.Add((int)Movement.limited);

                                        for (int l = kingI - 1; l >= i - k; l--)
                                        {
                                            if (l != i)
                                            {
                                                m.Add(l);
                                                m.Add(j);
                                            }
                                        }
                                        return m;
                                    }
                                    else if (c == 'p')
                                    {
                                        if (board[i + 1, j].pieceType == '*')
                                        {
                                            m.Add((int)Movement.limited);
                                            if (i == 1)
                                            {
                                                m.Add(i + 1);
                                                m.Add(j);
                                                if (board[i + 2, j].pieceType == '*')
                                                {
                                                    m.Add(i + 2);
                                                    m.Add(j);
                                                }
                                            }
                                            else
                                            {
                                                m.Add(i + 1);
                                                m.Add(j);
                                            }
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                        }
                                        return m;
                                    }
                                    else
                                    {
                                        m.Add((int)Movement.none);
                                        return m;
                                    }
                                }
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            else
            {
                for (int l = 1; l < 8; l++)
                {
                    if (kingI + l < 8 && kingJ + l < 8 && i == kingI + l && j == kingJ + l)
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (k + i < 8 && k + j < 8 && board[k + i, k + j].color != Color.none)
                            {
                                if (board[k + i, k + j].color != board[i, j].color)
                                {
                                    if (board[k + i, k + j].pieceType == 'b' || board[k + i, k + j].pieceType == 'q')
                                    {
                                        int q = j - 1;
                                        for (int p = i - 1; p > kingI; p--)
                                        {
                                            if (board[p, q].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                            q--;
                                        }

                                        if (c == 'b' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            int o = kingI + 1;
                                            for (int n = kingJ + 1; n <= j + k; n++)
                                            {
                                                if (n != j)
                                                {
                                                    m.Add(o);
                                                    m.Add(n);
                                                }
                                                o++;
                                            }
                                            return m;
                                        }
                                        else if (c == 'p')
                                        {
                                            if (board[k + i, k + j].id == board[i + 1, j + 1].id)
                                            {
                                                m.Add((int)Movement.limited);
                                                m.Add(i + 1);
                                                m.Add(j + 1);
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                    else if (kingI - l >= 0 && kingJ + l < 8 && i == kingI - l && j == kingJ + l)
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (i - k >= 0 && j + k < 8 && board[i - k, k + j].color != Color.none)
                            {
                                if (board[i - k, k + j].color != board[i, j].color)
                                {
                                    if (board[i - k, k + j].pieceType == 'b' || board[i - k, k + j].pieceType == 'q')
                                    {
                                        int q = j - 1;
                                        for (int p = i + 1; p < kingI; p++)
                                        {
                                            if (board[p, q].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                            q--;
                                        }

                                        if (c == 'b' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            int o = kingI - 1;
                                            for (int n = kingJ + 1; n <= j + k; n++)
                                            {
                                                if (n != j)
                                                {
                                                    m.Add(o);
                                                    m.Add(n);
                                                }
                                                o--;
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                    else if (kingI + l < 8 && kingJ - l >= 0 && i == kingI + l && j == kingJ - l)
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (k + i < 8 && j - k >= 0 && board[k + i, j - k].color != Color.none)
                            {
                                if (board[k + i, j - k].color != board[i, j].color)
                                {
                                    if (board[k + i, j - k].pieceType == 'b' || board[k + i, j - k].pieceType == 'q')
                                    {
                                        int q = j + 1;
                                        for (int p = i - 1; p > kingI; p--)
                                        {
                                            if (board[p, q].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                            q++;
                                        }

                                        if (c == 'b' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            int o = kingI + 1;
                                            for (int n = kingJ - 1; n >= j - k; n--)
                                            {
                                                if (n != j)
                                                {
                                                    m.Add(o);
                                                    m.Add(n);
                                                }
                                                o++;
                                            }
                                            return m;
                                        }
                                        else if (c == 'p')
                                        {
                                            if (board[k + i, j - k].id == board[i + 1, j - 1].id)
                                            {
                                                m.Add((int)Movement.limited);
                                                m.Add(i + 1);
                                                m.Add(j - 1);
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                    else if (kingI - l >= 0 && kingJ - l >= 0 && i == kingI - l && j == kingJ - l)
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (i - k >= 0 && j - k >= 0 && board[i - k, j - k].color != Color.none)
                            {
                                if (board[i - k, j - k].color != board[i, j].color)
                                {
                                    if (board[i - k, j - k].pieceType == 'b' || board[i - k, j - k].pieceType == 'q')
                                    {
                                        int q = j + 1;
                                        for (int p = i + 1; p < kingI; p++)
                                        {
                                            if (board[p, q].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                            q++;
                                        }

                                        if (c == 'b' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            int o = kingI - 1;
                                            for (int n = kingJ - 1; n >= j - k; n--)
                                            {
                                                if (n != j)
                                                {
                                                    m.Add(o);
                                                    m.Add(n);
                                                }
                                                o--;
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            {
                int kingI = pieces[BlackKingID].i;
                int kingJ = pieces[BlackKingID].j;
                if (i == kingI)
                {
                    if (j > kingJ)
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (j + k < 8 && board[i, j + k].color != Color.none)
                            {
                                if (board[i, j + k].color != board[i, j].color)
                                {
                                    if (board[i, j + k].pieceType == 'r' || board[i, j + k].pieceType == 'q')
                                    {
                                        for (int l = j - 1; l > kingJ; l--)
                                        {
                                            if (board[i, l].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                        }

                                        if (c == 'r' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            for (int l = kingJ + 1; l <= j + k; l++)
                                            {
                                                if (l != j)
                                                {
                                                    m.Add(i);
                                                    m.Add(l);
                                                }
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (j - k >= 0 && board[i, j - k].color != Color.none)
                            {
                                if (board[i, j - k].color != board[i, j].color)
                                {
                                    if (board[i, j - k].pieceType == 'r' || board[i, j - k].pieceType == 'q')
                                    {
                                        for (int l = j + 1; l < kingJ; l++)
                                        {
                                            if (board[i, l].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                        }

                                        if (c == 'r' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            for (int l = kingJ - 1; l >= j - k; l--)
                                            {
                                                if (l != j)
                                                {
                                                    m.Add(i);
                                                    m.Add(l);
                                                }
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                }
                else if (j == kingJ)
                {
                    if (i > kingI)
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (i + k < 8 && board[i + k, j].color != Color.none)
                            {
                                if (board[i + k, j].color != board[i, j].color)
                                {
                                    if (board[i + k, j].pieceType == 'r' || board[i + k, j].pieceType == 'q')
                                    {
                                        for (int l = i - 1; l > kingI; l--)
                                        {
                                            if (board[l, j].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                        }

                                        if (c == 'r' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            for (int l = kingI + 1; l <= i + k; l++)
                                            {
                                                if (l != i)
                                                {
                                                    m.Add(l);
                                                    m.Add(j);
                                                }
                                            }
                                            return m;
                                        }
                                        else if (c == 'p')
                                        {
                                            if (board[i - 1, j].pieceType == '*')
                                            {
                                                m.Add((int)Movement.limited);
                                                if (i == 6)
                                                {
                                                    m.Add(i - 1);
                                                    m.Add(j);
                                                    if (board[i - 2, j].pieceType == '*')
                                                    {
                                                        m.Add(i - 2);
                                                        m.Add(j);
                                                    }
                                                }
                                                else
                                                {
                                                    m.Add(i - 1);
                                                    m.Add(j);
                                                }
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 1; k < 8; k++)
                        {
                            if (i - k >= 0 && board[i - k, j].color != Color.none)
                            {
                                if (board[i - k, j].color != board[i, j].color)
                                {
                                    if (board[i - k, j].pieceType == 'r' || board[i - k, j].pieceType == 'q')
                                    {
                                        for (int l = i + 1; l < kingI; l++)
                                        {
                                            if (board[l, j].pieceType != '*')
                                            {
                                                m.Add((int)Movement.full);
                                                return m;
                                            }
                                        }

                                        if (c == 'r' || c == 'q')
                                        {
                                            m.Add((int)Movement.limited);

                                            for (int l = kingI - 1; l >= i - k; l--)
                                            {
                                                if (l != i)
                                                {
                                                    m.Add(l);
                                                    m.Add(j);
                                                }
                                            }
                                            return m;
                                        }
                                        else if (c == 'p')
                                        {
                                            if (board[i - 1, j].pieceType == '*')
                                            {
                                                m.Add((int)Movement.limited);
                                                if (i == 1)
                                                {
                                                    m.Add(i - 1);
                                                    m.Add(j);
                                                    if (board[i - 2, j].pieceType == '*')
                                                    {
                                                        m.Add(i - 2);
                                                        m.Add(j);
                                                    }
                                                }
                                                else
                                                {
                                                    m.Add(i - 1);
                                                    m.Add(j);
                                                }
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                            }
                                            return m;
                                        }
                                        else
                                        {
                                            m.Add((int)Movement.none);
                                            return m;
                                        }
                                    }
                                    else break;
                                }
                                else break;
                            }
                        }
                    }
                }
                else
                {
                    for (int l = 1; l < 8; l++)
                    {
                        if (kingI + l < 8 && kingJ + l < 8 && i == kingI + l && j == kingJ + l)
                        {
                            for (int k = 1; k < 8; k++)
                            {
                                if (k + i < 8 && k + j < 8 && board[k + i, k + j].color != Color.none)
                                {
                                    if (board[k + i, k + j].color != board[i, j].color)
                                    {
                                        if (board[k + i, k + j].pieceType == 'b' || board[k + i, k + j].pieceType == 'q')
                                        {
                                            int q = j - 1;
                                            for (int p = i - 1; p > kingI; p--)
                                            {
                                                if (board[p, q].pieceType != '*')
                                                {
                                                    m.Add((int)Movement.full);
                                                    return m;
                                                }
                                                q--;
                                            }

                                            if (c == 'b' || c == 'q')
                                            {
                                                m.Add((int)Movement.limited);

                                                int o = kingI + 1;
                                                for (int n = kingJ + 1; n <= j + k; n++)
                                                {
                                                    if (n != j)
                                                    {
                                                        m.Add(o);
                                                        m.Add(n);
                                                    }
                                                    o++;
                                                }
                                                return m;
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                                return m;
                                            }
                                        }
                                        else break;
                                    }
                                    else break;
                                }
                            }
                        }
                        else if (kingI - l >= 0 && kingJ + l < 8 && i == kingI - l && j == kingJ + l)
                        {
                            for (int k = 1; k < 8; k++)
                            {
                                if (i - k >= 0 && j + k < 8 && board[i - k, k + j].color != Color.none)
                                {
                                    if (board[i - k, k + j].color != board[i, j].color)
                                    {
                                        if (board[i - k, k + j].pieceType == 'b' || board[i - k, k + j].pieceType == 'q')
                                        {
                                            int q = j - 1;
                                            for (int p = i + 1; p < kingI; p++)
                                            {
                                                if (board[p, q].pieceType != '*')
                                                {
                                                    m.Add((int)Movement.full);
                                                    return m;
                                                }
                                                q--;
                                            }

                                            if (c == 'b' || c == 'q')
                                            {
                                                m.Add((int)Movement.limited);

                                                int o = kingI - 1;
                                                for (int n = kingJ + 1; n <= j + k; n++)
                                                {
                                                    if (n != j)
                                                    {
                                                        m.Add(o);
                                                        m.Add(n);
                                                    }
                                                    o--;
                                                }
                                                return m;
                                            }
                                            else if (c == 'p')
                                            {
                                                if (board[i - k, k + j].id == board[i - 1, j + 1].id)
                                                {
                                                    m.Add((int)Movement.limited);
                                                    m.Add(i - 1);
                                                    m.Add(j + 1);
                                                }
                                                else
                                                {
                                                    m.Add((int)Movement.none);
                                                }
                                                return m;
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                                return m;
                                            }
                                        }
                                        else break;
                                    }
                                    else break;
                                }
                            }
                        }
                        else if (kingI + l < 8 && kingJ - l >= 0 && i == kingI + l && j == kingJ - l)
                        {
                            for (int k = 1; k < 8; k++)
                            {
                                if (k + i < 8 && j - k >= 0 && board[k + i, j - k].color != Color.none)
                                {
                                    if (board[k + i, j - k].color != board[i, j].color)
                                    {
                                        if (board[k + i, j - k].pieceType == 'b' || board[k + i, j - k].pieceType == 'q')
                                        {
                                            int q = j + 1;
                                            for (int p = i - 1; p > kingI; p--)
                                            {
                                                if (board[p, q].pieceType != '*')
                                                {
                                                    m.Add((int)Movement.full);
                                                    return m;
                                                }
                                                q++;
                                            }

                                            if (c == 'b' || c == 'q')
                                            {
                                                m.Add((int)Movement.limited);

                                                int o = kingI + 1;
                                                for (int n = kingJ - 1; n >= j - k; n--)
                                                {
                                                    if (n != j)
                                                    {
                                                        m.Add(o);
                                                        m.Add(n);
                                                    }
                                                    o++;
                                                }
                                                return m;
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                                return m;
                                            }
                                        }
                                        else break;
                                    }
                                    else break;
                                }
                            }
                        }
                        else if (kingI - l >= 0 && kingJ - l >= 0 && i == kingI - l && j == kingJ - l)
                        {
                            for (int k = 1; k < 8; k++)
                            {
                                if (i - k >= 0 && j - k >= 0 && board[i - k, j - k].color != Color.none)
                                {
                                    if (board[i - k, j - k].color != board[i, j].color)
                                    {
                                        if (board[i - k, j - k].pieceType == 'b' || board[i - k, j - k].pieceType == 'q')
                                        {
                                            int q = j + 1;
                                            for (int p = i + 1; p < kingI; p++)
                                            {
                                                if (board[p, q].pieceType != '*')
                                                {
                                                    m.Add((int)Movement.full);
                                                    return m;
                                                }
                                                q++;
                                            }

                                            if (c == 'b' || c == 'q')
                                            {
                                                m.Add((int)Movement.limited);

                                                int o = kingI - 1;
                                                for (int n = kingJ - 1; n >= j - k; n--)
                                                {
                                                    if (n != j)
                                                    {
                                                        m.Add(o);
                                                        m.Add(n);
                                                    }
                                                    o--;
                                                }
                                                return m;
                                            }
                                            else if (c == 'p')
                                            {
                                                if (board[i - k, j - k].id == board[i - 1, j - 1].id)
                                                {
                                                    m.Add((int)Movement.limited);
                                                    m.Add(i - 1);
                                                    m.Add(j - 1);
                                                }
                                                else
                                                {
                                                    m.Add((int)Movement.none);
                                                }
                                                return m;
                                            }
                                            else
                                            {
                                                m.Add((int)Movement.none);
                                                return m;
                                            }
                                        }
                                        else break;
                                    }
                                    else break;
                                }
                            }
                        }
                    }
                }
            }
        }
        m.Add((int)Movement.full);
        return m;
    }

    public int NumberOfChechers(int kingId)
    {
        int checkers = 0;
        int kingI = pieces[kingId].i;
        int kingJ = pieces[kingId].j;
        if (pieces[kingId].color == Color.white)
        {
            for (int i = 1; i < 8; i++)
            {
                if (kingI + i < 8)
                {
                    if (board[kingI + i, kingJ].pieceType != '*')
                    {
                        if (board[kingI + i, kingJ].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI + i, kingJ].pieceType == 'r' || board[kingI + i, kingJ].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI + i, kingJ].id;
                                }
                                else if(checkers == 2)
                                {
                                    checker2 = board[kingI + i, kingJ].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI - i >= 0)
                {
                    if (board[kingI - i, kingJ].pieceType != '*')
                    {
                        if (board[kingI - i, kingJ].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI - i, kingJ].pieceType == 'r' || board[kingI - i, kingJ].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI - i, kingJ].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI - i, kingJ].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingJ + i < 8)
                {
                    if (board[kingI, kingJ + i].pieceType != '*')
                    {
                        if (board[kingI, kingJ + i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI, kingJ + i].pieceType == 'r' || board[kingI, kingJ + i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI, kingJ + i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI, kingJ + i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingJ - i >= 0)
                {
                    if (board[kingI, kingJ - i].pieceType != '*')
                    {
                        if (board[kingI, kingJ - i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI, kingJ - i].pieceType == 'r' || board[kingI, kingJ - i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI, kingJ - i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI, kingJ - i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }



            for (int i = 1; i < 8; i++)
            {
                if (kingI + i < 8 && kingJ + i < 8)
                {
                    if (board[kingI + i, kingJ + i].pieceType != '*')
                    {
                        if (board[kingI + i, kingJ + i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI + i, kingJ + i].pieceType == 'b' || board[kingI + i, kingJ + i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI + i, kingJ + i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI + i, kingJ + i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI + i < 8 && kingJ - i >= 0)
                {
                    if (board[kingI + i, kingJ - i].pieceType != '*')
                    {
                        if (board[kingI + i, kingJ - i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI + i, kingJ - i].pieceType == 'b' || board[kingI + i, kingJ - i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI + i, kingJ - i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI + i, kingJ - i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI - i >= 0 && kingJ + i < 8)
                {
                    if (board[kingI - i, kingJ + i].pieceType != '*')
                    {
                        if (board[kingI - i, kingJ + i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI - i, kingJ + i].pieceType == 'b' || board[kingI - i, kingJ + i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI - i, kingJ + i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI - i, kingJ + i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI - i >= 0 && kingJ - i >= 0)
                {
                    if (board[kingI - i, kingJ - i].pieceType != '*')
                    {
                        if (board[kingI - i, kingJ - i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI - i, kingJ - i].pieceType == 'b' || board[kingI - i, kingJ - i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI - i, kingJ - i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI - i, kingJ - i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }

            if(kingI + 1 < 8 && kingJ + 2 < 8 && board[kingI + 1, kingJ + 2].color != board[kingI, kingJ].color && board[kingI + 1, kingJ + 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 1, kingJ + 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 1, kingJ + 2].id;
                    return checkers;
                }
            }
            if (kingI + 1 < 8 && kingJ - 2 >= 0 && board[kingI + 1, kingJ - 2].color != board[kingI, kingJ].color && board[kingI + 1, kingJ - 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 1, kingJ - 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 1, kingJ - 2].id;
                    return checkers;
                }
            }
            if (kingI - 1 >= 0 && kingJ + 2 < 8 && board[kingI - 1, kingJ + 2].color != board[kingI, kingJ].color && board[kingI - 1, kingJ + 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 1, kingJ + 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 1, kingJ + 2].id;
                    return checkers;
                }
            }
            if (kingI - 1 >= 0 && kingJ - 2 >= 0 && board[kingI - 1, kingJ - 2].color != board[kingI, kingJ].color && board[kingI - 1, kingJ - 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 1, kingJ - 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 1, kingJ - 2].id;
                    return checkers;
                }
            }

            if (kingI + 2 < 8 && kingJ + 1 < 8 && board[kingI + 2, kingJ + 1].color != board[kingI, kingJ].color && board[kingI + 2, kingJ + 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 2, kingJ + 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 2, kingJ + 1].id;
                    return checkers;
                }
            }
            if (kingI + 2 < 8 && kingJ - 1 >= 0 && board[kingI + 2, kingJ - 1].color != board[kingI, kingJ].color && board[kingI + 2, kingJ - 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 2, kingJ - 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 2, kingJ - 1].id;
                    return checkers;
                }
            }
            if (kingI - 2 >= 0 && kingJ + 1 < 8 && board[kingI - 2, kingJ + 1].color != board[kingI, kingJ].color && board[kingI - 2, kingJ + 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 2, kingJ + 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 2, kingJ + 1].id;
                    return checkers;
                }
            }
            if (kingI - 2 >= 0 && kingJ - 1 >= 0 && board[kingI - 2, kingJ - 1].color != board[kingI, kingJ].color && board[kingI - 2, kingJ - 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 2, kingJ - 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 2, kingJ - 1].id;
                    return checkers;
                }
            }

            if(kingI + 1 < 8 && kingJ + 1 < 8 && board[kingI + 1, kingJ + 1].color != board[kingI, kingJ].color && board[kingI + 1, kingJ + 1].pieceType == 'p')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 1, kingJ + 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 1, kingJ + 1].id;
                    return checkers;
                }
            }
            if (kingI + 1 < 8 && kingJ - 1 >= 0 && board[kingI + 1, kingJ - 1].color != board[kingI, kingJ].color && board[kingI + 1, kingJ - 1].pieceType == 'p')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 1, kingJ - 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 1, kingJ - 1].id;
                    return checkers;
                }
            }
        }
        else
        {
            for (int i = 1; i < 8; i++)
            {
                if (kingI + i < 8)
                {
                    if (board[kingI + i, kingJ].pieceType != '*')
                    {
                        if (board[kingI + i, kingJ].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI + i, kingJ].pieceType == 'r' || board[kingI + i, kingJ].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI + i, kingJ].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI + i, kingJ].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI - i >= 0)
                {
                    if (board[kingI - i, kingJ].pieceType != '*')
                    {
                        if (board[kingI - i, kingJ].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI - i, kingJ].pieceType == 'r' || board[kingI - i, kingJ].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI - i, kingJ].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI - i, kingJ].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingJ + i < 8)
                {
                    if (board[kingI, kingJ + i].pieceType != '*')
                    {
                        if (board[kingI, kingJ + i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI, kingJ + i].pieceType == 'r' || board[kingI, kingJ + i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI, kingJ + i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI, kingJ + i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingJ - i >= 0)
                {
                    if (board[kingI, kingJ - i].pieceType != '*')
                    {
                        if (board[kingI, kingJ - i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI, kingJ - i].pieceType == 'r' || board[kingI, kingJ - i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI, kingJ - i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI, kingJ - i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }



            for (int i = 1; i < 8; i++)
            {
                if (kingI + i < 8 && kingJ + i < 8)
                {
                    if (board[kingI + i, kingJ + i].pieceType != '*')
                    {
                        if (board[kingI + i, kingJ + i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI + i, kingJ + i].pieceType == 'b' || board[kingI + i, kingJ + i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI + i, kingJ + i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI + i, kingJ + i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI + i < 8 && kingJ - i >= 0)
                {
                    if (board[kingI + i, kingJ - i].pieceType != '*')
                    {
                        if (board[kingI + i, kingJ - i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI + i, kingJ - i].pieceType == 'b' || board[kingI + i, kingJ - i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI + i, kingJ - i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI + i, kingJ - i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI - i >= 0 && kingJ + i < 8)
                {
                    if (board[kingI - i, kingJ + i].pieceType != '*')
                    {
                        if (board[kingI - i, kingJ + i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI - i, kingJ + i].pieceType == 'b' || board[kingI - i, kingJ + i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI - i, kingJ + i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI - i, kingJ + i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                if (kingI - i >= 0 && kingJ - i >= 0)
                {
                    if (board[kingI - i, kingJ - i].pieceType != '*')
                    {
                        if (board[kingI - i, kingJ - i].color != board[kingI, kingJ].color)
                        {
                            if (board[kingI - i, kingJ - i].pieceType == 'b' || board[kingI - i, kingJ - i].pieceType == 'q')
                            {
                                checkers++;
                                if (checkers == 1)
                                {
                                    checker = board[kingI - i, kingJ - i].id;
                                }
                                else if (checkers == 2)
                                {
                                    checker2 = board[kingI - i, kingJ - i].id;
                                    return checkers;
                                }
                                break;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }

            if (kingI + 1 < 8 && kingJ + 2 < 8 && board[kingI + 1, kingJ + 2].color != board[kingI, kingJ].color && board[kingI + 1, kingJ + 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 1, kingJ + 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 1, kingJ + 2].id;
                    return checkers;
                }
            }
            if (kingI + 1 < 8 && kingJ - 2 >= 0 && board[kingI + 1, kingJ - 2].color != board[kingI, kingJ].color && board[kingI + 1, kingJ - 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 1, kingJ - 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 1, kingJ - 2].id;
                    return checkers;
                }
            }
            if (kingI - 1 >= 0 && kingJ + 2 < 8 && board[kingI - 1, kingJ + 2].color != board[kingI, kingJ].color && board[kingI - 1, kingJ + 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 1, kingJ + 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 1, kingJ + 2].id;
                    return checkers;
                }
            }
            if (kingI - 1 >= 0 && kingJ - 2 >= 0 && board[kingI - 1, kingJ - 2].color != board[kingI, kingJ].color && board[kingI - 1, kingJ - 2].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 1, kingJ - 2].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 1, kingJ - 2].id;
                    return checkers;
                }
            }

            if (kingI + 2 < 8 && kingJ + 1 < 8 && board[kingI + 2, kingJ + 1].color != board[kingI, kingJ].color && board[kingI + 2, kingJ + 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 2, kingJ + 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 2, kingJ + 1].id;
                    return checkers;
                }
            }
            if (kingI + 2 < 8 && kingJ - 1 >= 0 && board[kingI + 2, kingJ - 1].color != board[kingI, kingJ].color && board[kingI + 2, kingJ - 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI + 2, kingJ - 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI + 2, kingJ - 1].id;
                    return checkers;
                }
            }
            if (kingI - 2 >= 0 && kingJ + 1 < 8 && board[kingI - 2, kingJ + 1].color != board[kingI, kingJ].color && board[kingI - 2, kingJ + 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 2, kingJ + 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 2, kingJ + 1].id;
                    return checkers;
                }
            }
            if (kingI - 2 >= 0 && kingJ - 1 >= 0 && board[kingI - 2, kingJ - 1].color != board[kingI, kingJ].color && board[kingI - 2, kingJ - 1].pieceType == 'n')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 2, kingJ - 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 2, kingJ - 1].id;
                    return checkers;
                }
            }

            if (kingI - 1 >= 0 && kingJ + 1 < 8 && board[kingI - 1, kingJ + 1].color != board[kingI, kingJ].color && board[kingI - 1, kingJ + 1].pieceType == 'p')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 1, kingJ + 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 1, kingJ + 1].id;
                    return checkers;
                }
            }
            if (kingI - 1 >= 0 && kingJ - 1 >= 0 && board[kingI - 1, kingJ - 1].color != board[kingI, kingJ].color && board[kingI - 1, kingJ - 1].pieceType == 'p')
            {
                checkers++;
                if (checkers == 1)
                {
                    checker = board[kingI - 1, kingJ - 1].id;
                }
                else if (checkers == 2)
                {
                    checker2 = board[kingI - 1, kingJ - 1].id;
                    return checkers;
                }
            }
        }
        return checkers;
    }

    public void Promote(int id, int[] array)
    {
        pieces[id].pieceType = (char)array[0];
        pieces[id].points = array[1];

        board[pieces[id].i, pieces[id].j].pieceType = (char)array[0];
    }

}

public struct Piece
{
    public sbyte id;
    public Color color;
    public char pieceType;
    public int i;
    public int j;
    public int points;
    public List<int> validMoves;
}

public enum Color
{
    white, black, none
}

public enum Movement
{
    none, limited, full
}

public enum PiecePoints : int
{
    pawn = 100, rook = 500, knight = 300, bishop = 300, queen = 900
}
