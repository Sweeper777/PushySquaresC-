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
}
