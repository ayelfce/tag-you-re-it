using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float timeLeft = 120f;
    private bool timerIsRunning = false;

    [PunRPC]
    public void StartTimer()
    {
        timerIsRunning = true;  // Timer'ı başlat
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateCountdownText(timeLeft);
            }
            else
            {
                timerIsRunning = false;
                countdownText.text = "Time's Up!";
                GameManager.Instance.tourEnd = true;
                foreach (Photon.Realtime.Player player in GameManager.Instance.allPlayers)
                {
                    object playerRol;
                    player.CustomProperties.TryGetValue("Role", out playerRol);
                    if (playerRol.ToString() == "EBE")
                    {
                        GameManager.Instance.ebemiz = player;

                    }


                }
                //PhotonView photonView = PhotonView.Get(GameManager.Instance);
                //photonView.RPC("EndRound", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void UpdateCountdownText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    public void ResetTimer()
    {
        timeLeft = 120f; // Başlangıç zamanına sıfırlar
        Debug.Log("Timer sıfırlandı.");
    }
}
