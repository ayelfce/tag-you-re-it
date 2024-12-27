using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI newSeeker;
    public void EndGameButton()
{
    PhotonNetwork.LeaveRoom();
}


    // Photon'dan ayrılma işlemi tamamlandığında çağrılır
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
