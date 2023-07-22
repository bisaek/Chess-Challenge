using System;
using System.Text.RegularExpressions;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };
    public Move Think(Board board, Timer timer)
    {
        Random rnd = new Random();
        Move[] captureMoves = board.GetLegalMoves(true);
        if (captureMoves.Length > 0) return captureMoves[rnd.Next(0, captureMoves.Length)];
        
        Move[] moves = board.GetLegalMoves();


        Move bestMove = moves[rnd.Next(0, moves.Length)];
        int highestValueCapture = 0;

        foreach (Move move in moves)
        {
            Piece targetPiece = board.GetPiece(move.TargetSquare);
            int value = pieceValues[(int)targetPiece.PieceType];

            if (isMoveDangerous(board, move)) value -= (int)move.MovePieceType;

            if (value > highestValueCapture)
            {
                highestValueCapture = value;
                bestMove = move;
            }

        }
        
        return bestMove;
    }

    public bool isMoveDangerous(Board board, Move move)
    {
        board.MakeMove(move);
        foreach (Move capMove in board.GetLegalMoves(true))
        {
            if (move.TargetSquare.Equals(capMove.TargetSquare))
            {
                board.UndoMove(move);
                return true;
            }
        }
        board.UndoMove(move);
        return false;
    }
}