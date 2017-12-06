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
        }
        }
    }
}
