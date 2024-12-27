using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndRoundScreen : MonoBehaviour
{
    public TextMeshProUGUI previousTaggedPlayerText;

    // Start() veya uygun bir metodda RPC çağrısını tetikleyebilirsiniz
    void Start()
    {
        // GameManager'dan önceki tagger'ı al
        Photon.Realtime.Player previousTagger = GameManager.Instance.GetPreviousTaggedPlayer();

        if (previousTagger != null)
        {
            // Eğer bir önceki ebe varsa, ismimi Text'e yerleştir
            previousTaggedPlayerText.text = "New SEEKER for the next round: " + previousTagger.NickName;
        }
        else
        {
            // Eğer önceki tagger yoksa
            previousTaggedPlayerText.text = "No previous tagger.";
        }

        // EndRound sonrası, RPC çağırarak önceki tagger'ı tüm oyunculara gönder
        GameManager.Instance.GetPreviousTaggedPlayer();
    }

    // EndRound ekranında oyunu başlatma butonu
    public void OnContinueButtonClicked()
    {
        // Yeni oyunda önceki tagger'ı yeni ebe olarak ayarlayabilirsiniz
        Photon.Realtime.Player newTagger = GameManager.Instance.GetPreviousTaggedPlayer();
        if (newTagger != null)
        {
            StartNewGameWithNewTagger(newTagger);
        }
        else
        {
            StartNewGameWithRandomTagger();
        }
    }

    // Yeni oyun başlatma fonksiyonu (eğer önceki ebe varsa)
    void StartNewGameWithNewTagger(Photon.Realtime.Player newTagger)
    {
        Debug.Log("Starting new game with tagger: " + newTagger.NickName);
        // Burada yeni oyun başlatılacak ve yeni ebe atanacak
    }

    // Rastgele bir tagger seçme (eğer önceki tagger yoksa)
    void StartNewGameWithRandomTagger()
    {
        Photon.Realtime.Player randomTagger = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)];
        Debug.Log("Starting new game with random tagger: " + randomTagger.NickName);
        // Burada rastgele yeni ebe atanacak
    }
}
