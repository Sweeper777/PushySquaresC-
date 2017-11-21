using System;
using System.Collections.Generic;
using System.Linq;

namespace PushySquares {
	public struct Map {
		public Tile[,] Board { get; }
		public Dictionary<Color, Position> Spawnpoints { get; set; }
		public Map(Tile[,] board, Dictionary<Color, Position> spawnpoints) {
			Board = board;
			Spawnpoints = spawnpoints;
		}

	}
}
