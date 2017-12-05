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

        readonly Color myColor;
        }
    }
}
