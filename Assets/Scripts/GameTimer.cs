using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float timeLeft = 120f;
    private bool timerIsRunning = false;

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
            }
        }
    }

    void UpdateCountdownText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
