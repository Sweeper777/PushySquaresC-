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
}