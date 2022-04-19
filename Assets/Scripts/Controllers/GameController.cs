public class GameController : Singleton<GameController>
{
    private void Awake() {
        instance = this;
    }

    public void Connect()
    {
        UIGame.GameConnect(false);
        UIGame.WaitingConnection(true);
        GameLogicController.Init();
    } 

    public void RestartGame(){
        UIGame.GameResult(false);
        UIGame.WaitOpponent(true);
        GameLogicController.RestartGame();
    }

    public static void StartGame()
    {
        GameLogicController.Restart();
    }
}
