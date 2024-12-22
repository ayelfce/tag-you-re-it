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
    public GameObject startButton; // Oyun başlatma butonu (herkeste görünür olacak)

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

        // Start button'ı tüm oyuncular için görünebilir olacak
        startButton.SetActive(true);
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
        if (PhotonNetwork.IsMasterClient) // Yalnızca odayı kuran kişi (MasterClient) bu butona basabilir
        {
            Debug.Log("Game starting, calling RPC...");
            photonView.RPC("LoadGameScene", RpcTarget.AllBuffered); // RPC tüm oyunculara çağrılacak
        }
        else
        {
            Debug.Log("Only the MasterClient can start the game.");
        }
    }

    // RPC metodu, tüm oyunculara sahne yüklenmesini iletir
    [PunRPC]
    private void LoadGameScene()
    {
        Debug.Log("Loading game scene for all players...");
        PhotonNetwork.LoadLevel("GameScene"); // Burada sahne yükleniyor
    }

    // Oyun başladığında, MasterClient dışında her oyuncu sahneye yüklenecek
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Player joined room: " + PhotonNetwork.LocalPlayer.NickName);
    }
}
