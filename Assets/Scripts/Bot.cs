using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameBot {

    public static class Bot {

        public static Move GetBestMove(Board board, int maxDepth, PlayerType playerType) {

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

            int alpha = int.MinValue;
            int beta = int.MaxValue;
            int bestScore = int.MinValue;
            Move bestMove = null;

            List<Move> possibleMoves = board.GenerateValidMoves();
            foreach (Move move in possibleMoves) {
                board.MakeMove(move);
                int score = NegamaxAlphaBeta(board, alpha, beta, maxDepth - 1, playerColour);
                board.UndoMove(move);

                if (score > bestScore) {
                    bestScore = score;
                    bestMove = move;
                }

                alpha = Math.Max(alpha, score);
            }

            Debug.Log($"{playerType} chose move {bestMove?.ToString() ?? "null"} with score {bestScore}");

            // If there are no valid moves to make, it will be null. We should handle this
            return bestMove;
        }

        private static int NegamaxAlphaBeta(Board board, int alpha, int beta, int depth, int colour) {

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
