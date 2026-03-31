using UnityEngine;

public class MenuUiManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject ReadyCanvas;
    [SerializeField] private GameObject PlayerselectorCanvas;
    [SerializeField] private GameObject PlayerCreatorCanvas;
    [SerializeField] private GameObject PlayerCanvas;

    private int SaveId = 1;


    void Start()
    {
        GoToMainMenuCanvas();
        
    }
    public void GoToMainMenuCanvas()
    {
        MainMenuCanvas.SetActive(true);
        ReadyCanvas.SetActive(false);
        PlayerselectorCanvas.SetActive(false);
        PlayerCreatorCanvas.SetActive(false);
        PlayerCanvas.SetActive(false);
    }

    public void GoToReadyCanvas()
    {
        MainMenuCanvas.SetActive(false);
        ReadyCanvas.SetActive(true);
        PlayerselectorCanvas.SetActive(false);
        PlayerCreatorCanvas.SetActive(false);
        PlayerCreatorCanvas.SetActive(false);
    }

    public void GoToCharacterCreatorCanvas()
    {
        MainMenuCanvas.SetActive(false);
        ReadyCanvas.SetActive(false);
        PlayerselectorCanvas.SetActive(false);
        PlayerCreatorCanvas.SetActive(true);
        PlayerCanvas.SetActive(true);
    }

    public void GoToCharacterSelectorCanvas()
    {
        MainMenuCanvas.SetActive(false);
        ReadyCanvas.SetActive(false);
        PlayerselectorCanvas.SetActive(true);
        PlayerCreatorCanvas.SetActive(false);
        PlayerCanvas.SetActive(true);
    }
}
