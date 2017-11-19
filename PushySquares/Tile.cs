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
}