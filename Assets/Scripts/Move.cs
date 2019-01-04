using System;

namespace GameBot {

    public class Move {

        public BoardCoordinate Coordinate { get; private set; }
        public TileState Piece { get; private set; }

        public Move(BoardCoordinate coordinate, TileState piece) {

            if (piece == TileState.EMPTY) {
                throw new ArgumentException("A move cannot have an empty piece");
            }

            Coordinate = coordinate;
            Piece = piece;
        }

        public override string ToString() {
            return $"Move {{ Piece: {Piece}, Coordinate: ({Coordinate.X}, {Coordinate.Y}) }}";
        }

    }

}
