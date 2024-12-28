using Photon.Pun;

public class mainMenu : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void CreateRoomButton()
    {
        PhotonNetwork.LoadLevel("CreateARoom");
    }

    public void JoinARoomButton()
    {
        PhotonNetwork.LoadLevel("JoinARoom");
    }
}
