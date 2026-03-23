using UnityEngine;

public class MenuUiManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject ReadyCanvas;
    [SerializeField] private GameObject PlayerselectorCanvas;
    [SerializeField] private GameObject PlayerCreatorCanvas;

    public void GoToMainMenuCanvas()
    {
        MainMenuCanvas.SetActive(false);
        ReadyCanvas.SetActive(true);
        PlayerselectorCanvas.SetActive(false);
        PlayerCreatorCanvas.SetActive(false);
    }

    public void GoToReadyCanvas()
    {
        MainMenuCanvas.SetActive(false);
        ReadyCanvas.SetActive(true);
        PlayerselectorCanvas.SetActive(false);
        PlayerCreatorCanvas.SetActive(false);
    }

    public void GoToCharacterCreatorCanvas()
    {
        MainMenuCanvas.SetActive(false);
        ReadyCanvas.SetActive(false);
        PlayerselectorCanvas.SetActive(false);
        PlayerCreatorCanvas.SetActive(true);
    }

    public void GoToCharacterSelectorCanvas()
    {
        MainMenuCanvas.SetActive(false);
        ReadyCanvas.SetActive(false);
        PlayerselectorCanvas.SetActive(true);
        PlayerCreatorCanvas.SetActive(false);
    }
}
