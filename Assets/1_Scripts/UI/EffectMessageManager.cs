using System.Collections;
using TMPro;
using UnityEngine;

public class EffectMessageManager : MonoBehaviour
{
    public static EffectMessageManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _MessageText;
    [SerializeField] private CanvasGroup _MessageCanvasGroup;
    [SerializeField] private float _DisplayDuration = 2f;
    [SerializeField] private float _FadeInDuration = 0.3f;
    [SerializeField] private float _FadeOutDuration = 0.3f;

    [HideInInspector] public float totalDuration;

    private Coroutine _CurrentMessageCoroutine;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_MessageCanvasGroup != null)
        {
            _MessageCanvasGroup.alpha = 0;
        }

        totalDuration = _DisplayDuration + _FadeInDuration + _FadeOutDuration;
    }

    public void ShowMessage(string pMessage)
    {
        if (_CurrentMessageCoroutine != null)
        {
            StopCoroutine(_CurrentMessageCoroutine);
        }

        _CurrentMessageCoroutine = StartCoroutine(DisplayMessage(pMessage));
    }

    private IEnumerator DisplayMessage(string pMessage)
    {
        _MessageText.text = pMessage;

        float lElapsed = 0;
        float lRatio = 0f;
        while (lElapsed < _FadeInDuration)
        {
            lElapsed += Time.deltaTime;
            lRatio = lElapsed / _FadeInDuration;

            _MessageCanvasGroup.alpha = Mathf.Lerp(0, 1 , lRatio);
            yield return null;
        }
        _MessageCanvasGroup.alpha = 1;

        yield return new WaitForSeconds(_DisplayDuration);

        lElapsed = 0;
        lRatio = 0;

        while (lElapsed < _FadeOutDuration)
        {
            lElapsed += Time.deltaTime;
            lRatio += lElapsed / _FadeOutDuration;

            _MessageCanvasGroup.alpha = Mathf.Lerp(1, 0, lRatio);
            yield return null;
        }
        _MessageCanvasGroup.alpha = 0;
    }
}
