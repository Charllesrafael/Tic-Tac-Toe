using System.Runtime.InteropServices;

public class FunctionsJslib{
    
    [DllImport("__Internal")]
    public static extern void InitWebSocket(string url, string nameGameObject);
    
    [DllImport("__Internal")]
    public static extern void Send(string type, string msg);
}
