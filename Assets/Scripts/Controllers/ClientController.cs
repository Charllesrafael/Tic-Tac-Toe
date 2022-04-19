using System;
using UnityEngine;

[RequireComponent(typeof(WebSocketJslib))]
public class ClientController : Singleton<ClientController>
{
    private string url = "ws://localhost:8080";

    internal static void Init()
    {
        FunctionsJslib.InitWebSocket(instance.url, instance.gameObject.name);
    }

    private void Start() 
    {
        instance = this;
        WebSocketJslib.OnConnected += OnConnected;
        WebSocketJslib.OnMessage += OnMessage;
    }

    private void OnDestroy() 
    {
        WebSocketJslib.OnConnected -= OnConnected;
        WebSocketJslib.OnMessage -= OnMessage;
    }

    private void OnConnected(string obj)
    { 
        GameLogicController.Connected();
    }

    private void OnMessage(TypeAndMessage data)
    {
        OnGridReceived(data);
        switch (data.type)
        {
            case "STATE":
                OnStateReceived(data);
                break;
            case "RESULT":
                OnResultReceived(data);
                break;
            case "RESTART":
                OnRestartReceived(data);
                break;
        }
    }

    private void OnGridReceived(TypeAndMessage data)
    {
        GameLogicController.SetGrid(data.grid);
    }

    private void OnStateReceived(TypeAndMessage data)
    {
        GameLogicController.SetState(data.message);
    }

    private static void OnResultReceived(TypeAndMessage data)
    {
        GameLogicController.SetTextResult(data.message);
    }

    private void OnRestartReceived(TypeAndMessage data)
    {
        GameLogicController.Restart();
        OnStateReceived(data);
    }

    internal static void SendChoice(int index)
    {
        instance.Send(MessageType.TypeEnum.CHOICE.ToString(), index.ToString());
    }

    internal static void RestartGame()
    {
        instance.Send(MessageType.TypeEnum.ACTION.ToString(), "RESTART");
    }

    private void Send(string typeMessage, string message){
        
        FunctionsJslib.Send(typeMessage, message);
    }
}
