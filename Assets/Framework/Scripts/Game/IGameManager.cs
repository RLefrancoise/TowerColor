namespace Framework.Game
{
    public interface IGameManager
    {
        LevelManager LevelManager { get; }
        GameState CurrentState { get; }
        void ChangeState(GameState state);
    }
}