using System;

namespace PushySquares {
	public enum Color {
		Color1 = 1, Color2 = 2, Color3 = 3, Color4 = 4, Grey = 0
	}

	public enum Direction {
		Up, Down, Left, Right
	}

	public enum Tile {
		Empty, Void, Wall, SquareColor1, SquareColor2, SquareColor3, SquareColor4, SquareGrey
	}

	public static class TileExtensions {
		public static Tile FromColor(Color color) {
			switch (color) {
			case Color.Color1:
				return Tile.SquareColor1;
			case Color.Color2:
				return Tile.SquareColor2;
			case Color.Color3:
				return Tile.SquareColor3;
			case Color.Color4:
				return Tile.SquareColor4;
			case Color.Grey:
				return Tile.SquareGrey;
			default:
				throw new ArgumentException ("Invalid Color!");
			}
		}
	}
}