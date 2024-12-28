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

    private void Start()
    {
        // Başlangıçta trigger'ı kapalı yapıyoruz
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = false;
        Debug.Log("Collider başlangıçta isTrigger: " + collider.isTrigger);
        StartCoroutine(EnableTriggerAfterDelay(20f));
    }

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
                                GameManager.Instance.seekedPlayers.Add(player);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Ebe olmayan oyuncu alana girdi: " + otherPhotonView.Owner.NickName);
                        
                        if (!GameManager.Instance.sobelenemezler.Contains(otherPhotonView.Owner))
                        {
                            GameManager.Instance.sobelenemezler.Add(otherPhotonView.Owner);
                            Debug.Log("SOBE, artık sobelenemez: " + otherPhotonView.Owner.NickName);
                            GameManager.Instance.remainers.Remove(otherPhotonView.Owner);
                            GameManager.Instance.notificationList.Add($"SOBE, artık sobelenemez: {otherPhotonView.Owner.NickName}");
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

    private IEnumerator EnableTriggerAfterDelay(float delay)
    {
        Debug.Log("Coroutine başladı, " + delay + " saniye bekleniyor...");
        yield return new WaitForSeconds(delay);

        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
        Debug.Log("20 saniye sonra isTrigger özelliği: " + collider.isTrigger);
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
