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

        public int GetScore() {

            GameState gameState = GetGameState();

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

        private GameState GetGameState() {

            // TODO: Generate the indices to be searched instead of extracting the rows

            bool canBeChaos = true;

            // Columns
            for (int i = 0; i < Constants.GRID_SIZE; i++) {

                TileState[] column = Grid.GetColumn(i);

                if (IsOrder(column)) {
                    return GameState.ORDER;
                }

                if (canBeChaos && !IsChaos(column)) {
                    canBeChaos = false;
                    break;
                }
            }

            // Rows
            for (int i = 0; i < Constants.GRID_SIZE; i++) {

                TileState[] row = Grid.GetRow(i);

                if (IsOrder(row)) {
                    return GameState.ORDER;
                }

                if (canBeChaos && !IsChaos(row)) {
                    canBeChaos = false;
                    break;
                }
            }

            // Diagonals
            TileState[] diagonal = Grid.GetDiagonal();
            if (IsOrder(diagonal)) {
                return GameState.ORDER;
            }

            if (canBeChaos && !IsChaos(diagonal)) {
                canBeChaos = false;
            }

            TileState[] antiDiagonal = Grid.GetAntiDiagonal();
            if (IsOrder(antiDiagonal)) {
                return GameState.ORDER;
            }

            if (canBeChaos && !IsChaos(antiDiagonal)) {
                canBeChaos = false;
            }

            // TODO: Add off-diagonals

            if (canBeChaos) {
                return GameState.CHAOS;
            } else {
                return GameState.NOONE;
            }

        }

        private bool IsOrder(TileState[] tiles) {
            TileState orderCandidate = tiles[0];
            int successiveOrderTiles = orderCandidate != TileState.EMPTY ? 1 : 0;

            for (int i = 1; i < tiles.Length; i++) {

                // If any tile except the first one is an empty tile
                // without us already having deduced that this is
                // order, it is impossible for this to be order
                if (tiles[i] == TileState.EMPTY) {
                    return false;
                }

                if (tiles[i] != orderCandidate) {
                    // If we change tile state after the first tile, 
                    // it's impossible for this to be order
                    if (i > 1) {
                        return false;
                    } else {
                        orderCandidate = tiles[i];
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

        private bool IsChaos(TileState[] tiles) {
            bool foundBlack = false;
            bool foundWhite = false;
            // If there are both black and white pieces among the
            // tiles that are not directly at the edge, this is chaos
            for (int i = 1; i < tiles.Length - 1; i++) {
                switch (tiles[i]) {
                    case TileState.BLACK:
                        foundBlack = true;
                        break;
                    case TileState.WHITE:
                        foundWhite = true;
                        break;
                    case TileState.EMPTY:
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected tile state: {tiles[i]}");
                }
            }
            return foundBlack && foundWhite;
        }

    }

}
