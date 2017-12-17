using System;
using System.Linq;
using System.Collections.Generic;
namespace PushySquares {
    /// <summary>
    /// A class containing extension methods for <see cref="Game"/>.
    /// </summary>
    public static class GameExtensions {
        /// <summary>
        /// Gets the <see cref="Player"/> object whose <see cref="Player.Color"/> property is the specifed color.
        /// </summary>
        /// <returns>The <see cref="Player"/> object whose <see cref="Player.Color"/> property is the specifed color.</returns>
        /// <param name="game">The game that the player is in.</param>
        /// <param name="color">The color of the player to be returned.</param>
        public static Player GetPlayer(this Game game, Color color) {
            return game.Players.Where(x => x.Color == color).First();
        }

        /// <summary>
        /// Gets the opponents for the player of the specified color.
        /// </summary>
        /// <returns>The opponents for the player of the specified color.</returns>
        /// <param name="game">The game that the player is in.</param>
        /// <param name="color">The specified color.</param>
        public static List<Color> OpponentsOf(this Game game, Color color) {
            return game.Players.Where(x => x.Color != color).Select(x => x.Color).ToList();
        }

        /// <summary>
        /// Attempts to move all of the current player's squares in a specific direction. This is
        /// equivalent to calling the appropriate <see cref="Game.MoveUp()"/>, 
        /// <see cref="Game.MoveDown()"/>, <see cref="Game.MoveLeft()"/> and <see cref="Game.MoveRight()"/> methods.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="direction">The direction in which the player's squares should be moved.</param>
        public static void Move(this Game game, Direction direction) {
            switch (direction) {
                case Direction.Down:
                    game.MoveDown();
                    break;
                case Direction.Up:
                    game.MoveUp();
                    break;
                case Direction.Left:
                    game.MoveLeft();
                    break;
                case Direction.Right:
                    game.MoveRight();
                    break;
            }
        }
    }

    /// <summary>
    /// Represents an AI which will decide the best move in a given game if it were a given player in
    /// that game.
    /// </summary>
    public class GameAI {
        Stack<Game> gameStates = new Stack<Game>();
        Game CurrentGame => gameStates.Peek();

        readonly int wSelfLife;
        readonly int wDiffLives;
        readonly int wSquareThreshold;
        readonly int wSelfSpreadBelowThreshold;
        readonly int wSelfSpreadAboveThreshold;
        readonly int wOpponentSpread;
        readonly int wSelfInDanger;
        readonly int wOpponentInDangerBelowThreshold;
        readonly int wOpponentInDangerAboveThreshold;

        readonly Color myColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PushySquares.GameAI"/> class, with the
        /// specified <see cref="Game"/> object, the color of the player that the AI is going to
        /// represent, and the weights of different features of the game.
        /// </summary>
        /// <param name="game">The given <see cref="Game"/> object.</param>
        /// <param name="myColor">The color of the player that the AI is going to be.</param>
        /// <param name="wSelfLife">Weight of <code>selfLife</code>.</param>
        /// <param name="wDiffLives">Weight of <code>diffLives</code>.</param>
        /// <param name="wSquareThreshold">The square threshold.</param>
        /// <param name="wSelfSpreadBelowThreshold">Weight of <code>selfSpread</code> below the square
        ///  threshold.</param>
        /// <param name="wSelfSpreadAboveThreshold">Weight of <code>selfSpread</code> below the square
        /// threshold.</param>
        /// <param name="wOpponentSpread">Weight of <code>opponentSpread</code>.</param>
        /// <param name="wSelfInDanger">Weight of <code>selfInDanger</code>.</param>
        /// <param name="wOpponentInDangerBelowThreshold">Weight of <code>opponentInDanger</code>
        /// below the square threshold.</param>
        /// <param name="wOpponentInDangerAboveThreshold">Weight of <code>opponentInDanger</code>
        /// above the square threshold.</param>
        public GameAI(Game game,
                      Color myColor,
                      int wSelfLife, 
                      int wDiffLives, 
                      int wSquareThreshold, 
                      int wSelfSpreadBelowThreshold, 
                      int wSelfSpreadAboveThreshold, 
                      int wOpponentSpread, 
                      int wSelfInDanger, 
                      int wOpponentInDangerBelowThreshold, 
                      int wOpponentInDangerAboveThreshold) {
            gameStates.Push(game);
            this.myColor = myColor;
            this.wSelfLife = wSelfLife;
            this.wDiffLives = wDiffLives;
            this.wSquareThreshold = wSquareThreshold;
            this.wSelfSpreadBelowThreshold = wSelfSpreadBelowThreshold;
            this.wSelfSpreadAboveThreshold = wSelfSpreadAboveThreshold;
            this.wOpponentSpread = wOpponentSpread;
            this.wSelfInDanger = wSelfInDanger;
            this.wOpponentInDangerBelowThreshold = wOpponentInDangerBelowThreshold;
            this.wOpponentInDangerAboveThreshold = wOpponentInDangerAboveThreshold;
        }

        /// <summary>
        /// Evaluates the heuristics of the current game state.
        /// </summary>
        /// <returns>A value representing wehther the current game state advantageous to the AI player.
        /// The larger the value, the more advantageous to the AI player.</returns>
        public int EvaluateHeuristics() {
            var livingPlayers = CurrentGame.Players.Where(x => x.Lives > 0).ToList();
            var me = CurrentGame.GetPlayer(myColor);
            if (me.Lives == 0) {
                return int.MinValue;
            }
            if (livingPlayers.Count == 1 && me.Lives > 0) {
                return int.MaxValue;
            }
            if (livingPlayers.Count == 0) {
                return 0;
            }
            var finalSelfLives = me.Lives;
            var opponents = CurrentGame.OpponentsOf(myColor);
            var finalDiffLives = 0;
            if (livingPlayers.Count == 2) {
                finalDiffLives = me.Lives - CurrentGame.GetPlayer(opponents.First()).Lives;
            }
            var mySquares = CurrentGame.Board.PositionsOf(myColor);
            var finalSelfSpread = -GetSpread(mySquares, CurrentGame.Spawnpoints[myColor]);
            var finalOpponentSpread = opponents.Select(x => GetSpread(CurrentGame.Board.PositionsOf(x), CurrentGame.Spawnpoints[x])).Sum() / opponents.Count;
            var selfInDanger = mySquares.Select(x => IsInDanger(x, IsEdge(x), myColor)).Where(x => x).Count();
            if (selfInDanger >= me.Lives) {
                return int.MinValue;
            }
            var finalSelfInDanger = -selfInDanger;
            var opponentInDanger = 0;
            foreach (var opponent in opponents) {
                opponentInDanger += CurrentGame.Board.PositionsOf(opponent).Select(x => IsInDanger(x, IsEdge(x), opponent)).Where(x => x).Count();
            }
            var finalOpponentInDanger = opponentInDanger;
            return finalSelfLives * wSelfLife +
                finalDiffLives * wDiffLives +
                finalSelfSpread * (mySquares.Count < wSquareThreshold ? wSelfSpreadBelowThreshold : wSelfSpreadAboveThreshold) +
                finalOpponentSpread * wOpponentSpread +
                finalSelfInDanger * wSelfInDanger +
                finalOpponentInDanger * (mySquares.Count < wSquareThreshold ? wOpponentInDangerBelowThreshold : wOpponentInDangerAboveThreshold);
        }

        /// <summary>
        /// Gets the best next move.
        /// </summary>
        /// <returns>A <see cref="Direction"/> representing the best move that should be taken.</returns>
        public Direction NextMove() {
            return Minimax(6, myColor).Item2;
        }

        int GetSpread(List<Position> positions, Position pivot) {
            var maxX = positions.Select(x => Math.Abs(x.X - pivot.X));
            var maxY = positions.Select(x => Math.Abs(x.Y - pivot.Y));
            if (maxX.Count() != 0 && maxY.Count() != 0) {
                return Math.Max(maxX.Max(), maxY.Max());
            }
            return 0;
        }

        List<Direction> IsEdge(Position position) {
            var edges = new List<Direction>();
            if (CurrentGame.Board.ItemAt(position.Above) == Tile.Void) {
                edges.Add(Direction.Up);
            }
            if (CurrentGame.Board.ItemAt(position.Below) == Tile.Void) {
                edges.Add(Direction.Down);
            }
            if (CurrentGame.Board.ItemAt(position.Left) == Tile.Void) {
                edges.Add(Direction.Left);
            }
            if (CurrentGame.Board.ItemAt(position.Right) == Tile.Void) {
                edges.Add(Direction.Right);
            }
            return edges;
        }

        bool IsInDanger(Position position, List<Direction> edges, Color c) {
            foreach (var edge in edges) {
                Func<Position, Position> translate = null;
                switch (edge) {
                    case Direction.Down:
                        translate = x => x.Above;
                        break;
                    case Direction.Up:
                        translate = x => x.Below;
                        break;
                    case Direction.Left:
                        translate = x => x.Right;
                        break;
                    case Direction.Right:
                        translate = x => x.Left;
                        break;
                }
                var curr = position;
                while (true) {
                    curr = translate(curr);
                    var tile = CurrentGame.Board.ItemAt(curr);
                    if (tile == Tile.Empty || tile == Tile.Void || tile == Tile.Wall) {
                        break;
                    }
                    if (tile != TileExtensions.FromColor(c)) {
                        return true;
                    }
                }
            }
            return false;
        }

        Tuple<int, Direction> Minimax(int depth, Color color) {
            var bestScore = color == myColor ? int.MinValue : int.MaxValue;
            int currentScore;
            Direction? bestDirection = null;
            if (CurrentGame.Players.Where(x => x.Lives > 0).Count() < 2 || depth == 0) {
                bestScore = EvaluateHeuristics();
            } else {
                var squareCount = CurrentGame.Board.PositionsOf(color).Count;
                var moves = squareCount == 0 ? new[] { Direction.Up } : new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
                foreach (var move in moves) {
                    var gameCopy = CurrentGame.CreateCopy();
                    gameCopy.Move(move);
                    gameStates.Push(gameCopy);
                    if (color == myColor) {
                        currentScore = Minimax(depth - 1, CurrentGame.CurrentPlayer.Color).Item1;
                        if (currentScore > bestScore) {
                            bestScore = currentScore;
                            bestDirection = move;
                        }
                    } else {
                        currentScore = Minimax(depth - 1, CurrentGame.CurrentPlayer.Color).Item1;
                        if (currentScore < bestScore)
                        {
                            bestScore = currentScore;
                            bestDirection = move;
                        }
                    }
                    gameStates.Pop();
                }
            }
            return new Tuple<int, Direction>(bestScore, bestDirection ?? Direction.Left);
        }
    }
}
