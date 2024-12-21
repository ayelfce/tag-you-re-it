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
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public TextMeshProUGUI countdownText;
    public GameObject[] players;
    public GameObject blackScreenPanel; // Ebe'nin ekranını karartacak panel
    private bool isEbe = false;
    private float countdownTime = 10f;

    private CharacterController ebeController;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Ebe'yi rastgele seç
            int randomIndex = Random.Range(0, PhotonNetwork.PlayerList.Length);
            PhotonView playerView = PhotonNetwork.PlayerList[randomIndex].TagObject as PhotonView;
            playerView.RPC("SetEbe", RpcTarget.All, true);  // Ebe'yi tüm oyunculara bildir
        }
    }

    [PunRPC]
    public void SetEbe(bool isEbePlayer)
    {
        isEbe = isEbePlayer;

        if (isEbe)
        {
            // Ebe'nin hareketini engelle
            ebeController = GetComponent<CharacterController>();
            StartCoroutine(StartCountdown());
        }
        else
        {
            // Diğer oyuncular için saklanma mekaniğini başlat
            ShowHidePlayers(true);
        }
    }

    private void ShowHidePlayers(bool show)
    {
        foreach (var player in players)
        {
            player.SetActive(show);
        }
    }

    private IEnumerator StartCountdown()
    {
        // Ebe'nin ekranını karart
        blackScreenPanel.SetActive(true);
        countdownText.gameObject.SetActive(true);

        // Ebe'nin hareketini engelle
        while (countdownTime > 0)
        {
            countdownText.text = "Ebe sensin! " + Mathf.Ceil(countdownTime) + " saniye kaldı!";
            countdownTime -= Time.deltaTime;

            // Ebe hareket etmeye çalıştığında engelle
            if (ebeController != null)
            {
                ebeController.Move(Vector3.zero); // Hareketi sıfırla
            }

            yield return null;
        }

        // Geri sayım bitince, normal oyun başlasın
        countdownText.gameObject.SetActive(false);
        blackScreenPanel.SetActive(false);  // Ekranı normale döndür

        // Ebe'nin hareketine izin ver
        ShowHidePlayers(true);  // Diğer oyuncuları görünür yap
    }

    public override void OnJoinedRoom()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
