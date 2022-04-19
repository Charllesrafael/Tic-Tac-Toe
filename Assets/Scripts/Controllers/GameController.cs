public class GameController : Singleton<GameController>
{
    private void Awake() {
        instance = this;
    }

    public void Connect()
    {
        GameLogicController.Init();
        UIGame.GameConnect(false);
        UIGame.WaitingConnection(true);
    } 

    public void RestartGame(){
        GameLogicController.RestartGame();
        UIGame.GameResult(false);
        UIGame.WaitOpponent(true);
    }

    public static void StartGame()
    {
        GameLogicController.Restart();
    }
}
