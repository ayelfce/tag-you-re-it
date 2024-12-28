using Photon.Pun;
using TMPro;
using UnityEngine;

public class EndRoundScreen : MonoBehaviourPun
{
    public static EndRoundScreen Instance { get; private set; }
    public TextMeshProUGUI previousTaggedPlayerText;
    public Photon.Realtime.Player ebemiss;

    void Start()
    {
        // "NewEbe" bilgisi odadan alınır
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("NewEbe", out object ebeNick))
        {
            foreach (Photon.Realtime.Player ebe in GameManager.Instance.allPlayers)
            {
                if (ebe.NickName == (string)ebeNick)
                {
                    ebemiss = ebe;
                    Debug.Log($"Ebemiz: {ebemiss.NickName}");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("NewEbe not found in room properties.");
        }

        if (ebemiss != null)
        {
            previousTaggedPlayerText.text = "New SEEKER for the next round: " + ebemiss.NickName;
        }
        else
        {
            previousTaggedPlayerText.text = "No previous tagger.";
        }
    }

    public void OnContinueButtonClicked()
    {
        // Sadece master client oyun başlatabilir
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client is starting the game.");

            // Yeni ebe belirlenir ve oyun başlatma sinyali gönderilir
            if (ebemiss != null)
            {
                photonView.RPC("StartNewGame", RpcTarget.All, ebemiss.NickName);
            }
            else
            {
                Photon.Realtime.Player randomTagger = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)];
                photonView.RPC("StartNewGame", RpcTarget.All, randomTagger.NickName);
            }
        }
        else
        {
            Debug.LogWarning("Only the master client can start the game.");
        }
    }

    [PunRPC]
    void StartNewGame(string taggerNickName)
    {
        Debug.Log($"Starting new game. Seeker is: {taggerNickName}");

        // Yeni ebe ayarlanır
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == taggerNickName)
            {
                var properties = player.CustomProperties;
                properties["Role"] = "EBE";
                player.SetCustomProperties(properties);
            }
            else {
                var properties = player.CustomProperties;
                properties["Role"] = "Hiding";
                player.SetCustomProperties(properties);
            }
        }

        // Oyun değişkenleri sıfırlanır
        ResetGameState();

        // Yeni oyun sahnesi yüklenir
        PhotonNetwork.LoadLevel("GameScene");
    }

    void ResetGameState()
    {
        // Timer sıfırlanır
        GameManager.Instance.ResetTimer();

        // Görülen oyuncular listesi sıfırlanır
        GameManager.Instance.ResetSeenList();

        Debug.Log("Game state has been reset.");
    }
}
