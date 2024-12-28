using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInteractable : MonoBehaviour
{
    private void OnMouseDown()
    {
        UpdateNotificationList();
    }

    [PunRPC]
    public void UpdateNotificationList()
    {
        Photon.Realtime.Player clickedPlayer = GetComponent<PhotonView>().Owner;

        if (clickedPlayer != null)
        {
            string playerName = clickedPlayer.NickName;
            Debug.Log($"Tıklanan oyuncu: {playerName}");

            // Ebe olan oyuncunun kendisine tıklanması durumunda işlemi engelle
            if (PhotonNetwork.LocalPlayer.CustomProperties["Role"]?.ToString() == "EBE")
            {
                if (clickedPlayer == PhotonNetwork.LocalPlayer)
                {
                    Debug.Log("Ebe kendisine tıklayamaz!");
                    return;  // Eğer tıklanan oyuncu Ebe ve kendisi ise, işlemi sonlandır
                }

                Debug.Log($"{playerName} isimli oyuncuya tıkladınız!");

                // Eğer tıklanan oyuncu sobelenemezler listesinde değilse ve seenPlayers listesinde değilse
                if (!GameManager.Instance.sobelenemezler.Contains(clickedPlayer) &&
                    !GameManager.Instance.seenPlayers.Contains(clickedPlayer))
                {
                    // Hiding rolünde olduğu kontrolü
                    if (GameManager.Instance.GetPlayerRole(clickedPlayer) == "Hiding")
                    {
                        GameManager.Instance.seenPlayers.Add(clickedPlayer);
                        Debug.Log($"{playerName} GameManager'daki seenPlayers listesine eklendi!");

                        // Bu bildirimi tüm oyunculara göndereceğiz
                        PhotonView photonView = PhotonView.Get(this);
                        photonView.RPC("ShowNotificationOnAllClients", RpcTarget.All, $"{playerName} is seen!!!");
                    }
                }
                else
                {
                    Debug.Log($"{playerName} sobelenemezler listesinde veya zaten seenPlayers listesinde!");
                }
            }
        }
        else
        {
            Debug.Log("Tıklanan nesnede Photon.Realtime.Player scripti yok.");
        }
    }

    // Bu RPC fonksiyonu tüm oyuncularda çalışacak ve bildirimi gösterecek
    [PunRPC]
    public void ShowNotificationOnAllClients(string notificationMessage)
    {
        Debug.Log($"Bildirim Gönderildi: {notificationMessage}");

        // Bildirimi tüm oyunculara göster (örneğin UI ile)
        GameManager.Instance.notificationList.Add(notificationMessage);
    }
}
