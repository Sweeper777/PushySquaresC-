using System;
using System.Collections.Generic;
using System.Linq;

namespace PushySquares {
    /// <summary>
    /// Represnets an arrangement of holes, walls, initial squares and spawnpoints used in a game of 
    /// PushySquares.
    /// </summary>
	public struct Map {
        /// <summary>
        /// Gets the game board.
        /// </summary>
        /// <value>The game board.</value>
		public Tile[,] Board { get; }

        /// <summary>
        ///  Gets or sets the spawnpoints.
        /// </summary>
        /// <value>The spawnpoints.</value>
		public Dictionary<Color, Position> Spawnpoints { get; set; }

        /// <summary>
        /// Gets or sets the slippery positions on the map.
        /// </summary>
        /// <value>The slippery positions.</value>
        public List<Position> SlipperyPositions { get; set; }

        /// <summary>
        /// The standard map used in a standard game of PushySquares.
        /// </summary>
		public static readonly Map Standard = new Map(
@"....OO....
.1++++++2.
.++++++++.
.++++++++.
O+++OO+++O
O+++OO+++O
.++++++++.
.++++++++.
.4++++++3.
....OO....");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PushySquares.Map"/> struct with the specifed
        /// board configuration and spawnpoints.
        /// </summary>
        /// <param name="board">The initial board configuration.</param>
        /// <param name="spawnpoints">The spawnpoints.</param>
		public Map(Tile[,] board, Dictionary<Color, Position> spawnpoints) {
			Board = board;
			Spawnpoints = spawnpoints;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PushySquares.Map"/> struct with a string
        /// representing the initial map configuration and spawnpoints
        /// </summary>
        /// <param name="mapString">The string representing the initial map configuration and
        /// spawnpoints</param>
        /// <remarks>
        /// The map string must be a matrix of characters, with new rows denoted by new line 
        /// characters. This table shows what character corresponds to what <see cref="T:PushySquares.Tile"/>.
        /// <list type="table">
        ///     <item>
        ///         <item><code>.</code></item>
        ///         <item><see cref="E:PushySquares.Tile.Void"/></item>
        ///     </item>
        ///     <item>
        ///         <item><code>+</code></item>
        ///         <item><see cref="E:PushySquares.Tile.Empty"/></item>
        ///     </item>
        ///      <item>
        ///         <item><code>O</code></item>
        ///         <item><see cref="E:PushySquares.Tile.Wall"/></item>
        ///     </item>
        ///      <item>
        ///         <item><code>1</code></item>
        ///         <item>Spawnpoint for <see cref="E:PushySquares.Color.Color1"/></item>
        ///     </item>
        ///      <item>
        ///         <item><code>2</code></item>
        ///         <item>Spawnpoint for <see cref="E:PushySquares.Color.Color2"/></item>
        ///     </item>
        ///      <item>
        ///         <item><code>3</code></item>
        ///         <item>Spawnpoint for <see cref="E:PushySquares.Color.Color3"/></item>
        ///     </item>
        ///      <item>
        ///         <item><code>4</code></item>
        ///         <item>Spawnpoint for <see cref="E:PushySquares.Color.Color4"/></item>
        ///     </item>
        /// </list>
        /// </remarks>
		public Map(string mapString) {
			string[] lines = mapString.Split(Environment.NewLine.ToCharArray()).Where(x => x != "").ToArray();
			Board = new Tile[lines[0].Length, lines.Length];
			Spawnpoints = new Dictionary<Color, Position>();
			for (int i = 0 ; i < lines.Length ; i++) {
				for (int j = 0 ; j < lines[i].Length ; j++) {
					char c = lines[i][j];
					switch (c) {
					case '.':
						Board[i, j] = Tile.Void;
						break;
					case '+':
						Board[i, j] = Tile.Empty;
						break;
					case 'O':
						Board[i, j] = Tile.Wall;
						break;
					case '1':
						Board[i, j] = Tile.Empty;
						Spawnpoints[Color.Color1] = new Position(i, j);
						break;
					case '2':
						Board[i, j] = Tile.Empty;
						Spawnpoints[Color.Color2] = new Position(i, j);
						break;
					case '3':
						Board[i, j] = Tile.Empty;
						Spawnpoints[Color.Color3] = new Position(i, j);
						break;
					case '4':
						Board[i, j] = Tile.Empty;
						Spawnpoints[Color.Color4] = new Position(i, j);
						break;
					}
				}
			}
		}
	}
}

