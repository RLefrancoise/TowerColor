using Framework.Game;

namespace Framework.Views
{
    public abstract class MenuView : View
    {
        public override GameState State => GameState.Menu;
    }
}