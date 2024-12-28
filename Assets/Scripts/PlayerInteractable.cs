using UnityEngine;
using Photon.Pun;

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
            Debug.Log($"Clicked Player: {playerName}");

            // Ebe olan oyuncunun kendisine tıklanması durumunda işlemi engelle
            if (PhotonNetwork.LocalPlayer.CustomProperties["Role"]?.ToString() == "EBE")
            {
                if (clickedPlayer == PhotonNetwork.LocalPlayer)
                {
                    Debug.Log("Seeker cannot clicked themself!");
                    return;  // Eğer tıklanan oyuncu Ebe ve kendisi ise, işlemi sonlandır
                }

                Debug.Log($"You clicked to {playerName} !");

                // Eğer tıklanan oyuncu sobelenemezler listesinde değilse ve seenPlayers listesinde değilse
                if (!GameManager.Instance.sobelenemezler.Contains(clickedPlayer) &&
                    !GameManager.Instance.seenPlayers.Contains(clickedPlayer))
                {
                    // Hiding rolünde olduğu kontrolü
                    if (GameManager.Instance.GetPlayerRole(clickedPlayer) == "Hiding")
                    {
                        GameManager.Instance.seenPlayers.Add(clickedPlayer);
                        Debug.Log($"Added to {playerName} seenPlayers in GameManager");

                        // Bu bildirimi tüm oyunculara gönder
                        PhotonView photonView = PhotonView.Get(this);
                        photonView.RPC("ShowNotificationOnAllClients", RpcTarget.All, $"{playerName} is seen!!!");
                    }
                }
                else
                {
                    Debug.Log($"{playerName} is already in sobelenemezler list or seenPlayers list!");
                }
            }
        }
        else
        {
            Debug.Log("Photon.Realtime.Player script not found.");
        }
    }

    // Bu RPC fonksiyonu tüm oyuncularda çalışacak ve bildirimi gösterecek
    [PunRPC]
    public void ShowNotificationOnAllClients(string notificationMessage)
    {
        Debug.Log($"Notification sent: {notificationMessage}");

        // Bildirimi tüm oyunculara göster (örneğin UI ile)
        GameManager.Instance.notificationList.Add(notificationMessage);
    }
}
