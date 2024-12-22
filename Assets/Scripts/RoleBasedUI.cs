using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoleBasedUI : MonoBehaviour
{
    public GameObject blackScreen; // Siyah ekran için GameObject
    public TMP_Text countdownText; // Geri sayım metni

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString() == "EBE")
        {
            // Eğer oyuncu Ebe ise ShowEbeScreen coroutine'ini başlat
            StartCoroutine(ShowEbeScreen());
        }
        else
        {
            // Ebe olmayanlar için siyah ekranı gizle
            blackScreen.SetActive(false);
        }
    }

    private IEnumerator ShowEbeScreen()
    {
        // Siyah ekranı aktif et
        blackScreen.SetActive(true);

        // Geri sayımı başlat (10 saniye)
        for (int i = 10; i > 0; i--)
        {
            countdownText.text = "Ebe sensin\n" + i.ToString(); // "Ebe sensin" mesajını ve geri sayımı göster
            yield return new WaitForSeconds(1f); // 1 saniye bekle
        }

        // Geri sayım bitince siyah ekranı kaldır
        blackScreen.SetActive(false);

        // Oyun başladığında başka bir işlem yapabilirsiniz
        // Örneğin: oyun başlatmak için bir fonksiyon çağırabilirsiniz
    }
}
