using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class TagArea : MonoBehaviourPunCallbacks
{
    private const string EBE_ROLE = "EBE"; // Ebe rolü tanımı
    private Player sobeci;

    // private void OnTriggerEnter(Collider other)
    // {
    //     PhotonView otherPhotonView = other.GetComponent<PhotonView>();

    //     if (otherPhotonView != null && otherPhotonView.Owner != null)
    //     {
    //         // Oyuncunun rolünü al
    //         object playerRole;
    //         if (otherPhotonView.Owner.CustomProperties.TryGetValue("Role", out playerRole))
    //         {
    //             if (playerRole.ToString() == EBE_ROLE)
    //             {
    //                 List<Player> removeList = new List<Player>();
    //                 Debug.Log("Ebe alana girdi: " + otherPhotonView.Owner.NickName);
    //                 foreach (Player player in GameManager.Instance.seenPlayers)
    //                 {
    //                     if (!player.sobelenemez)
    //                     {
    //                         removeList.Add(player);
    //                         GameManager.Instance.seekedPlayers.Add(player);
    //                         GameManager.Instance.notificationList.Add($"{player.GetPlayerName()} is seeked.");
    //                         player.sobelenemez = true;
    //                     }
    //                 }
    //                 foreach (Player player in removeList)
    //                 {
    //                     GameManager.Instance.seenPlayers.Remove(player);
    //                 }
                    
                    
    //                 // Alana girdiğinde bir RPC çağır
    //                 if (PhotonNetwork.IsMasterClient)
    //                 {
    //                     photonView.RPC("OnEbeEnterArea", RpcTarget.All, otherPhotonView.Owner.NickName);
    //                 }
    //             }
    //             else
    //             {
    //                 if (!GameManager.Instance.sobelenemezler.Contains(otherPhotonView.Owner.NickName))
    //                 {
    //                     GameManager.Instance.sobelenemezler.Add(otherPhotonView.Owner.NickName);
    //                     GameManager.Instance.notificationList.Add($"{otherPhotonView.Owner.NickName} COMPLETED.");
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("Oyuncunun rolü bulunamadı.");
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("PhotonView bulunamadı veya Owner yok.");
    //     }
    // }

    private void OnTriggerEnter(Collider other)
{
    PhotonView otherPhotonView = other.GetComponent<PhotonView>();

    if (otherPhotonView != null && otherPhotonView.Owner != null)
    {
        if (GameManager.Instance != null)
        {
            // Oyuncunun rolünü al
            object playerRole;
            if (otherPhotonView.Owner.CustomProperties.TryGetValue("Role", out playerRole))
            {
                if (playerRole.ToString() == EBE_ROLE)
                {
                    List<Player> removeList = new List<Player>();
                    Debug.Log("Ebe alana girdi: " + otherPhotonView.Owner.NickName);

                    // GameManager.Instance.seenPlayers'daki her oyuncu için kontrol yap
                    foreach (Player player in GameManager.Instance.seenPlayers)
                    {
                        // Null kontrolü ve sobelenemez kontrolü
                        if (player != null && !player.sobelenemez)
                        {
                            removeList.Add(player);
                            GameManager.Instance.seekedPlayers.Add(player);
                            GameManager.Instance.notificationList.Add($"{player.GetPlayerName()} is seeked.");
                            player.sobelenemez = true;
                        }
                        else
                        {
                            Debug.LogWarning("Player veya sobelenemez null: " + player);
                        }
                    }

                    // ... (Diğer kodlar)
                    // Buraya diğer işlemlerini ekleyebilirsin

                }
                else
                {
                    // ... (Diğer kodlar)
                    // Buraya diğer işlemlerini ekleyebilirsin
                }
            }
            else
            {
                Debug.Log("Oyuncunun rolü bulunamadı.");
            }
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }
    else
    {
        Debug.Log("PhotonView bulunamadı veya Owner yok.");
    }
}

    [PunRPC]
    public void OnEbeEnterArea(string playerName)
    {
        Debug.Log("Ebe alana girdi (RPC çağrısı): " + playerName);
    }
}
