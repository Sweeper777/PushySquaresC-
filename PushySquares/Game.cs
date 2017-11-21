using System;
using System.Collections.Generic;

namespace PushySquares {
	public class Game {
		public static readonly Dictionary<int, int> PlayerCountToTurnsUntilNewSquare = new Dictionary<int, int>() {
			{ 2, 2 },
			{ 3, 4 },
			{ 4, 4 }
		};

	}

	public delegate void GameDelegate(Direction? direction, 
	                                  Position[] originalPositions, 
	                                  Position[] destroyedSquaresPositions, 
	                                  Position[] greyedOutPositions, 
	                                  Color? newSquareColor);
}

