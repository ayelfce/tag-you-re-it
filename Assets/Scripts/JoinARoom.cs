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
        // Eðer baðlanmamýþsak, baðlanmaya çalýþ
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinARoomButton()
    {
        if (string.IsNullOrWhiteSpace(username.text))
        {
            ShowWarning("Lütfen bir kullanýcý adý girin.");
            return;
        }

        if (string.IsNullOrWhiteSpace(roomName.text))
        {
            ShowWarning("Lütfen bir oda adý girin.");
            return;
        }

        PhotonNetwork.NickName = username.text;

        // Odaya katýlma
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName.text);
            Debug.Log("Katýldý.");
        }
        else
        {
            ShowWarning("Baðlantý Hatasý!");
        }

        //PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnConnectedToMaster()
    {
        // Master Server'a baþarýyla baðlandý
        Debug.Log("Connected.");
    }

    public override void OnJoinedRoom()
    {
        // Odaya baþarýyla katýldýðýnda LobbyScene sahnesine geçiþ
        PhotonNetwork.LoadLevel("LobbyScene");  // Odaya katýldýðýnda LobbyScene sahnesine yönlendirilir
    }

    private void ShowWarning(string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;

        // 3 saniye sonra uyarýyý gizle
        StartCoroutine(HideWarning());
    }

    private IEnumerator HideWarning()
    {
        yield return new WaitForSeconds(3f);
        warningText.gameObject.SetActive(false);
    }

    
}
