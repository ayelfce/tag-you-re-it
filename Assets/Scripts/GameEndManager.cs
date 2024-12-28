using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndRoundScreen : MonoBehaviour
{
    public static EndRoundScreen Instance { get; private set; }
    public TextMeshProUGUI previousTaggedPlayerText;
    public Photon.Realtime.Player ebemiss1, ebemiss;

    // Start() veya uygun bir metodda RPC çağrısını tetikleyebilirsiniz
    void Start()
    {
        //PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "NewEbe", GameManager.Instance.ebemiz.NickName } });
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("NewEbe", out object ebeNick))
        {
            foreach(Photon.Realtime.Player ebe in GameManager.Instance.allPlayers)
            {
                if(ebe.NickName == (string)ebeNick)
                {
                    ebemiss = ebe;
                    Debug.Log($"Ebemiz: {ebemiss.NickName}");
                }
            } // Tür dönüşümünü yapın
        }
        else
        {
            Debug.Log("NewEbe not found in room properties.");
        }

        // GameManager'dan önceki tagger'ı al
        //Photon.Realtime.Player previousTagger = GameManager.Instance.GetPreviousTaggedPlayer();

        if (ebemiss != null)
        {
            // Eğer bir önceki ebe varsa, ismimi Text'e yerleştir
            previousTaggedPlayerText.text = "New SEEKER for the next round: " + ebemiss.NickName;
        }
        else
        {
            // Eğer önceki tagger yoksa
            previousTaggedPlayerText.text = "No previous tagger.";
        }

        // EndRound sonrası, RPC çağırarak önceki tagger'ı tüm oyunculara gönder
        //GameManager.Instance.GetPreviousTaggedPlayer();
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
