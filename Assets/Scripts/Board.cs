using System;

namespace GameBot {

    public class Board {

        public TileState[,] Grid { get; private set; } = new TileState[Constants.GRID_SIZE, Constants.GRID_SIZE];

        public void CopyFrom(Board board) {
            Array.Copy(board.Grid, Grid, Grid.Length);
        }

        public void AddPiece(int x, int y, TileState piece) {

            if (x < 0 || x >= Constants.GRID_SIZE || y < 0 || y >= Constants.GRID_SIZE) {
                throw new ArgumentException($"({x}, {y}) is not a valid tile coordinate");
            }

            if (piece == TileState.EMPTY) {
                throw new ArgumentException("Cannot add empty piece");
            }

            if (Grid[x, y] != TileState.EMPTY) {
                throw new ArgumentException("Cannot add piece to non-empty tile");
            }

            Grid[x, y] = piece;

        }

    }

}
