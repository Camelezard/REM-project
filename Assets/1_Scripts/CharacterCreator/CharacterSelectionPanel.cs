using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectionPanel : MonoBehaviour
{
    [SerializeField] private CharacterLoader _characterLoader;
    [Header("UI References")]
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Button _toggleButton;
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private GameObject _buttonPrefab;

    [Header("Animation")]
    [SerializeField] private float _slideTime = 0.3f;
    [SerializeField] private float _panelHiddenX = -500f;
    [SerializeField] private float _panelShownX = 0f;

    private int _currentSlotIndex = -1;

    public int CurrentSlotIndex => _currentSlotIndex;

    private bool _isShown = false;
    private List<Button> _characterButtons = new List<Button>();

    void Start()
    {
        if (_toggleButton != null)
            _toggleButton.onClick.AddListener(TogglePanel);

        PopulateCharacterButtons();
        HidePanelInstant();
    }

    private void PopulateCharacterButtons()
    {
        // Clear existing buttons
        foreach (Transform child in _buttonContainer)
            Destroy(child.gameObject);

        _characterButtons.Clear();

        // Get all saved characters from CharacterDatabase
        List<CharacterData> savedCharacters = CharacterDatabase.instance.GetAllSavedCharacters();

        for (int i = 0; i < savedCharacters.Count; i++)
        {
            CharacterData data = savedCharacters[i];
            if (data == null) continue;

            GameObject btnObj = Instantiate(_buttonPrefab, _buttonContainer);
            Button btn = btnObj.GetComponent<Button>();
            int index = i; // capture pour closure

            btn.onClick.AddListener(() => OnCharacterSelected(index));

            // Set button text
            Text btnText = btnObj.GetComponentInChildren<Text>();

            _characterButtons.Add(btn);
        }
    }

    private void OnCharacterSelected(int slotIndex)
    {
        _currentSlotIndex = slotIndex;

        if (_characterLoader != null)
        {
            _characterLoader.LoadFromSlot(slotIndex);
        }

        // Fermer le panneau
        SlidePanel(false);
    }
    public void TogglePanel()
    {
        SlidePanel(!_isShown);
    }

    private void SlidePanel(bool show)
    {
        StopAllCoroutines();
        StartCoroutine(SlideCoroutine(show));
    }

    private IEnumerator SlideCoroutine(bool show)
    {
        float startX = _panel.anchoredPosition.x;
        float endX = show ? _panelShownX : _panelHiddenX;

        float t = 0f;

        while (t < _slideTime)
        {
            t += Time.deltaTime;
            float x = Mathf.Lerp(startX, endX, t / _slideTime);
            _panel.anchoredPosition = new Vector2(x, _panel.anchoredPosition.y);
            yield return null;
        }

        // Forcer la position finale exacte
        _panel.anchoredPosition = new Vector2(endX, _panel.anchoredPosition.y);
        _isShown = show; // mettre à jour seulement après la fin de l'animation
    }

    private void HidePanelInstant()
    {
        _panel.anchoredPosition = new Vector2(_panelHiddenX, _panel.anchoredPosition.y);
        _isShown = false;
    }
}