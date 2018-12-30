using System;
using System.Collections.Generic;

namespace GameBot {

    public static class BoardScorer {

        private static List<BoardCoordinate[]> rowLookup = new List<BoardCoordinate[]>();

        static BoardScorer() {
            GenerateRowLookup();
        }

        private static void GenerateRowLookup() {
            rowLookup.Clear();

            // Rows
            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                BoardCoordinate[] row = new BoardCoordinate[Constants.GRID_SIZE];
                for (int j = 0; j < Constants.GRID_SIZE; j++) {
                    row[j] = new BoardCoordinate(i, j);
                }
                rowLookup.Add(row);
            }

            // Columns
            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                BoardCoordinate[] column = new BoardCoordinate[Constants.GRID_SIZE];
                for (int j = 0; j < Constants.GRID_SIZE; j++) {
                    column[j] = new BoardCoordinate(j, i);
                }
                rowLookup.Add(column);
            }

            // Diagonal
            BoardCoordinate[] diagonal = new BoardCoordinate[Constants.GRID_SIZE];
            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                diagonal[i] = new BoardCoordinate(i, i);
            }
            rowLookup.Add(diagonal);

            // Anti-diagonal
            BoardCoordinate[] antiDiagonal = new BoardCoordinate[Constants.GRID_SIZE];
            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                antiDiagonal[i] = new BoardCoordinate(i, Constants.GRID_SIZE - 1 - i);
            }
            rowLookup.Add(antiDiagonal);

            // Off-diagonals
            BoardCoordinate[] offDiagonal_1 = new BoardCoordinate[Constants.GRID_SIZE - 1];
            for (int i = 0; i < Constants.GRID_SIZE - 1; i++) {
                offDiagonal_1[i] = new BoardCoordinate(i, i + 1);
            }
            rowLookup.Add(offDiagonal_1);

            BoardCoordinate[] offDiagonal_2 = new BoardCoordinate[Constants.GRID_SIZE - 1];
            for (int i = 0; i < Constants.GRID_SIZE - 1; i++) {
                offDiagonal_2[i] = new BoardCoordinate(i + 1, i);
            }
            rowLookup.Add(offDiagonal_2);

            // Off-anti-diagonals
            BoardCoordinate[] offAntiDiagonal_1 = new BoardCoordinate[Constants.GRID_SIZE - 1];
            for (int i = 0; i < Constants.GRID_SIZE - 1; i++) {
                offAntiDiagonal_1[i] = new BoardCoordinate(i, Constants.GRID_SIZE - 2 - i);
            }
            rowLookup.Add(offAntiDiagonal_1);

            BoardCoordinate[] offAntiDiagonal_2 = new BoardCoordinate[Constants.GRID_SIZE - 1];
            for (int i = 0; i < Constants.GRID_SIZE - 1; i++) {
                offAntiDiagonal_2[i] = new BoardCoordinate(i + 1, Constants.GRID_SIZE - 1 - i);
            }
            rowLookup.Add(offAntiDiagonal_2);

        }

        public static int GetScore(Board board) {

            GameState gameState = GetGameState(board);

            switch (gameState) {
                case GameState.ORDER:
                    return Constants.ORDER_SCORE;
                case GameState.CHAOS:
                    return Constants.CHAOS_SCORE;
                case GameState.NOONE:
                    return Constants.NOONE_SCORE;
                default:
                    throw new InvalidOperationException($"Unexpected game state: {gameState}");
            }

        }

        public static GameState GetGameState(Board board) {

            bool canBeChaos = true;

            for (int i = 0; i < rowLookup.Count; i++) {

                if (IsOrder(board, rowLookup[i])) {
                    return GameState.ORDER;
                }

                if (canBeChaos && !IsChaos(board, rowLookup[i])) {
                    canBeChaos = false;
                }

            }

            if (canBeChaos) {
                return GameState.CHAOS;
            } else {
                return GameState.NOONE;
            }

        }

        private static bool IsOrder(Board board, BoardCoordinate[] row) {

            TileState orderCandidate = board.GetTileState(row[0]);
            int successiveOrderTiles = orderCandidate != TileState.EMPTY ? 1 : 0;

            for (int i = 1; i < row.Length; i++) {

                TileState tile = board.GetTileState(row[i]);

                // If any tile except the first one is an empty tile
                // without us already having deduced that this is
                // order, it is impossible for this to be order
                if (tile == TileState.EMPTY) {
                    return false;
                }

                if (tile != orderCandidate) {
                    // If we change tile state after the first tile, 
                    // it's impossible for this to be order
                    if (i > 1) {
                        return false;
                    } else {
                        orderCandidate = tile;
                    }
                } else {
                    successiveOrderTiles++;
                    if (successiveOrderTiles == Constants.GRID_SIZE - 1) {
                        return true;
                    }
                }
            }
            // We will only reach this state if we test one of the off-diagonals
            // and that one isn't order
            return false;
        }

        private static bool IsChaos(Board board, BoardCoordinate[] row) {

            // If we are testing a row that is not an off-diagonal or off-anti-diagonal,
            // we should only look at the tiles that are not at the edge
            int startIndex = row.Length == Constants.GRID_SIZE ? 1 : 0;

            bool foundBlack = false;
            bool foundWhite = false;

            // If there are both black and white pieces among the
            // tiles this is chaos
            for (int i = startIndex; i < Constants.GRID_SIZE - 1; i++) {
                switch (board.GetTileState(row[i])) {
                    case TileState.BLACK:
                        foundBlack = true;
                        break;
                    case TileState.WHITE:
                        foundWhite = true;
                        break;
                    case TileState.EMPTY:
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected tile state: {board.GetTileState(row[i])}");
                }
            }
            return foundBlack && foundWhite;
        }

    }

}
