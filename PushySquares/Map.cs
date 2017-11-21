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

		public Map(string mapString) {
			string[] lines = mapString.Split(Environment.NewLine.ToCharArray()).Where(x => x != "").ToArray();
			Board = new Tile[lines[0].Length, lines.Length];
			Spawnpoints = new Dictionary<Color, Position>();
		}
	}
}
