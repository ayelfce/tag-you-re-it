using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TagArea : MonoBehaviourPunCallbacks
{
    private const string EBE_ROLE = "EBE";
    private const string SobelenemezProperty = "Sobelenemez";
    
    private Photon.Realtime.Player sobeci;

    private void OnTriggerEnter(Collider other)
    {
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();

        if (otherPhotonView != null && otherPhotonView.Owner != null)
        {
            if (GameManager.Instance != null)
            {
                object playerRole;
                if (otherPhotonView.Owner.CustomProperties.TryGetValue("Role", out playerRole))
                {
                    if (playerRole.ToString() == EBE_ROLE)
                    {
                        Debug.Log("Ebe alana girdi: " + otherPhotonView.Owner.NickName);
                        
                        // Ebe alanına girdiğinde, seenPlayers listesinde yer alan oyuncuları kontrol et
                        foreach (var player in GameManager.Instance.seenPlayers)
                        {
                            if (player != null && player != otherPhotonView.Owner)
                            {
                                // Eğer hiding rolündeki oyuncu seenPlayers listesinde ve Ebe tag area'ya giriyorsa, oyunu bitir
                                if (GameManager.Instance.GetPlayerRole(player) == "Hiding")
                                {
                                    Debug.Log("Game Over: " + player.NickName + " has been tagged by EBE!");
                                    PhotonView photonView = PhotonView.Get(GameManager.Instance);
                                    photonView.RPC("EndRound", RpcTarget.All); // Oyunu bitir
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        List<Photon.Realtime.Player> removeList = new List<Photon.Realtime.Player>();
                        Debug.Log("Ebe olmayan oyuncu alana girdi: " + otherPhotonView.Owner.NickName);

                        foreach (Photon.Realtime.Player player in GameManager.Instance.seenPlayers)
                        {
                            object playerRoleForSeenPlayer;
                            if (player.CustomProperties.TryGetValue("Role", out playerRoleForSeenPlayer))
                            {
                                if (playerRoleForSeenPlayer.ToString() != EBE_ROLE && !GetSobelenemezProperty(player))
                                {
                                    removeList.Add(player);
                                    GameManager.Instance.seekedPlayers.Add(player);
                                    GameManager.Instance.notificationList.Add($"{player.NickName} is seeked.");
                                    SetSobelenemezProperty(player, true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Oyuncunun rolü bulunamadı.");
                }
            }
        }
    }

    public bool GetSobelenemezProperty(Photon.Realtime.Player player)
    {
        if (player.CustomProperties.TryGetValue(SobelenemezProperty, out object value))
        {
            return (bool)value;
        }
        return false;
    }

    public void SetSobelenemezProperty(Photon.Realtime.Player player, bool value)
    {
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { SobelenemezProperty, value } });
    }

    [PunRPC]
    public void OnEbeEnter(Photon.Realtime.Player player)
    {
        Debug.Log($"{player.NickName} is EBE, players tagged!");
    }
}
