using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushySquares
{
    /// <summary>
    /// Represents a game of PushySquares.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// A dictionary that maps the number of players to how many turns should a new square spawn.
        /// </summary>
        public static readonly Dictionary<int, int> PlayerCountToTurnsUntilNewSquare = new Dictionary<int, int>() {
            { 2, 2 },
            { 3, 4 },
            { 4, 4 }
        };

        /// <summary>
        /// Gets or sets the game board.
        /// </summary>
        /// <value>The game board.</value>
        public Tile[,] Board { get; set; }

        /// <summary>
        /// Gets or sets the spawnpoints for each color.
        /// </summary>
        /// <value>The spawnpoints.</value>
        public Dictionary<Color, Position> Spawnpoints { get; set; }

        /// <summary>
        /// Gets or sets the slippery positions on the map.
        /// </summary>
        /// <value>The slippery positions.</value>
        public List<Position> SlipperyPositions { get; set; }

        /// <summary>
        /// Gets or sets the players in this game.
        /// </summary>
        /// <value>The players.</value>
        public List<Player> Players { get; set; }
        int currentPlayerIndex = 0;

        /// <summary>
        /// Gets the current player. The current player is the player that will make a move in the
        /// current turn.
        /// </summary>
        /// <value>The current player.</value>
        public Player CurrentPlayer => Players[currentPlayerIndex];

        /// <summary>
        /// Gets or sets the delegate. The delegate will be invoked when a player makes a move
        /// </summary>
        /// <value>The delegate.</value>
        public GameDelegate Delegate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PushySquares.Game"/> class with the
        /// specified map, number of players, and lives for each player.
        /// </summary>
        /// <param name="map">The <see cref="T:PushySquares.Map"/> to be used in this game.</param>
        /// <param name="playerCount">Number of players.</param>
        /// <param name="lives">Number of lives for each player.</param>
        public Game(Map map, int playerCount, int lives = 5)
        {
            Board = map.Board;
            Spawnpoints = map.Spawnpoints;
            SlipperyPositions = map.SlipperyPositions;
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

        /// <summary>
        /// Attempts to move all of the current player's squares downwards.
        /// </summary>
        public void MoveDown()
        {
            Move((x, y) => y.Y.CompareTo(x.Y), Direction.Down);
        }

        /// <summary>
        /// Attempts to move all of the current player's squares upwards.
        /// </summary>
        public void MoveUp()
        {
            Move((x, y) => x.Y.CompareTo(y.Y), Direction.Up);
        }

        /// <summary>
        /// Attempts to move all of the current player's squares to the right.
        /// </summary>
        public void MoveRight()
        {
            Move((x, y) => y.X.CompareTo(x.X), Direction.Right);
        }

        /// <summary>
        /// Attempts to move all of the current player's squares to the left.
        /// </summary>
        public void MoveLeft()
        {
            Move((x, y) => x.X.CompareTo(y.X), Direction.Left);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:PushySquares.Game"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that shows the current player's 
        /// <see cref="Player.TurnsUntilNewSquare"/>, <see cref="Player.Lives"/> of all the players,
        /// and the current game board.</returns>
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
                    if (SlipperyPositions.Contains(new Position(x, y)))
                    {
                        builder.Append("\U0001F4A6");
                    }
                    else
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
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        /// <summary>
        /// Creates a deep copy of the current game, except for the <see cref="Delegate"/> property.
        /// </summary>
        /// <returns>A deep copy of this game.</returns>
        public Game CreateCopy()
        {
            Game copy = new Game();
            copy.Board = (Tile[,])Board.Clone();
            copy.Spawnpoints = new Dictionary<Color, Position>(Spawnpoints);
            copy.currentPlayerIndex = currentPlayerIndex;
            copy.Players = Players.Select(x => x.CreateCopy()).ToList();
            return copy;
        }

        void SpawnNewSquare(Color c)
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

        private void Move(Comparison<Position> sorter, Direction direction)
        {
            var displacement = direction.GetDisplacementFunction();
            var reverseDisplacement = direction.GetReverseDisplacementFunction();
            var allSquarePositions = Board.PositionsOf(CurrentPlayer.Color);
            if (allSquarePositions.Count == 0)
            {
                Color? newSquareColor = NextTurn();
                Delegate?.Invoke(direction, new List<Position>(), new List<Position>(), new List<Position>(), new List<Position>(), newSquareColor);
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
            var slippedPositions = new List<Position>();
            foreach (var slipperyPosition in SlipperyPositions) {
                if (Board.ItemAt(slipperyPosition).IsSquare()) {
                    var displaced = displacement(slipperyPosition);
                    if (Board.ItemAt(displaced) == Tile.Empty || Board.ItemAt(displaced) == Tile.Void) {
                        slippedPositions.Add(reverseDisplacement(slipperyPosition));
                        movingSquaresPositions.Remove(reverseDisplacement(slipperyPosition));
                        if (Board.ItemAt(displaced) == Tile.Void) {
                            beingDestroyedSquaresPositions.Add(reverseDisplacement(slipperyPosition));
                            Board.ItemAt(slipperyPosition) = Tile.Empty;
                        } else {
                            var slippedTile = Board.ItemAt(slipperyPosition);
                            Board.ItemAt(slipperyPosition) = Tile.Empty;
                            Board.ItemAt(displaced) = slippedTile;
                        }
                    }
                }
            }
            var newSquareColor1 = NextTurn();
            Delegate?.Invoke(direction, movingSquaresPositions, slippedPositions, beingDestroyedSquaresPositions, greyedOutSquaresPositions, newSquareColor1);
        }
    }

    /// <summary>
    /// Represents a method that will be called when a player makes a move.
    /// </summary>
    public delegate void GameDelegate(Direction? direction,
                                      List<Position> originalPositions,
                                      List<Position> slippedPositions,
                                      List<Position> destroyedSquaresPositions,
                                      List<Position> greyedOutPositions,
                                      Color? newSquareColor);
}

