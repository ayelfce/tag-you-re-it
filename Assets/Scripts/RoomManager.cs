using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName;
    public TMP_InputField username;
    public TextMeshProUGUI warningText;
    public byte maxPlayers = 4;

    // Oyun başlatma için kullanılacak buton
    public GameObject startButton;

    private void Start()
    {
        // Eğer bağlanmamışsak, bağlanmaya çalış
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void GoBackButton() {
        SceneManager.LoadScene("MainMenu");
    }

    public void CreateButton()
    {
        // Kullanıcı adı ve oda adı kontrolü
        if (string.IsNullOrWhiteSpace(username.text))
        {
            ShowWarning("Lütfen bir kullanıcı adı girin.");
            return;
        }

        if (string.IsNullOrWhiteSpace(roomName.text))
        {
            ShowWarning("Lütfen bir oda adı girin.");
            return;
        }

        // Kullanıcı adı ayarla
        PhotonNetwork.NickName = username.text;

        // Odayı yaratmak için önce Master Server'a bağlanmış olmalısınız
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = maxPlayers;
            PhotonNetwork.CreateRoom(roomName.text, roomOpt);
        }
        else
        {
            ShowWarning("Bağlantı hatası!");
        }

        //PhotonNetwork.LoadLevel("LobbyScene");
    }


    public override void OnConnectedToMaster()
    {
        // Master Server'a başarıyla bağlandı
        Debug.Log("Connected.");
    }
    public override void OnJoinedRoom()
    {
        // Odaya başarıyla katıldığınızda bu metot çağrılır
        Debug.Log("Odaya katıldım: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("LobbyScene");
    }


    private void ShowWarning(string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;

        // 3 saniye sonra uyarıyı gizle
        StartCoroutine(HideWarning());
    }

    private IEnumerator HideWarning()
    {
        yield return new WaitForSeconds(3f);
        warningText.gameObject.SetActive(false);
    }
}
