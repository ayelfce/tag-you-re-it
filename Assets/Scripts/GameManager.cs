// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameManager : MonoBehaviour {

//     public static GameManager Instance = null;                         

//     void Awake()
//     {

//         if (Instance == null)
//         {
//             Instance = this;
//         }

//         else if (Instance != this)
//         {
//             Destroy(gameObject);
//         }

//         // Dont destroy on reloading the scene
//         DontDestroyOnLoad(gameObject);


//     }
//     public Player Player;

// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance = null; // Singleton
    private const string RoleProperty = "Role"; // Custom property anahtarı
    private const string EBE = "Ebe";
    private const string HIDING = "Hiding";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // Sahneler arası geçişte korunur
    }

    void Start()
    {
        // Sadece MasterClient rolleri belirler
        if (PhotonNetwork.IsMasterClient)
        {
            AssignRoles();
        }
    }

    private void AssignRoles()
    {
        // Tüm oyuncuları al
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        // Rastgele bir ebe seç
        int randomIndex = Random.Range(0, players.Length);
        players[randomIndex].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { RoleProperty, EBE } });

        // Diğer oyuncuları saklanan yap
        for (int i = 0; i < players.Length; i++)
        {
            if (i != randomIndex)
            {
                players[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { RoleProperty, HIDING } });
            }
        }
    }

    // Doğru imza
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(RoleProperty))
        {
            string role = (string)changedProps[RoleProperty];
            Debug.Log($"Player {targetPlayer.NickName} is assigned as {role}");
        }
    }

    public string GetPlayerRole(Photon.Realtime.Player player)
    {
        if (player.CustomProperties.TryGetValue(RoleProperty, out object role))
        {
            return (string)role;
        }
        return null;
    }
}

// using UnityEngine;
// using Photon.Pun;
// using TMPro;
// using System.Collections;

// public class GameManager : MonoBehaviourPunCallbacks
// {
//     public TextMeshProUGUI countdownText;
//     public GameObject blackScreenPanel;
//     private bool isEbe = false;
//     private float countdownTime = 10f;

//     private void Start()
//     {
//         if (PhotonNetwork.IsMasterClient)
//         {
//             int randomIndex = Random.Range(0, PhotonNetwork.PlayerList.Length);
//             Photon.Realtime.Player selectedPlayer = PhotonNetwork.PlayerList[randomIndex];

//             photonView.RPC("SetEbe", RpcTarget.All, selectedPlayer.NickName);
//         }
//     }

//     [PunRPC]
//     public void SetEbe(string ebeName)
//     {
//         if (PhotonNetwork.NickName == ebeName)
//         {
//             isEbe = true;
//             StartCoroutine(StartCountdown());
//         }
//     }

//     private IEnumerator StartCountdown()
//     {
//         blackScreenPanel.SetActive(true);
//         countdownText.gameObject.SetActive(true);

//         while (countdownTime > 0)
//         {
//             countdownText.text = "Ebe sensin! " + Mathf.Ceil(countdownTime) + " saniye kaldı!";
//             countdownTime -= Time.deltaTime;
//             yield return null;
//         }

//         countdownText.gameObject.SetActive(false);
//         blackScreenPanel.SetActive(false);
//     }
// }
