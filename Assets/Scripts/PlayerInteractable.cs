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
            }
        }
        else
        {
            Debug.Log("Týklanan nesnede Player scripti yok.");
        }
    }
}


