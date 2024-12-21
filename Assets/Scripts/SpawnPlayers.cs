using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;
    PlayerFollow playerFollow;

    void Awake() {
        playerFollow = FindObjectOfType<PlayerFollow>();
    }

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer() {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        playerFollow.SetCameraTarget(player.transform);
    }

}
