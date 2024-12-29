using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Connecting : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        SceneManager.LoadScene("MainMenu");
    }

}
