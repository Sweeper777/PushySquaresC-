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

        Game() { }

        public void MoveDown()
        {
            Move(x => x.Below, (x, y) => y.Y.CompareTo(x.Y), Direction.Down);
        }

        public void MoveUp()
        {
            Move(x => x.Above, (x, y) => x.Y.CompareTo(y.Y), Direction.Up);
        }

        public void MoveRight()
        {
            Move(x => x.Right, (x, y) => y.X.CompareTo(x.X), Direction.Right);
        }

        public void MoveLeft()
        {
            Move(x => x.Left, (x, y) => x.X.CompareTo(y.X), Direction.Left);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var dict = new Dictionary<Tile, string>() {
                {Tile.SquareColor1, "\U0001F6B9"},
                {Tile.SquareColor2, "\U0001F6BA️"},
                {Tile.SquareColor3, "\U0001F6BC"},
                {Tile.SquareColor4, "\u2747\uFE0F️ "}
            };
            builder.AppendLine($"Current Turn: {dict[TileExtensions.FromColor(CurrentPlayer.Color)]}");
            builder.Append("Lives: ");
            Players.ForEach(x => builder.Append($"{x.Lives} "));
            builder.AppendLine();
            builder.AppendLine($"New Square In: {CurrentPlayer.TurnsUntilNewSquare}");
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                for (int x = 0; x < Board.GetLength(0); x++)
                {
                    switch (Board[x, y])
                    {
                        case Tile.Empty:
                            builder.Append("\u2B1C\uFE0F");
                            break;
                        case Tile.Void:
                            builder.Append("  ");
                            break;
                        case Tile.Wall:
                            builder.Append("\U0001F532");
                            break;
                        case Tile.SquareGrey:
                            builder.Append("\u2139\uFE0F️ ");
                            break;
                        case Tile.SquareColor1:
                            builder.Append("\U0001F6B9");
                            break;
                        case Tile.SquareColor2:
                            builder.Append("\U0001F6BA️");
                            break;
                        case Tile.SquareColor3:
                            builder.Append("\U0001F6BC️");
                            break;
                        case Tile.SquareColor4:
                            builder.Append("\u2747\uFE0F️ ");
                            break;
                    }
                }
                builder.AppendLine();
            }
            return builder.ToString();
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

        private List<Position> HandleDeaths(List<Position> destroyedSquarePositions)
        {
            var retVal = new List<Position>();
            foreach (var player in Players)
            {
                var destroyedSquares = destroyedSquarePositions.Where(x => Board.ItemAt(x) == TileExtensions.FromColor(player.Color));
                player.Lives -= destroyedSquares.Count();
                if (player.Lives == 0)
                {
                    foreach (var pos in Board.PositionsOf(player.Color))
                    {
                        retVal.Add(pos);
                        Board.ItemAt(pos) = Tile.SquareGrey;
                    }
                }
            }
            return retVal;
        }

        private void Move(Func<Position, Position> displacement, Comparison<Position> sorter, Direction direction)
        {
            var allSquarePositions = Board.PositionsOf(CurrentPlayer.Color);
            if (allSquarePositions.Count == 0)
            {
                Color? newSquareColor = NextTurn();
                Delegate?.Invoke(direction, new List<Position>(), new List<Position>(), new List<Position>(), newSquareColor);
                return;
            }

            var movingSquaresPositions = new List<Position>();
            var beingDestroyedSquaresPositions = new List<Position>();
            foreach (var position in allSquarePositions)
            {
                var pushedPositions = new List<Position>() { position };
                while (true)
                {
                    switch (Board.ItemAt(displacement(pushedPositions.Last())))
                    {
                        case Tile.Empty:
                            goto outOfLoop;
                        case Tile.Wall:
                            pushedPositions.Clear();
                            goto outOfLoop;
                        case Tile.Void:
                            beingDestroyedSquaresPositions.Add(pushedPositions.Last());
                            goto outOfLoop;
                        case Tile.SquareColor1:
                        case Tile.SquareColor2:
                        case Tile.SquareColor3:
                        case Tile.SquareColor4:
                        case Tile.SquareGrey:
                            pushedPositions.Add(displacement(pushedPositions.Last()));
                            break;
                    }
                }
            outOfLoop:
                movingSquaresPositions.AddRange(pushedPositions);
            }
            var sortedPositions = movingSquaresPositions.Distinct().ToList();
            sortedPositions.Sort(sorter);
            beingDestroyedSquaresPositions = beingDestroyedSquaresPositions.Distinct().ToList();
            var greyedOutSquaresPositions = HandleDeaths(beingDestroyedSquaresPositions);
            foreach (var position in sortedPositions)
            {
                var tile = Board.ItemAt(position);
                Board.ItemAt(position) = Tile.Empty;
                if (!beingDestroyedSquaresPositions.Contains(position))
                {
                    Board.ItemAt(displacement(position)) = tile;
                }
            }
            var newSquareColor1 = NextTurn();
            Delegate?.Invoke(direction, movingSquaresPositions, beingDestroyedSquaresPositions, greyedOutSquaresPositions, newSquareColor1);
        }
    }

    public delegate void GameDelegate(Direction? direction,
                                      List<Position> originalPositions,
                                      List<Position> destroyedSquaresPositions,
                                      List<Position> greyedOutPositions,
                                      Color? newSquareColor);
}
