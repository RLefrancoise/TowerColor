using UnityEngine;
using Framework.Game;

namespace Framework.Views
{
    public class GameOverView : View
    {
        public override GameState State => GameState.GameOver;
    }
}