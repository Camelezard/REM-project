using UnityEngine;
using UnityEngine.UI;

public class MemoryPlayer : MonoBehaviour
{
    [SerializeField] private Text _ScoreText;
    [SerializeField] private Text _MessageText;

    public int score { get; private set; }
    private static int _MesageIndex = 0;

    private readonly string[] TURN_MESSAGES =
{
    "Allez !",
    "A ton tour !",
    "Concentre-toi !",
    "Tu peux le faire !",
    "Ne lâche pas !",
    "Reste attentif !",
    "Fais confiance à ta mémoire !",
    "Fais le bon choix !",
    "Persévère !",
    "Reste focus !",
    "Prends ton temps !",
    "Réfléchis bien !",
    "Ça va venir !"
};



    public void UpdateScor(int pScore)
    {
        score += pScore;
        _ScoreText.text = score.ToString();
    }

    public void StartTurn()
    {

        int lLastIndex = _MesageIndex;
        do
        {
            _MesageIndex = Random.Range(0, TURN_MESSAGES.Length);
        } while (lLastIndex == _MesageIndex);

        _MessageText.text = TURN_MESSAGES[_MesageIndex];
    }


    public void EndTurn()
    {
        _MessageText.text = null;
    }
}

