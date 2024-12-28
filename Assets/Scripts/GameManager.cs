using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }
    private const string RoleProperty = "Role"; // Custom property anahtarı
    private const string EBE = "EBE";
    private const string HIDING = "Hiding";
    private Photon.Realtime.Player previousTaggedPlayer = null; // Bir önceki turda taglenen kişi
    public List<Photon.Realtime.Player> sobelenemezler = new List<Photon.Realtime.Player>();
    public List<Photon.Realtime.Player> seenPlayers = new List<Photon.Realtime.Player>();
    public List<Photon.Realtime.Player> seekedPlayers = new List<Photon.Realtime.Player>();
    public List<string> notificationList = new List<string>();
    public bool tourEnd = false;
    public Photon.Realtime.Player ebemiz = null;
    public Photon.Realtime.Player[] allPlayers;
    public List<Photon.Realtime.Player> remainers = new List<Photon.Realtime.Player>();
    public GameTimer timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Önceki tagger'ı belirlemek için bir metot
    public void SetPreviousTaggedPlayer(Photon.Realtime.Player player)
    {
        previousTaggedPlayer = player;
    }

    // Önceki tagger'ı almak için bir metot
    public Photon.Realtime.Player GetPreviousTaggedPlayer()
    {
        return previousTaggedPlayer;
    }

    void Start()
    {
        allPlayers = PhotonNetwork.PlayerList;
        remainers.Clear();
        foreach (Photon.Realtime.Player p in allPlayers)
        {
            Debug.Log("Player added to remainers: " + p.NickName);
            remainers.Add(p);
        }

        if (notificationList == null)
        {
            notificationList = new List<string>();
        }

        if (seenPlayers == null)
        {
            seenPlayers = new List<Photon.Realtime.Player>();
        }

        if (seekedPlayers == null)
        {
            seekedPlayers = new List<Photon.Realtime.Player>();
        }

        if (sobelenemezler == null)
        {
            sobelenemezler = new List<Photon.Realtime.Player>();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            AssignRoles();
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (GetPlayerRole(player) == EBE)
            {
                sobelenemezler.Add(player);
                Debug.Log($"{player.NickName} added to sobelenemezler.");
            }
        }
    }

    void Update()
    {

        if (seekedPlayers.Count != 0 && !tourEnd)
        {
            notificationList.Add($"Game Over: {seekedPlayers[0].NickName} is SEEKER");
            ebemiz = seekedPlayers[0];
            tourEnd = true;
        }

        if (remainers.Count() == 0 && !tourEnd)
        {
            foreach (Photon.Realtime.Player player in allPlayers)
            {
                object playerRol;
                player.CustomProperties.TryGetValue("Role", out playerRol);
                if (playerRol.ToString() == "EBE")
                {
                    ebemiz = player;
                    tourEnd = true;
                }
            }
        }
        if (tourEnd)
        {
            Debug.Log("Tour Ends");
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "NewEbe", ebemiz.NickName } });
            StartCoroutine(CheckCustomPropertiesAndProceed());
        }

        // sobelenemezler listesinde olan oyuncuları seenPlayers'dan çıkart
        List<Photon.Realtime.Player> removee = new List<Photon.Realtime.Player>();
        foreach (Photon.Realtime.Player player in seenPlayers)
        {
            if (sobelenemezler.Contains(player))
            {
                removee.Add(player);
            }
        }

        foreach (Photon.Realtime.Player playere in removee)
        {
            seenPlayers.Remove(playere);
            Debug.Log($"{playere.NickName} removed from seenPlayers because they are sobelenemez.");
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

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room, initializing Role-Based UI...");
        RoleBasedUI roleBasedUI = FindObjectOfType<RoleBasedUI>();
        if (roleBasedUI != null)
        {
            roleBasedUI.gameObject.SetActive(true);
        }
    }

    private IEnumerator CheckCustomPropertiesAndProceed()
    {
        yield return new WaitForSeconds(2f); // 2 saniye bekle

        // "NewEbe" özelliği her oyuncuya ulaştı mı kontrol et
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("NewEbe", out object ebeNick))
        {
            Debug.Log("NewEbe property has been distributed successfully.");
            photonView.RPC("EndRoundS", RpcTarget.All);
        }
        else
        {
            Debug.LogWarning("NewEbe property was not distributed to all players in time.");
        }
    }


    [PunRPC]
    public void EndRoundS()
    {
        Debug.Log("End Round Came");
        SceneManager.LoadScene("EndRoundScreen");
    }

    public void AssignRoles()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        int ebeIndex = -1;

        // Bir önceki taglenen oyuncuyu bul
        if (previousTaggedPlayer != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == previousTaggedPlayer)
                {
                    ebeIndex = i;
                }
            }
        }

        // Eğer previousTaggedPlayer yoksa rastgele bir ebe seç
        if (ebeIndex == -1)
        {
            ebeIndex = Random.Range(0, players.Length);
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (i == ebeIndex)
            {
                players[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", EBE } });
                Debug.Log($"{players[i].NickName} is the new EBE.");
            }
            else
            {
                players[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", HIDING } });
            }
        }
        Debug.Log("1");
        // Güncellenen sobelenemez listesi
        sobelenemezler.Clear();
        foreach (Photon.Realtime.Player playeri in players)
        {
            Debug.Log("0");
            if (GetPlayerRole(playeri) == EBE)
            {
                sobelenemezler.Add(playeri);
                Debug.Log($"{playeri.NickName} added to sobelenemezler.");
            }
        }
    }

    private void LoadEndRoundScene()
    {
        SceneManager.LoadScene("EndRoundScreen");
    }

    [PunRPC]
    public void ResetTimer()
    {
        if (timer != null)
        {
            timer.ResetTimer();
            Debug.Log("Timer reset.");
        }
        else
        {
            Debug.LogWarning("Timer reset is unsuccessful.");
        }
    }

    [PunRPC]
    public void ResetSeenList()
    {
        if (seenPlayers != null)
        {
            seenPlayers.Clear();
            Debug.Log("Görülen oyuncu listesi sıfırlandı.");
        }
        else
        {
            Debug.LogWarning("Görülen oyuncu listesi null, sıfırlama başarısız.");
        }
    }

}
