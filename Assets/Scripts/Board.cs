using System;
using System.Collections.Generic;

namespace GameBot {

    public class Board {

        private TileState[,] grid = new TileState[Constants.GRID_SIZE, Constants.GRID_SIZE];

        public void CopyFrom(Board board) {
            Array.Copy(board.grid, grid, grid.Length);
        }

        public TileState GetTileState(BoardCoordinate coordinate) {
            return grid[coordinate.X, coordinate.Y];
        }

        public void MakeMove(Move move) {

            if (GetTileState(move.Coordinate) != TileState.EMPTY) {
                throw new ArgumentException("Cannot add piece to non-empty tile");
            }

            grid[move.Coordinate.X, move.Coordinate.Y] = move.Piece;
        }

        public List<Move> GenerateValidMoves() {

            List<Move> validMoves = new List<Move>();

            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                for (int j = 0; j < Constants.GRID_SIZE; j++) {
                    if (grid[i, j] == TileState.EMPTY) {
                        BoardCoordinate coordinate = new BoardCoordinate(i, j);
                        validMoves.Add(new Move(coordinate, TileState.BLACK));
                        validMoves.Add(new Move(coordinate, TileState.WHITE));
                    }
                }
            }

            return validMoves;
        }

    }

}
