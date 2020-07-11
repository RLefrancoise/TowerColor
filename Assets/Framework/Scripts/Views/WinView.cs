using Framework.Game;

namespace Framework.Views
{
    public abstract class WinView : View
    {
        public override GameState State => GameState.Win;
    }
}