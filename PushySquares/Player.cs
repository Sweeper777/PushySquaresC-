using System;

namespace PushySquares {
    /// <summary>
    /// Represents a player in a game of Pushy Squares.
    /// </summary>
    public class Player {
        /// <summary>
        /// Gets or sets the number of turns until a new square spawns.
        /// </summary>
        /// <value>The number of turns until a new square spawns.</value>
		public int TurnsUntilNewSquare { get; set; }

		private int lives;

        /// <summary>
        /// Gets or sets the number of lives this player has.
        /// </summary>
        /// <value>The number of lives this player has.</value>
		public int Lives {
			get { return lives; }
			set { lives = value == 0 ? 0 : value; }
		}

        /// <summary>
        /// Gets the color of this player.
        /// </summary>
        /// <value>The color of this player.</value>
		public Color Color { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PushySquares.Player"/> class with the
        /// number of turns until a new square spawns, number of lives, and the player's color.
        /// </summary>
        /// <param name="turnsUntilNewSquare">Number of turns until a new square spawns.</param>
        /// <param name="lives">Number of lives.</param>
        /// <param name="color">Color.</param>
		public Player(int turnsUntilNewSquare, int lives, Color color) {
			Lives = lives;
			TurnsUntilNewSquare = turnsUntilNewSquare;
			Color = color;
		}

        /// <summary>
        /// Create a deep copy of this player
        /// </summary>
        /// <returns>A deep copy of this player with everything being the same.</returns>
        public Player CreateCopy() {
            return new Player(TurnsUntilNewSquare, Lives, Color);
        }
    }
}

