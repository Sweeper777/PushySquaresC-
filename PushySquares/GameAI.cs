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

    }
}
