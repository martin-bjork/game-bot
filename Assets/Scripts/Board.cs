using System;
using System.Collections.Generic;
using System.Text;

namespace GameBot {

    public class Board {

        private TileState[,] grid = new TileState[Constants.GRID_SIZE, Constants.GRID_SIZE];

        public Board() { }

        public TileState GetTileState(BoardCoordinate coordinate) {
            return grid[coordinate.X, coordinate.Y];
        }

        public void MakeMove(Move move) {

            if (GetTileState(move.Coordinate) != TileState.EMPTY) {
                throw new ArgumentException("Cannot add piece to non-empty tile");
            }

            grid[move.Coordinate.X, move.Coordinate.Y] = move.Piece;
        }

        public void UndoMove(Move move) {

            if (GetTileState(move.Coordinate) != move.Piece) {
                throw new ArgumentException("Cannot undo move that hasn't been made");
            }

            grid[move.Coordinate.X, move.Coordinate.Y] = TileState.EMPTY;
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

        public string GenerateBoardVisualization(char blackToken = 'X', char whiteToken = 'O', char spaceToken = ' ') {

            // Set it to default size grid size + padding of one character on each side + one newline per row
            StringBuilder sb = new StringBuilder((Constants.GRID_SIZE + 3) * (Constants.GRID_SIZE + 2));

            // Top border
            sb.Append(' ');
            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                sb.Append('-');
            }
            sb.Append(' ');

            // Grid
            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                sb.Append('|');
                for (int j = 0; j < Constants.GRID_SIZE; j++) {
                    switch (grid[i,j]) {
                        case TileState.BLACK:
                            sb.Append(blackToken);
                            break;
                        case TileState.WHITE:
                            sb.Append(whiteToken);
                            break;
                        case TileState.EMPTY:
                            sb.Append(spaceToken);
                            break;
                        default:
                            throw new InvalidOperationException($"Unexpected tile state: {grid[i, j]}");
                    }
                }
                sb.Append('|');
                sb.Append(Environment.NewLine);
            }

            // Bottom border
            sb.Append(' ');
            for (int i = 0; i < Constants.GRID_SIZE; i++) {
                sb.Append('-');
            }
            sb.Append(' ');

            return sb.ToString();
        }

    }

}
