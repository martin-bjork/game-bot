using System;
using System.Collections.Generic;

namespace GameBot {

    public class Bot {

        public Move GetBestMove(Board board, int maxDepth, PlayerType playerType) {

            if (maxDepth < 1) {
                throw new ArgumentException("The search depth must be at least 1");
            }

            int playerColour;
            switch (playerType) {
                case PlayerType.CHAOS:
                    playerColour = -1;
                    break;
                case PlayerType.ORDER:
                    playerColour = 1;
                    break;
                default:
                    throw new ArgumentException($"Encountered unexpected player type: {playerType}");
            }

            int bestScore = int.MinValue;
            Move bestMove = null;

            List<Move> possibleMoves = board.GenerateValidMoves();
            foreach (Move move in possibleMoves) {
                board.MakeMove(move);
                int score = NegamaxAlphaBeta(board, int.MinValue, int.MaxValue, maxDepth - 1, playerColour);
                board.UndoMove(move);

                if (score > bestScore) {
                    bestScore = score;
                    bestMove = move;
                }
            }

            // TODO: Figure out if it's possible for bestMove to be null here
            // If there are no valid moves to make, it will be null

            return bestMove;
        }

        private int NegamaxAlphaBeta(Board board, int alpha, int beta, int depth, int colour) {

            GameState gameState = BoardScorer.GetGameState(board);

            if (depth == 0 || gameState != GameState.NOONE) {
                return colour * BoardScorer.GetScore(gameState);
            }

            List<Move> validMoves = board.GenerateValidMoves();
            // NOTE: Possible to add move ordering here

            int score = int.MinValue;
            foreach (Move move in validMoves) {

                board.MakeMove(move);
                score = Math.Max(score, -NegamaxAlphaBeta(board, -beta, -alpha, depth - 1, -colour));
                board.UndoMove(move);

                alpha = Math.Max(alpha, score);
                if (alpha >= beta) {
                    break;
                }
            }

            return score;
        }

    }

}
