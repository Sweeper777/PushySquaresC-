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

