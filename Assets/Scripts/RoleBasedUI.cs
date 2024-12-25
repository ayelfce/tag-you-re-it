using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoleBasedUI : MonoBehaviour
{
    public GameObject blackScreen; // Siyah ekran için GameObject
    public TMP_Text countdownText; // Geri sayım metni
    
    public GameTimer gameTimer;
    public PhotonView photonView;


    private void Start()
    {
        // İlk başta Role kontrol et
        CheckRole();
    }

    private void CheckRole()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            // Role zaten atanmışsa işlemi başlat
            object role = PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            HandleRole(role);
        }
        else
        {
            Debug.LogWarning("Role property is not set for this player. Retrying...");
            // Role atanmadıysa 0.5 saniye bekleyerek tekrar kontrol et
            StartCoroutine(WaitForRoleAssignment());
        }
    }

    private IEnumerator WaitForRoleAssignment()
    {
        // Role ataması yapılana kadar bekle
        int retries = 0; // Deneme sayısını takip et
        while (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role") && retries < 10) // 10 deneme
        {
            Debug.Log("Waiting for Role assignment...");
            retries++;
            yield return new WaitForSeconds(1f); // 1 saniye bekle ve tekrar kontrol et
        }

        if (retries >= 10)
        {
            Debug.LogWarning("Role assignment took too long!");
        }

        // Role ataması tamamlandıktan sonra işlemi başlat
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            object role = PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            Debug.Log($"Role assigned: {role}");
            HandleRole(role);
        }
        else
        {
            Debug.LogWarning("Role still not assigned.");
        }
    }

    private void HandleRole(object role)
    {
        Debug.Log($"Handling role: {role}");
        if (role.ToString() == "EBE")
        {
            StartCoroutine(ShowEbeScreen());
        }
        else
        {
            blackScreen.SetActive(false); // Ebe olmayanlar için siyah ekranı gizle
            // "Hiding" rolü için de geri sayım başlatılacak
            StartGameTimer();
        }
    }

    private IEnumerator ShowEbeScreen()
    {
        Debug.Log("Ebe ekranı gösteriliyor.");

        blackScreen.SetActive(true);

        for (int i = 10; i > 0; i--)
        {
            countdownText.text = $"Ebe sensin\n{i}"; // Geri sayım
            Debug.Log($"Geri sayım: {i}");
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Geri sayım bitti, siyah ekran kapatılıyor.");
        blackScreen.SetActive(false);
        if (gameTimer != null)
        {
            // Ebe ekranında gameTimer başlatılıyor
            gameTimer.StartTimer();  
        }
        countdownText.text = "";
        // Diğer oyuncular için aynı anda başlat
        photonView.RPC("StartGameTimer", RpcTarget.All);
    }

    [PunRPC]
    private void StartGameTimer()
    {
        if (gameTimer != null)
        {
            gameTimer.StartTimer();  // Tüm oyuncular için gameTimer'ı başlat
        }
    }
}
