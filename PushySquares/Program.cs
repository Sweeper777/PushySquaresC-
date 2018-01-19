using System;
using System.Text;
using System.Linq;

namespace PushySquares {
    class MainClass {
        public static void Main(string[] args) {
            //Game game = new Game(Map.Standard, 4);
            //Console.WriteLine(game);
            //game.MoveDown();
            //Console.WriteLine(game);
            PlayGame();
        }

        public static void PlayGame()
        {
            Game game = new Game(Map.Standard, 4);
            Console.WriteLine(game);
            while (true)
            {
                var direction = Console.ReadKey();
                switch (direction.Key)
                {
                    case ConsoleKey.UpArrow:
                        game.MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        game.MoveDown();
                        break;
                    case ConsoleKey.LeftArrow:
                        game.MoveLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        game.MoveRight();
                        break;
                }
                Console.WriteLine(game);
            }
        }

        public static void PlayWithAI()
        {
            Game game = new Game(Map.Standard, 2);
            Console.WriteLine(game);
            while (game.Players.Where(x => x.Lives > 0).Count() > 1)
            {
                if (game.CurrentPlayer.Color == Color.Color1)
                {
                    var direction = Console.ReadKey();
                    switch (direction.Key)
                    {
                        case ConsoleKey.UpArrow:
                            game.MoveUp();
                            break;
                        case ConsoleKey.DownArrow:
                            game.MoveDown();
                            break;
                        case ConsoleKey.LeftArrow:
                            game.MoveLeft();
                            break;
                        case ConsoleKey.RightArrow:
                            game.MoveRight();
                            break;
                    }
                }
                else
                {
                    var ai = new GameAI(game.CreateCopy(), game.CurrentPlayer.Color, 553, 8371, 3, 5646, 3791, 8583, 6187, 680, 9157);
                    game.Move(ai.NextMove());
                    //var right = game.CreateCopy();
                    //right.MoveRight();
                    //var ai = new GameAI(right.CreateCopy(), game.CurrentPlayer.Color, 553, 8371, 3, 5646, 3791, 8583, 6187, 680, 9157);
                    //Console.WriteLine(ai.EvaluateHeuristics());
                    //var left = game.CreateCopy();
                    //left.MoveLeft();
                    //ai = new GameAI(left.CreateCopy(), game.CurrentPlayer.Color, 553, 8371, 3, 5646, 3791, 8583, 6187, 680, 9157);
                    //Console.WriteLine(ai.EvaluateHeuristics());
                    //break;
                }
                Console.WriteLine(game);
            }
        }

        public static void AIMatch()
        {
            Game game = new Game(Map.Standard, 4);
            Console.WriteLine(game);
            while (game.Players.Where(x => x.Lives > 0).Count() > 1)
            {
                var ai = new GameAI(game.CreateCopy(), game.CurrentPlayer.Color, 553, 8371, 3, 5646, 3791, 8583, 6187, 680, 9157);
                game.Move(ai.NextMove());
                Console.WriteLine(game);
            }
        }
    }
}
