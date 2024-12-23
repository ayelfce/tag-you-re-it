using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerInteractable : MonoBehaviour
{
    private void OnMouseDown()
    {
        // Týklanan objenin Player scriptini bul
        Player clickedPlayer = GetComponent<Player>();
        if (clickedPlayer != null)
        {
            string playerName = clickedPlayer.GetPlayerName();
            Debug.Log($"Týklanan oyuncu: {playerName}");

            // Eðer ebe isen iþlem yap
            if (PhotonNetwork.LocalPlayer.CustomProperties["Role"]?.ToString() == "EBE")
            {
                Debug.Log($"{playerName} isimli oyuncuya týkladýnýz!");
                if (!GameManager.Instance.seenPlayers.Contains(clickedPlayer))
                {
                    clickedPlayer.isSeen = true; // Oyuncuyu seen olarak iþaretle
                    GameManager.Instance.seenPlayers.Add(clickedPlayer); // Listeye ekle
                    Debug.Log($"{playerName} GameManager'daki seenPlayers listesine eklendi!");
                    GameManager.Instance.notificationList.Add($"{playerName} is seen!!!");
                }else
                {
                    Debug.Log($"{playerName} zaten GameManager'daki seenPlayers listesinde!");
                }
            }
        }
        else
        {
            Debug.Log("Týklanan nesnede Player scripti yok.");
        }
    }
}


