using System;
using System.Linq;
using System.Collections.Generic;
namespace PushySquares {
    public static class GameExtensions {
        public static Player GetPlayer(this Game game, Color color) {
            return game.Players.Where(x => x.Color == color).First();
        }

        public static List<Color> OpponentsOf(this Game game, Color color) {
            return game.Players.Where(x => x.Color != color).Select(x => x.Color).ToList();
        }

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
            }
        }
    }
}
