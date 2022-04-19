using TMPro;
using UnityEngine;

public class UIGame: Singleton<UIGame>{

    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private GameObject gameConnect;
    [SerializeField] private GameObject waitingConnection;
    [SerializeField] private GameObject waitOpponent;
    [SerializeField] private GameObject gameResult;

    private void Awake() {
        instance = this;
    }

    internal static void GameConnect(bool value){
        instance.gameConnect.SetActive(value);
    }

    internal static void WaitingConnection(bool value){
        instance.waitingConnection.SetActive(value);
    }

    internal static void WaitOpponent(bool value){
        instance.waitOpponent.SetActive(value);
    }

    internal static void SetTextResult(string text){
        instance.textResult.text = "YOU " + text;
    }

    internal static void GameResult(bool value){
        instance.gameResult.SetActive(value);
    }
}
