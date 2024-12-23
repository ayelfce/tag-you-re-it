using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TagArea : MonoBehaviourPunCallbacks
{
    // OnTriggerEnter fonksiyonu, Photon nesnesi olsa bile çalışır
    private void OnTriggerEnter(Collider other)
    {
        // Trigger'a giren nesne Photon nesnesi de olsa algılar
        Debug.Log("Trigger'a girildi: " + other.gameObject.name);
        
        // Eğer giren nesne bir PhotonView nesnesiyse, bunu ağda bildir
        if (other.gameObject.GetComponent<PhotonView>() != null)
        {
            Debug.Log("Photon nesnesi ile trigger'a girildi: " + other.gameObject.name);
            
            // Burada istediğiniz işlemi yapabilirsiniz, örneğin RPC çağırmak
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("HandleTriggerEvent", RpcTarget.All);
            }
        }
    }

    // Bu RPC metodu, tüm oyunculara trigger olayını bildirir
    [PunRPC]
    public void HandleTriggerEvent()
    {
        // Bu metod tüm istemcilerde çağrılacaktır
        Debug.Log("Trigger olayı tüm oyuncularda işleniyor");
    }
}
