using System;
using System.Collections.Generic;

namespace PushySquares {
	public delegate void GameDelegate(Direction? direction, 
	                                  Position[] originalPositions, 
	                                  Position[] destroyedSquaresPositions, 
	                                  Position[] greyedOutPositions, 
	                                  Color? newSquareColor);
}

