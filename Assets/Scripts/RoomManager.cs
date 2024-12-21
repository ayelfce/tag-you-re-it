using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
    }

    public void JoinButton()
    {
        // Kullanıcı adı ve oda adı kontrolü
        if (string.IsNullOrWhiteSpace(username.text))
        {
            ShowWarning("Kullanıcı adı boş olamaz!");
            return;
        }

        if (string.IsNullOrWhiteSpace(roomName.text))
        {
            ShowWarning("Lütfen bir oda adı girin.");
            return;
        }

        // Kullanıcı adı ayarla
        PhotonNetwork.NickName = username.text;

        // Odaya katılma
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName.text);
        }
        else
        {
            ShowWarning("Bağlantı Hatası!");
        }
    }

    public override void OnConnectedToMaster()
    {
        // Master Server'a başarıyla bağlandı
        Debug.Log("Connected.");
    }

    public override void OnJoinedRoom()
    {
        // Odaya başarıyla katıldığında LobbyScene sahnesine geçiş
        PhotonNetwork.LoadLevel("LobbyScene");  // Odaya katıldığında LobbyScene sahnesine yönlendirilir
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
