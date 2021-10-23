using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Minimax
{

    static readonly int Min = -200000;
    static readonly int Max = 200000;
    public static int loops = 0;

    public static int[] GetBestMove(Board board, int Mdepth, bool MaxPlayer)
    {
        if (Mdepth <= 0)
        {
            return new int[] { -1, -1, -1, -1, Minimax.Evaluate(board) }; // if checkmate, return 100000 or -100000
        }
        else if (board.whiteLost)
        {
            return new int[] { -1, -1, -1, -1, -1000000 };
        }
        else if (board.blackLost)
        {
            return new int[] { -1, -1, -1, -1, 1000000 };
        }

        Board bo = new Board(board);

        int[] best = new int[5];
        int[] eval = new int[5];
        if (MaxPlayer)
        {
            int max = Min;
            for (int i = 16; i < 32; i++)
            {
                if (bo.pieces[i].id != -1)
                {
                    for (int j = 0; j < bo.pieces[i].validMoves.Count; j += 2)
                    {
                        bo.Move(bo.pieces[i].i, bo.pieces[i].j, bo.pieces[i].validMoves[j], bo.pieces[i].validMoves[j + 1]);
                        eval = GetBestMove(bo, Mdepth - 1, false);
                        bo = new Board(board);
                        if (eval[4] > max)
                        {
                            best[0] = bo.pieces[i].i;
                            best[1] = bo.pieces[i].j;
                            best[2] = bo.pieces[i].validMoves[j];
                            best[3] = bo.pieces[i].validMoves[j + 1];
                            best[4] = eval[4];
                            max = eval[4];
                        }
                    }
                }
            }
            return best;
        }
        else
        {
            int min = Max;
            for (int i = 0; i < 16; i++)
            {
                if (bo.pieces[i].id != -1)
                {
                    for (int j = 0; j < bo.pieces[i].validMoves.Count; j += 2)
                    {
                        bo.Move(bo.pieces[i].i, bo.pieces[i].j, bo.pieces[i].validMoves[j], bo.pieces[i].validMoves[j + 1]);
                        eval = GetBestMove(bo, Mdepth - 1, true);
                        bo = new Board(board);
                        if (eval[4] < min)
                        {
                            best[0] = bo.pieces[i].i;
                            best[1] = bo.pieces[i].j;
                            best[2] = bo.pieces[i].validMoves[j];
                            best[3] = bo.pieces[i].validMoves[j + 1];
                            best[4] = eval[4];
                            min = eval[4];
                        }
                    }
                }
            }
            return best;
        }
    }

    public static int Evaluate(Board b)
    {
        int totalb = 0;
        int moves = 0;

        if (b.kingcastledblack)
        {
            totalb += 1000;
        }

        for (int i = 0; i < 16; i++)
        {
            if (b.pieces[i].id == -1)
            {
                totalb += b.pieces[i].points;
            }
        }

        for (int i = 16; i < 32; i++)
        {
            if (b.pieces[i].id == -1)
            {
                totalb -= b.pieces[i].points;
            }
            else
            {
                moves += b.pieces[i].validMoves.Count;
            }
        }

        for (int i = 16; i < 24; i++)
        {
            if (b.pieces[i].id != -1)
            {
                for (int j = i + 1; j < 24; j++)
                {
                    if (b.pieces[i].j == b.pieces[j].j)
                    {
                        totalb -= 20;
                        break;
                    }
                }
                bool isolated = true;
                if (b.pieces[i].i + 1 < 8 && b.board[b.pieces[i].i + 1, b.pieces[i].j].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                else if (b.pieces[i].i - 1 >= 0 && b.board[b.pieces[i].i - 1, b.pieces[i].j].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                else if (b.pieces[i].j + 1 < 8 && b.board[b.pieces[i].i, b.pieces[i].j + 1].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                else if (b.pieces[i].j - 1 >= 0 && b.board[b.pieces[i].i, b.pieces[i].j - 1].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                else if (b.pieces[i].i + 1 < 8 && b.pieces[i].j + 1 < 8 && b.board[b.pieces[i].i + 1, b.pieces[i].j + 1].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                else if (b.pieces[i].i + 1 < 8 && b.pieces[i].j - 1 >= 0 && b.board[b.pieces[i].i + 1, b.pieces[i].j - 1].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                else if (b.pieces[i].i - 1 >= 0 && b.pieces[i].j + 1 < 8 && b.board[b.pieces[i].i - 1, b.pieces[i].j + 1].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                else if (b.pieces[i].i - 1 >= 0 && b.pieces[i].j - 1 >= 0 && b.board[b.pieces[i].i - 1, b.pieces[i].j - 1].color == b.board[b.pieces[i].i, b.pieces[i].j].color)
                {
                    isolated = false;
                }
                if (isolated)
                    totalb -= 10;
                if (b.pieces[i].validMoves.Count == 0)
                    totalb -= 15;
            }
        }
        if (b.blackLost)
            totalb -= 10000;
        if (b.whiteLost)
            totalb += 10000;
        return totalb + moves;
    }

    public static int SimpleEvaluate(Board b)
    {
        int totalb = 0;
        int moves = 0;
        for (int i = 0; i < 16; i++)
            if (b.pieces[i].id == -1)
                totalb += b.pieces[i].points;

        if (b.kingcastledblack)
        {
            totalb += 1000;
        }

        for(int i=0; i<16; i++)
        {
            if (b.pieces[i].id == -1)
            {
                totalb += b.pieces[i].points;
            }
        }
        for (int i = 16; i < 32; i++)
        {
            if (b.pieces[i].id == -1)
            {
                totalb -= b.pieces[i].points;
            }
        }
        if (b.blackLost)
            totalb -= 10000;
        if (b.whiteLost)
            totalb += 10000;
        return totalb + moves;
    }

    public static void DeepTime(Board b)
    {
        float time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            int[] bes = new int[5];
            int[] eva = new int[5];
        }

        MonoBehaviour.print("best + eval array initialization: " + (Time.realtimeSinceStartup - time));

        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            Board board = new Board(b);
        }

        MonoBehaviour.print("Deep Copy: " + (Time.realtimeSinceStartup - time));

        Board boardRef = new Board();
        Board ref2;
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            ref2 = new Board(ref boardRef);
        }

        MonoBehaviour.print("Deep copy with ref keyword: " + (Time.realtimeSinceStartup - time));

        sbyte[] array1d = b.FinalState();

        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            Board board = new Board(array1d);
        }

        MonoBehaviour.print("Copy via 1d byte array: " + (Time.realtimeSinceStartup - time));

        Board bo = new Board(b);
        sbyte[,] array = bo.CurrentBoard();
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            Board board = new Board(array);
        }

        MonoBehaviour.print("Copy via 2d byte array: " + (Time.realtimeSinceStartup - time));

        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            Board board = new Board(b);
            board.Move(7, 0, 5, 0);
        }

        MonoBehaviour.print("Deep Copy and Move(): " + (Time.realtimeSinceStartup - time));

        time = Time.realtimeSinceStartup;
        int[] best = new int[5];
        int[] eval = new int[5];
        for (int i = 0; i < 30000; i++)
        {
            Board board = new Board(b);
            int a = Evaluate(board);
            if (a > 0)
            {
                best[0] = 7;
                best[1] = 0;
                best[2] = 5;
                best[3] = 0;
                best[4] = a;
            }
        }

        MonoBehaviour.print("Deep Copy, Evaluation and assignment: " + (Time.realtimeSinceStartup - time));

        time = Time.realtimeSinceStartup;
        int[] be = new int[5];
        for (int i = 0; i < 30000; i++)
        {
            Board board = new Board(b);
            int a = Evaluate(board);
            if (a > 0)
            {
                be[0] = 7;
                be[1] = 0;
                be[2] = 5;
                be[3] = 0;
                be[4] = a;
            }
        }

        MonoBehaviour.print("Deep Copy, Simpler Evaluation and assignment: " + (Time.realtimeSinceStartup - time));

        Board bo1 = new Board(b);
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            Piece[,] board1 = new Piece[8, 8];
            board1 = bo1.board;
        }

        MonoBehaviour.print("Copy of Pieces[,]: " + (Time.realtimeSinceStartup - time));

        int[,] intBoard = new int[8, 8];

        Board bo2 = new Board(b);
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            int[,] intBoard2 = new int[8, 8];
            intBoard = intBoard2;
        }

        MonoBehaviour.print("Copy of int[,]: " + (Time.realtimeSinceStartup - time));


        Board bb = new Board(b);
        string moves = "";
        for (int i = 0; i < 32; i++)
        {
            if (bb.pieces[i].id != -1)
                for (int j = 0; j < bb.pieces[i].validMoves.Count; j += 2)
                {
                    moves += "Move: " + bb.pieces[i].validMoves[j] + ", " + bb.pieces[i].validMoves[j + 1] + "\n";
                }
        }
        MonoBehaviour.print(moves);


        int int1 = 0, int2 = 0;
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            int1 = int2;
            int2 = 6;
        }

        MonoBehaviour.print("Copying ints: " + (Time.realtimeSinceStartup - time));


        byte sbyte1 = 0, sbyte2 = 0;
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            sbyte1 = sbyte2;
            sbyte2 = 8;
        }

        MonoBehaviour.print("Copying sbytes: " + (Time.realtimeSinceStartup - time));


    }
}
