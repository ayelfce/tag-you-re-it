using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class JoinARoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName;
    public TMP_InputField username;
    public TextMeshProUGUI warningText;
    public byte maxPlayers = 4;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinARoomButton()
    {
        if (string.IsNullOrWhiteSpace(username.text))
        {
            ShowWarning("Lütfen bir kullanıcı adı girin.");
            return;
        }

        if (string.IsNullOrWhiteSpace(roomName.text))
        {
            ShowWarning("Lütfen bir oda ad� girin.");
            return;
        }

        PhotonNetwork.NickName = username.text;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName.text);
            Debug.Log("Katıldı.");
        }
        else
        {
            ShowWarning("Bağlantı Hatası!");
        }

        //PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected.");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private void ShowWarning(string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;

        // 3 saniye sonra uyar�y� gizle
        StartCoroutine(HideWarning());
    }

    private IEnumerator HideWarning()
    {
        yield return new WaitForSeconds(3f);
        warningText.gameObject.SetActive(false);
    }

    
}
