using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinARoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName;
    public TMP_InputField username;
    public TextMeshProUGUI warningText;
    public byte maxPlayers = 4;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void GoBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void JoinARoomButton()
    {
        if (string.IsNullOrWhiteSpace(username.text))
        {
            ShowWarning("Please enter a username.");
            return;
        }

        if (string.IsNullOrWhiteSpace(roomName.text))
        {
            ShowWarning("Please enter a room name.");
            return;
        }

        PhotonNetwork.NickName = username.text;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName.text);
            Debug.Log("Joined.");
        }
        else
        {
            ShowWarning("Connection error!");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected.");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join room: {message}");
        ShowWarning("Room does not exist.");
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
