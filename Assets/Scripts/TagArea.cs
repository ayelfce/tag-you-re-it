using System.Collections;
using UnityEngine;
using Photon.Pun;

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
        Debug.Log("Collider is isTrigger at the beginning: " + collider.isTrigger);
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
                        Debug.Log("Seeker in in the tag area: " + otherPhotonView.Owner.NickName);

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
                        Debug.Log("Non-seeker player is in the tag area: " + otherPhotonView.Owner.NickName);

                        if (!GameManager.Instance.sobelenemezler.Contains(otherPhotonView.Owner))
                        {
                            GameManager.Instance.sobelenemezler.Add(otherPhotonView.Owner);
                            Debug.Log("Tag, you're it!: " + otherPhotonView.Owner.NickName);
                            GameManager.Instance.remainers.Remove(otherPhotonView.Owner);
                            GameManager.Instance.notificationList.Add($"Tag, you're it!: {otherPhotonView.Owner.NickName}");
                        }
                    }
                }
                else
                {
                    Debug.Log("Player role cannot found.");
                }
            }
        }
    }

    private IEnumerator EnableTriggerAfterDelay(float delay)
    {
        Debug.Log("Coroutine started, " + delay + " waiting...");
        yield return new WaitForSeconds(delay);

        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
        Debug.Log("20 seconds later isTrigger property: " + collider.isTrigger);
        for (int i = GameManager.Instance.remainers.Count - 1; i >= 0; i--)
        {
            Photon.Realtime.Player item = GameManager.Instance.remainers[i];
            if (GameManager.Instance.GetPlayerRole(item) == "EBE")
            {
                GameManager.Instance.remainers.RemoveAt(i);
            }
        }

        Debug.Log("Remainers from 20sc");
        foreach (Photon.Realtime.Player item in GameManager.Instance.remainers)
        {
            Debug.Log($"Remainer: {item.NickName}");
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
