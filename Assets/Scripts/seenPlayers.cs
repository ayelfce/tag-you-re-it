using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class seenPlayers : MonoBehaviourPunCallbacks
{
    public TMP_Text notificationsPlane;

    void Start()
    {
        // TMP_Text bileşeni atanmamışsa bir hata mesajı göster
        if (notificationsPlane == null)
        {
            Debug.LogError("notificationsPlane is not assigned in the Inspector!");
        }

        // GameManager kontrolü
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null! Ensure GameManager exists in the scene.");
        }
        else if (GameManager.Instance.notificationList == null)
        {
            Debug.LogWarning("notificationList is null! Initializing a new list...");
            GameManager.Instance.notificationList = new List<string>();
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.notificationList != null)
        {
            // TMP_Text kontrolü
            if (notificationsPlane != null)
            {
                notificationsPlane.text = ""; // Metni temizle
                foreach (var notification in GameManager.Instance.notificationList)
                {
                    notificationsPlane.text += notification + "\n"; // Bildirimleri ekle
                }
            }
            else
            {
                Debug.LogWarning("notificationsPlane is not assigned but attempting to update it.");
            }
        }
        else
        {
            Debug.LogWarning("GameManager.Instance or notificationList is null!");
        }
    }
}
