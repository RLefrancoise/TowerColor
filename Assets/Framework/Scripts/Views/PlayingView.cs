using UnityEngine;
using Framework.Game;

namespace Framework.Views
{
    public class PlayingView : View
    {
        public override GameState State => GameState.Playing;
    }
}