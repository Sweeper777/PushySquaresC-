using System;
using System.Collections.Generic;

namespace PushySquares {
    /// <summary>
    /// Represents a point on the game board.
    /// </summary>
	public struct Position {
        /// <summary>
        /// Gets the X coordinate of the position.
        /// </summary>
        /// <value>The X coordinate of the position.</value>
		public int X { get; }

        /// <summary>
        /// Gets the Y position
        /// </summary>
        /// <value>The Y coordinate of the position</value>
		public int Y { get; }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:PushySquares.Position"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
		public override int GetHashCode () {
			return X * 1000 + Y;
		}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:PushySquares.Position"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:PushySquares.Position"/>.</returns>
		public override string ToString ()
		{
			return $"({X}, {Y})";
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PushySquares.Position"/> struct with the specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
		public Position(int x, int y) {
			X = x;
			Y = y;
		}

        /// <summary>
        /// Gets a <see cref="T:PushySquares.Position"/> that represents a point on the board one 
        /// tile on top of this <see cref="T:PushySquares.Position"/>.
        /// </summary>
        /// <value>The <see cref="T:PushySquares.Position"/> above this.</value>
		public Position Above => new Position(X, Y - 1);

        /// <summary>
        /// Gets a <see cref="T:PushySquares.Position"/> that represents a point on the board one 
        /// tile below this <see cref="T:PushySquares.Position"/>.
        /// </summary>
        /// <value>The <see cref="T:PushySquares.Position"/> below this.</value>
		public Position Below  => new Position (X, Y + 1);

        /// <summary>
        /// Gets a <see cref="T:PushySquares.Position"/> that represents a point on the board one 
        /// tile to the right of this <see cref="T:PushySquares.Position"/>.
        /// </summary>
        /// <value>The <see cref="T:PushySquares.Position"/> to the right of this.</value>
		public Position Right => new Position (X + 1, Y);

        /// <summary>
        /// Gets a <see cref="T:PushySquares.Position"/> that represents a point on the board one 
        /// tile to the left of this <see cref="T:PushySquares.Position"/>.
        /// </summary>
        /// <value>The <see cref="T:PushySquares.Position"/> to the right of this.</value>
		public Position Left => new Position (X - 1, Y);
	}

	public static class ArrayExtensions {
        /// <summary>
        /// Returns a reference of the element in the 2D array that is at a specific 
        /// <see cref="T:PushySquares.Position"/>.
        /// </summary>
        /// <returns>The elemnt in the 2D array that is at a specific <see cref="T:PushySquares.Position"/>.</returns>
        /// <param name="array">The 2D array.</param>
        /// <param name="pos">The position.</param>
        /// <typeparam name="T">The type of the 2D array.</typeparam>
		public static ref T ItemAt<T>(this T[,] array, Position pos) {
            return ref array[pos.X, pos.Y];
		}

        /// <summary>
        /// Finds all the positions in the 2D array of <see cref="T:PushySquares.Tile"/> that matches 
        /// the specified <see cref="T:PushySquares.Tile"/>.
        /// </summary>
        /// <returns>The positions in the 2D array of <see cref="T:PushySquares.Tile"/> that matches 
        /// the specified <see cref="T:PushySquares.Tile"/>.</returns>
        /// <param name="array">The 2D array.</param>
        /// <param name="tile">The tile to look for.</param>
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

        /// <summary>
        /// Finds all the positions in the 2D array of <see cref="T:PushySquares.Tile"/> that matches 
        /// the specified <see cref="T:PushySquares.Color"/>.
        /// </summary>
        /// <returns>The positions in the 2D array of <see cref="T:PushySquares.Tile"/> that matches 
        /// the specified <see cref="T:PushySquares.Color"/>.</returns>
        /// <param name="array">The 2D array.</param>
        /// <param name="color">The color to look for.</param>
        public static List<Position> PositionsOf(this Tile[,] array, Color color) {
            return array.PositionsOf(TileExtensions.FromColor(color));
        }
	}
}