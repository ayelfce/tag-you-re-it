using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class SpawnPlayers : MonoBehaviour
{
    public List<GameObject> playerPrefabList;  // Karakter prefab'larının listesi
    public Transform spawnPoint;               // Başlangıç spawn noktası
    public float spawnRadius = 5f;             // Oyuncuların spawn edileceği maksimum mesafe
    private PlayerFollow playerFollow;

    private List<int> usedPrefabIndexes = new List<int>();  // Daha önce kullanılan prefab'ların index'leri

    void Awake()
    {
        playerFollow = FindObjectOfType<PlayerFollow>();
    }

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Rastgele bir pozisyon belirli bir mesafede hesapla
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0;  // Yüksekliği sabit tut
        Vector3 spawnPosition = spawnPoint.position + randomOffset;

        // Henüz kullanılmayan prefab'lar için bir liste oluştur
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < playerPrefabList.Count; i++)
        {
            if (!usedPrefabIndexes.Contains(i)) // Eğer prefab daha önce kullanılmamışsa
            {
                availableIndexes.Add(i);
            }
        }

        // Eğer kullanılabilir prefab varsa
        if (availableIndexes.Count > 0)
        {
            // Kullanılmamış prefab'lardan birini rastgele seç
            int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
            GameObject randomPlayerPrefab = playerPrefabList[randomIndex];

            // Seçilen prefab ile oyuncu oluşturuluyor
            GameObject player = PhotonNetwork.Instantiate(randomPlayerPrefab.name, spawnPosition, spawnPoint.rotation);
            playerFollow.SetCameraTarget(player.transform);

            // Seçilen prefab'ı kullanılan prefab listesine ekle
            usedPrefabIndexes.Add(randomIndex);
        }
        else
        {
            Debug.LogWarning("Tüm prefab'lar kullanıldı!");
        }
    }
}
