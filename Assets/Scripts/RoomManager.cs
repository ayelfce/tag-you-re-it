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

    public byte maxPlayers;

    public void CreateButton() {
        RoomOptions roomOpt = new RoomOptions();
        roomOpt.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(roomName.text, roomOpt);
    }

    public void JoinButton() {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
