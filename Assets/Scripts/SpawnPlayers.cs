using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint; // Başlangıç spawn noktası
    public float spawnRadius = 5f; // Oyuncuların spawn edileceği maksimum mesafe
    PlayerFollow playerFollow;

    void Awake() {
        playerFollow = FindObjectOfType<PlayerFollow>();
    }

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer() {
        // Rastgele bir pozisyon belirli bir mesafede hesapla
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0; // Yüksekliği sabit tut
        Vector3 spawnPosition = spawnPoint.position + randomOffset;

        // Oyuncuyu spawn et
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnPoint.rotation);
        playerFollow.SetCameraTarget(player.transform);
    }
}
