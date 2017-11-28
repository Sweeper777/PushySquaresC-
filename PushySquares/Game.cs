using System;
using System.Collections.Generic;

namespace PushySquares
{
    public class Game
    {
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

        public Game(Map map, int playerCount, int lives = 5)
        {
            Board = map.Board;
            Spawnpoints = map.Spawnpoints;
            Players = new List<Player>();
            switch (playerCount)
            {
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
            Players.Sort((x, y) => x.Color.CompareTo(y.Color));
            if (playerCount < 4)
            {
                Spawnpoints.Remove(Color.Color4);
            }
            if (playerCount < 3)
            {
                Spawnpoints.Remove(Color.Color2);
            }

            CurrentPlayer.TurnsUntilNewSquare--;
        }

        private void SpawnNewSquare(Color c)
        {
            Board.ItemAt(Spawnpoints[c]) = TileExtensions.FromColor(c);
        }

        private Color? NextTurn()
        {
            Color? retVal = null;
            if (!Players.Any(x => x.Lives > 0))
            {
                return Color.Grey;
            }
            do
            {
                currentPlayerIndex = currentPlayerIndex == Players.Count - 1 ? 0 : currentPlayerIndex + 1;
            } while (CurrentPlayer.Lives == 0);
            CurrentPlayer.TurnsUntilNewSquare--;
            if (CurrentPlayer.TurnsUntilNewSquare == 0)
            {
                if (Board.ItemAt(Spawnpoints[CurrentPlayer.Color]) == Tile.Empty)
                {
                    SpawnNewSquare(CurrentPlayer.Color);
                    retVal = CurrentPlayer.Color;
                }
                CurrentPlayer.TurnsUntilNewSquare = Game.PlayerCountToTurnsUntilNewSquare[Players.Count] + 1;
            }
            return retVal;
        }
    }

    public delegate void GameDelegate(Direction? direction,
                                      Position[] originalPositions,
                                      Position[] destroyedSquaresPositions,
                                      Position[] greyedOutPositions,
                                      Color? newSquareColor);
}
