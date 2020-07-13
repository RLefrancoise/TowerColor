namespace Framework.Game
{
    public interface IGameManager
    {
        ILevelManager LevelManager { get; }
        GameState CurrentState { get; }
        void ChangeState(GameState state, bool skipFade = false);
    }
}