using System;
using System.Collections.Generic;

namespace PushySquares {
	public class Game {
		public static readonly Dictionary<int, int> PlayerCountToTurnsUntilNewSquare = new Dictionary<int, int>() {
			{ 2, 2 },
			{ 3, 4 },
			{ 4, 4 }
		};

		public Tile[,] Board { get; set; }
		public Dictionary<Color, Position> Spawnpoints { get; set; }
		public List<Player> Players { get; set; }
		private int currentPlayerIndex = 0;
		public Player CurrentPlayer => Players[currentPlayerIndex];
		public GameDelegate Delegate { get; set; }

	}

	public delegate void GameDelegate(Direction? direction, 
	                                  Position[] originalPositions, 
	                                  Position[] destroyedSquaresPositions, 
	                                  Position[] greyedOutPositions, 
	                                  Color? newSquareColor);
}

