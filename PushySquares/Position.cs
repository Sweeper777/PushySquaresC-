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

	}
}