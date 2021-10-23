using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class AlphaBeta
{
    static readonly int Min = -200000, Max = 200000;
    public static readonly int Alpha = -200000, Beta = 200000;

    public static int[] GetBestMove(Board board, int Mdepth, bool MaxPlayer, int alpha, int beta)
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
                        eval = GetBestMove(bo, Mdepth - 1, false, alpha, beta);
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
                        if (max > alpha)
                        {
                            alpha = max;
                        }
                        if (alpha > beta)
                        {
                            break;
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
                        eval = GetBestMove(bo, Mdepth - 1, true, alpha, beta);
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
                        if (min < beta)
                        {
                            beta = min;
                        }
                        if (alpha > beta)
                        {
                            break;
                        }
                    }
                }
            }
            return best;
        }
    }
}