using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.WebSockets;
using System.Text;
using System;
using System.Threading;

public class WebSocketWindows : MonoBehaviour {
    [SerializeField] private string url;
	private ClientWebSocket webSocket;
	public static Action OnConnected;
	public static Action OnRecevedClose;
	public static Action<String> OnRecevedText;
	public static Action<ArraySegment<Byte>> OnRecevedBinary;



	public void Inicialize(){
		Task connect = Connect(url);
	}
	
    public void Send(string message){
        byte[] messageEncond = Encoding.UTF8.GetBytes(message);
        ArraySegment<byte> arrayBytes = new ArraySegment<byte>(messageEncond);
        Task taskSend = webSocket.SendAsync(arrayBytes,WebSocketMessageType.Text,true,CancellationToken.None);
	}

    private void OnDestroy() {
		Disconect();
	}

    private async Task Connect(string url)
    {
        try
		{
			webSocket = new ClientWebSocket();
			await webSocket.ConnectAsync(new Uri(url), CancellationToken.None);
			
			if (webSocket.State == WebSocketState.Open)
				OnConnected?.Invoke();
			
			Task receiveTask = Task.WhenAll( Received() );
		}
		catch (System.Exception e)
		{
			Debug.Log(e);
		}
    }

	private async Task Received(){
		ArraySegment<Byte> buffer = new ArraySegment<byte>(new byte[8192]);

		while (webSocket.State == WebSocketState.Open)
        {
            buffer = await WebSocketStateOpen(buffer);
        }
    }

    private async Task<ArraySegment<byte>> WebSocketStateOpen(ArraySegment<byte> buffer)
    {
        WebSocketReceiveResult result = null;
        using (var ms = new MemoryStream())
        {
            await FillMemoryStream(buffer, result, ms);
            CallByMenssageType(buffer, result, ms);
        }

        return buffer;
    }

    private async Task FillMemoryStream(ArraySegment<byte> buffer, WebSocketReceiveResult result, MemoryStream ms)
    {
        do
		{
			result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
			ms.Write(buffer.Array, buffer.Offset, result.Count);
		} while (!result.EndOfMessage);
    }

    private void CallByMenssageType(ArraySegment<byte> buffer, WebSocketReceiveResult result, MemoryStream ms)
    {
        switch (result.MessageType)
        {
            case WebSocketMessageType.Text:
                WebSocketTypeText(ms);
			break;
            case WebSocketMessageType.Binary:
                WebSocketTypeBinary(buffer);
			break;
            case WebSocketMessageType.Close:
                WebSocketTypeClose();
			break;
        }
    }

    private void WebSocketTypeText(MemoryStream ms)
    {
        using (var reader = new StreamReader(ms, Encoding.UTF8))
        {
            string message = reader.ReadToEnd();
            OnRecevedText?.Invoke(message);
        }
    }

    private void WebSocketTypeBinary(ArraySegment<byte> buffer)
    {
        OnRecevedBinary?.Invoke(buffer);
    }

    private void WebSocketTypeClose()
    {
        Task taskClose = webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        OnRecevedClose?.Invoke();
		Debug.Log("WebSocketTypeClose");
    }

    public void Disconect()
    {
        if (webSocket != null)
			webSocket.Dispose();

		Debug.Log("WebSocket closeed");
    }
}