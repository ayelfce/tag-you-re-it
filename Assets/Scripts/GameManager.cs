using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance = null; // Singleton
    private const string RoleProperty = "Role"; // Custom property anahtarı
    private const string EBE = "Ebe";
    private const string HIDING = "Hiding";
    // public GameObject roleBasedUI;

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
            // roleBasedUI.SetActive(true);
        }
    }

    private void AssignRoles()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        int randomIndex = Random.Range(0, players.Length);

        // Ebe seç
        players[randomIndex].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", "EBE" } });

        // Diğer oyuncular için rol belirle
        for (int i = 0; i < players.Length; i++)
        {
            if (i != randomIndex)
            {
                players[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", "HIDING" } });
            }
        }

        Debug.Log("Roles assigned successfully!");
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

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room, initializing Role-Based UI...");
        RoleBasedUI roleBasedUI = FindObjectOfType<RoleBasedUI>();
        if (roleBasedUI != null)
        {
            roleBasedUI.gameObject.SetActive(true);
        }
    }

}