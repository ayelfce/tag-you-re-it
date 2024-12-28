using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
    private bool tourEnd = false;
    public Photon.Realtime.Player[] allPlayers;
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

        allPlayers = PhotonNetwork.PlayerList;

        foreach (Photon.Realtime.Player p in allPlayers)
        {
            Debug.Log("Oyuncu: " + p.NickName);
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
            tourEnd = true;
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("EndRound", RpcTarget.All);
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

    // private void AssignRoles()
    // {
    //     Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
    //     int randomIndex = Random.Range(0, players.Length);

    //     players[randomIndex].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", EBE } });

    //     for (int i = 0; i < players.Length; i++)
    //     {
    //         if (i != randomIndex)
    //         {
    //             players[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", HIDING } });
    //         }
    //     }

    //     Debug.Log("Roles assigned successfully!");

    //     foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
    //     {
    //         if (GetPlayerRole(player) == EBE)
    //         {
    //             if (!seenPlayers.Contains(player))
    //             {
    //                 Debug.Log($"{player.NickName} is EBE and should not be added to seenPlayers.");
    //             }
    //         }
    //     }

    //     foreach (Photon.Realtime.Player player in players)
    //     {
    //         if (GetPlayerRole(player) == EBE)
    //         {
    //             sobelenemezler[player] = true;
    //             Debug.Log($"{player.NickName} is added to sobelenemezler.");
    //         }
    //     }
    // }

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

    public void OtherEndings()
    {
        if (seekedPlayers.Count == (allPlayers.Length - 1) && seekedPlayers.Count != 0)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("EndRound", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);


        }

        if (timer != null && timer.timeLeft <= 0)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("EndRound", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);


        }
    }

    [PunRPC]
    public void EndRound(string taggedPlayerName)
    {
        // Bir önceki ebe olarak taglenen oyuncuyu kaydet
        previousTaggedPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => p.NickName == taggedPlayerName);

        // Yeni sahneye geçiş
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
                    break;
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

        // Güncellenen sobelenemez listesi
        sobelenemezler.Clear();
        foreach (Photon.Realtime.Player player in players)
        {
            if (GetPlayerRole(player) == EBE)
            {
                sobelenemezler.Add(player);
                Debug.Log($"{player.NickName} added to sobelenemezler.");
            }
        }
    }

    private void LoadEndRoundScene()
    {
        SceneManager.LoadScene("EndRoundScreen");
    }

}
