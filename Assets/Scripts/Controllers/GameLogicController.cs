using System;
using UnityEngine;

public class GameLogicController :  Singleton<GameLogicController>
{

    private bool optionX;

    public enum enumState{
        PLAYER_CHOICE,
        WAIT_OPPONENT,
        WAIT_RESULT
    }
    internal enumState statePlayer;

   
    private void Awake() 
    {
        instance = this;
    }
    internal static void Init()
    {
        ClientController.Init();
    }

    internal static void Connected(){
        Debug.Log("OnConnected");
        UIGame.WaitingConnection(false);
    }

    internal static void Restart()
    {
        BoardController.RestartGame();
        UIGame.GameResult(false);
    }

    internal static void RestartGame(){
        ClientController.RestartGame();
    }
    
    internal static void SetState(string state){
        if(Enum.TryParse<enumState>(state, out instance.statePlayer))
        {
            instance.LogicState();
        }
    }

    private void LogicState()
    {
        switch (instance.statePlayer)
        {
            case enumState.PLAYER_CHOICE:
                BoardController.PlayerTurn();
            break;
            case enumState.WAIT_OPPONENT:
                BoardController.OpponentTurn();
            break;
        }
    }

    public static void SendChoice(int index)
    {
        ClientController.SendChoice(index);
    }

    internal static void SetGrid(string[] grid)
    {
        for (int i = 0; i < grid.Length; i++)
        {
            if (grid[i] == "")
                continue;

            BoardController.ActiveMonster( i , grid[i]);
        }
    }

    internal static void SetTextResult(string text){
        UIGame.SetTextResult(text);
        UIGame.GameResult(true);
        UIGame.WaitOpponent(false);
    }
}
