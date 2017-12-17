using System;

namespace PushySquares {
    /// <summary>
    /// Represents the color of a particular square on the board.
    /// </summary>
	public enum Color {
        /// <summary>
        /// The color of the player who goes first.
        /// </summary>
		Color1 = 1, 
        /// <summary>
        /// The color of the player who goes second.
        /// </summary>
        Color2 = 2, 
        /// <summary>
        /// The color of the player who goes third.
        /// </summary>
        Color3 = 3, 
        /// <summary>
        /// The color of the player who goes fourth.
        /// </summary>
        Color4 = 4, 
        /// <summary>
        /// The color of the players who died.
        /// </summary>
        Grey = 0
	}

    /// <summary>
    /// Represents the direction of a move that a player makes.
    /// </summary>
	public enum Direction {
		Up, Down, Left, Right
	}

    /// <summary>
    /// Represents a part of the board. The board is a matrix of tiles.
    /// </summary>
	public enum Tile {
		Empty, Void, Wall, SquareColor1, SquareColor2, SquareColor3, SquareColor4, SquareGrey
	}

	public static class TileExtensions {
        /// <summary>
        /// Gets the square <see cref="T:PushySquares.Tile"/> that is of a given color.
        /// </summary>
        /// <returns>A square <see cref="T:PushySquares.Tile"/> that has the same color as the parameter.</returns>
        /// <param name="color">The color of the returned <see cref="T:PushySquares.Tile"/>.</param>
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