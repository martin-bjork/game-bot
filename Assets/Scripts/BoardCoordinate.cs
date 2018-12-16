using System;

namespace GameBot {

    public class BoardCoordinate {

        public int X { get; private set; }
        public int Y { get; private set; }

        public BoardCoordinate(int x, int y) {

            if (x < 0 || x >= Constants.GRID_SIZE || y < 0 || y >= Constants.GRID_SIZE) {
                throw new ArgumentException($"({x}, {y}) is not a valid tile coordinate");
            }

            X = x;
            Y = y;

        }

    }

}
