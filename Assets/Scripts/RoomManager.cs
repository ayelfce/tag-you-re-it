using System.Collections;
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
    public GameObject startButton;

    private void Start()
    {
        // bağlantı kontrolü
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void GoBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CreateButton()
    {
        // Kullanıcı adı ve oda adı kontrolü
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

        // Odayı yaratmak için önce Master Server'a bağlanmış olmalısınız
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = maxPlayers;
            PhotonNetwork.CreateRoom(roomName.text, roomOpt);
        }
        else
        {
            ShowWarning("Connection error!");
        }
    }


    public override void OnConnectedToMaster()
    {
        // Master Server'a başarıyla bağlandı
        Debug.Log("Connected.");
    }
    public override void OnJoinedRoom()
    {
        // Odaya başarıyla katıldığınızda bu metot çağrılır
        Debug.Log("Joined to room: " + PhotonNetwork.CurrentRoom.Name);
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
