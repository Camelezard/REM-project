using UnityEngine;

public class Characterloader : MonoBehaviour
{
    [SerializeField] private CharacterCustomizer _Customizer;
    [SerializeField] private int _PlayerIndex = 1;

    void Start()
    {
        if (_Customizer == null)
        {
            _Customizer = GetComponent<CharacterCustomizer>();
        }
        else
        {
            ApplyPlayerAppearance();
        }


    }

    private void ApplyPlayerAppearance()
    {
        Player lPlayer = GameManager.GetInstance().GetPlayerInList(_PlayerIndex);

        if (lPlayer == null || lPlayer.characterData == null)
            return;

        _Customizer.LoadCharacter(lPlayer.characterData);
    }
}
