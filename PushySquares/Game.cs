using System;
using System.Collections.Generic;

namespace PushySquares {
	public class Game {
	}

	public delegate void GameDelegate(Direction? direction, 
	                                  Position[] originalPositions, 
	                                  Position[] destroyedSquaresPositions, 
	                                  Position[] greyedOutPositions, 
	                                  Color? newSquareColor);
}

