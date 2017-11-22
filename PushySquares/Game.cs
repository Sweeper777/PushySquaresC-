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

		public Game(Map map, int playerCount, int lives = 5) {
			Board = map.Board;
			Spawnpoints = map.Spawnpoints;
			Players = new List<Player>();
			switch (playerCount) {
			case 4:
				Players.Add(new Player(PlayerCountToTurnsUntilNewSquare[playerCount], lives, Color.Color4));
				SpawnNewSquare(Color.Color4);
				goto case 3;
			case 3:
				Players.Add(new Player(PlayerCountToTurnsUntilNewSquare[playerCount], lives, Color.Color2));
				SpawnNewSquare(Color.Color2);
				goto case 2;
			case 2:
				Players.Add(new Player(PlayerCountToTurnsUntilNewSquare[playerCount], lives, Color.Color1));
				SpawnNewSquare(Color.Color1);
				Players.Add(new Player(PlayerCountToTurnsUntilNewSquare[playerCount], lives, Color.Color3));
				SpawnNewSquare(Color.Color3);
				break;
			}
		}

		private void SpawnNewSquare(Color c) {
			Board.ItemAt(Spawnpoints[c]) = TileExtensions.FromColor(c);
		}
	}

	public delegate void GameDelegate(Direction? direction, 
	                                  Position[] originalPositions, 
	                                  Position[] destroyedSquaresPositions, 
	                                  Position[] greyedOutPositions, 
	                                  Color? newSquareColor);
}

