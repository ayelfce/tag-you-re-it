using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText; // Odanın adı için metin
    public TMP_Text playerListText; // Oyuncu listesi için metin
    public GameObject startButton; // Oyun başlatma butonu (sadece MasterClient için)

    private void Start()
    {
        // Odaya katılma işlemi başarılı olduğunda bu method çağrılır
        roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name; // Oda adını ekrana yazdır

        UpdatePlayerList(); // Oyuncu listesini güncelle
        StartCoroutine(UpdatePlayerListPeriodically()); // Oyuncu listesini periyodik olarak güncelle
    }

    // Oyuncuların listesi her değiştiğinde güncellenir
    private void UpdatePlayerList()
    {
        playerListText.text = "Players in Room: \n";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n"; // Oda içindeki her oyuncuyu listele
        }

        // Start button'ı yalnızca odayı oluşturan (MasterClient) görebilir
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    // Oyuncu listesini periyodik olarak güncellemek için coroutine
    private IEnumerator UpdatePlayerListPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1 saniye aralıklarla kontrol et
            UpdatePlayerList(); // Listeyi güncelle
        }
    }

    // Oyun başlatma butonuna basıldığında
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient) // Yalnızca odayı kuran kişi bu butona basabilir
        {
            PhotonNetwork.LoadLevel("GameScene"); // Oyunu başlatmak için GameScene'e geçiş yap
        }
    }
}
