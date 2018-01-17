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
        /// <summary>
        /// Move upwards.
        /// </summary>
		Up, 
        /// <summary>
        /// Move downwards.
        /// </summary>
        Down, 
        /// <summary>
        /// Move to the left.
        /// </summary>
        Left, 
        /// <summary>
        /// Move to the right.
        /// </summary>
        Right
	}

    /// <summary>
    /// Represents a part of the board. The board is a matrix of tiles.
    /// </summary>
	public enum Tile {
        /// <summary>
        /// A tile on the board with no squares on it.
        /// </summary>
		Empty, 
        /// <summary>
        /// An empty space on the board where squares can fall off.
        /// </summary>
        Void,
        /// <summary>
        /// A tile that blocks the squares' movements
        /// </summary>
        Wall, 
        /// <summary>
        /// A tile with a square whose color is <see cref="E:PushySquares.Color.Color1"/> on it.
        /// </summary>
        SquareColor1, 
        /// <summary>
        /// A tile with a square whose color is <see cref="E:PushySquares.Color.Color2"/> on it.
        /// </summary>
        SquareColor2, 
        /// <summary>
        /// A tile with a square whose color is <see cref="E:PushySquares.Color.Color3"/> on it.
        /// </summary>
        SquareColor3, 
        /// <summary>
        /// A tile with a square whose color is <see cref="E:PushySquares.Color.Color4"/> on it.
        /// </summary>
        SquareColor4, 
        /// <summary>
        /// A tile with a square whose color is <see cref="E:PushySquares.Color.Grey"/> on it.
        /// </summary>
        SquareGrey
	}

    /// <summary>
    /// A class containing extension methods for <see cref="T:PushySquares.Tile"/>.
    /// </summary>
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

        /// <summary>
        /// Whether or not the tile is a square tile
        /// </summary>
        /// <returns><c>true</c>, if the tile is a square tile, <c>false</c> otherwise.</returns>
        /// <param name="tile">The tile to check.</param>
        public static bool IsSquare(this Tile tile) {
            return tile.ToString().StartsWith("Square", StringComparison.InvariantCulture);
        }
	}
    public static class DirectionExtensions {
        /// <summary>
        /// Gets the displacement function for a particular direction. e.g. If <c>direction</c> is 
        /// <see cref="Direction.Up"/>, this method returns a function that returns the position above
        /// a particular position.
        /// </summary>
        /// <returns>The displacement function for a particular position.</returns>
        /// <param name="direction">The direction.</param>
        public static Func<Position, Position> GetDisplacementFunction(this Direction direction) {
            switch (direction) {
                case Direction.Up:
                    return (pos) => pos.Above;
                case Direction.Down:
                    return (pos) => pos.Below;
                case Direction.Left:
                    return (pos) => pos.Left;
                case Direction.Right:
                    return (pos) => pos.Right;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the reverse displacement function for a particular direction. e.g. If <c>direction</c> is 
        /// <see cref="Direction.Up"/>, this method returns a function that returns the position below
        /// a particular position.
        /// </summary>
        /// <returns>The reverse displacement function for a particular position.</returns>
        /// <param name="direction">The direction.</param>
        public static Func<Position, Position> GetReverseDisplacementFunction(this Direction direction) {
            switch (direction)
            {
                case Direction.Up:
                    return (pos) => pos.Below;
                case Direction.Down:
                    return (pos) => pos.Above;
                case Direction.Left:
                    return (pos) => pos.Right;
                case Direction.Right:
                    return (pos) => pos.Left;
                default:
                    return null;
            }
        }
    }
}