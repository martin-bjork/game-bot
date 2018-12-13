using System.Linq;

namespace GameBot {

    public static class Extensions {

        public static T[] GetColumn<T>(this T[,] array, int column) {
            return Enumerable.Range(0, array.GetLength(0))
                    .Select(i => array[i, column])
                    .ToArray();
        }

        public static T[] GetRow<T>(this T[,] array, int row) {
            return Enumerable.Range(0, array.GetLength(1))
                    .Select(i => array[row, i])
                    .ToArray();
        }

        // Assumes that the array is square
        public static T[] GetDiagonal<T>(this T[,] array) {
            return Enumerable.Range(0, array.GetLength(0))
                    .Select(i => array[i, i])
                    .ToArray();
        }

        // Assumes that the array is square
        public static T[] GetAntiDiagonal<T>(this T[,] array) {
            int length = array.GetLength(0);
            return Enumerable.Range(0, length)
                    .Select(i => array[i, length - i - 1])
                    .ToArray();
        }

    }

}
