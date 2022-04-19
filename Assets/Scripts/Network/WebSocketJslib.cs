using System;
using UnityEngine;

public class WebSocketJslib : MonoBehaviour
{
    public static Action<string> OnConnected;
    public static Action<string> OnClose;
    public static Action<TypeAndMessage> OnMessage;
    public static Action<string> OnError;

    public void Connected(string message)
    {
        Debug.Log("Connected : "+message);
        OnConnected?.Invoke(message);
    }
 
    public void Close(string message)
    {
        Debug.Log("Close : "+message);
        OnClose?.Invoke(message);
    }

    public void Message(string message)
    {
        TypeAndMessage choice = JsonUtility.FromJson<TypeAndMessage>(message);
        OnMessage?.Invoke(choice);
    }

    public void Error(string message)
    {
        Debug.Log("Error : "+message);
        OnError?.Invoke(message);
    }
}
