using System;

namespace PushySquares {
	public struct Position {
		public int X { get; }
		public int Y { get; }

		public override int GetHashCode () {
			return X * 1000 + Y;
		}

		public override string ToString ()
		{
			return $"({X}, {Y})";
		}

		public Position(int x, int y) {
			X = x;
			Y = y;
		}

		public Position Above => new Position(X, Y + 1);
		public Position Below  => new Position (X, Y - 1);
		public Position Right => new Position (X + 1, Y);
		public Position Left => new Position (X - 1, Y);
	}

	public static class ArrayExtensions {
		public static ref T ItemAt<T>(this T[,] array, Position pos) {
            return ref array[pos.X, pos.Y];
		}

        public static List<Position> PositionsOf(this Tile[,] array, Tile tile) {
            var list = new List<Position>();
            for (int x = 0; x < array.GetLength(0); x++) {
                for (int y = 0; y < array.GetLength(1); y++) {
                    if (array[x, y] == tile) {
                        list.Add(new Position(x, y));
                    }
                }
            }
            return list;
        }

        public static List<Position> PositionsOf(this Tile[,] array, Color color) {
            return array.PositionsOf(TileExtensions.FromColor(color));
        }
	}
}