using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class seenPlayers : MonoBehaviourPunCallbacks
{
    public TMP_Text notificationsPlane;

    // Start is called before the first frame update
    void Start()
    {
        if (notificationsPlane == null)
        {
            Debug.LogError("notificationsPlane is not assigned in the Inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.notificationList != null)
        {
            notificationsPlane.text = ""; // Önce metni temizle
            foreach (var notification in GameManager.Instance.notificationList)
            {
                notificationsPlane.text += notification + "\n"; // Bildirimleri listele
            }
        }
        else
        {
            Debug.LogWarning("GameManager.Instance or notificationList is null!");
        }
    }
}
