using Framework.Game;

namespace Framework.Views
{
    public abstract class GameOverView : View
    {
        public override GameState State => GameState.GameOver;
    }
}