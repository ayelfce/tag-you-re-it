using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class mainMenu : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
