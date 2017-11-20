using System;

namespace PushySquares {

	public class Player {
		public int TurnsUntilNewSquare { get; set; }
		private int lives;
		public int Lives {
			get { return lives; }
			set { lives = value == 0 ? 0 : value; }
		}
		public Color Color { get; }

	}
}

