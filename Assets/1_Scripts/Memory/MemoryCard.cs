using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] public Image _CardSprite;
    [SerializeField] public Image _CardAspect;
    [SerializeField] Sprite _HideSprite;
    [SerializeField] Sprite _ReveldSpriteAspeect;
    [SerializeField] float _ShowRotationDelay = 1;
    [SerializeField] float _ExitRotationDelay = 1;
    [SerializeField] Button _CardButton;

    [SerializeField] private Vector3 _JuiceRotation;

    public Transform _AnimationCardContainer;



    public Sprite _ShowSprite;
    public Memeory _MemoryManager;

    public int cardIndex = 0;

    private bool isTured = false;

    void Start()
    {
        _CardAspect.sprite = _HideSprite;
        _CardSprite.color = new Color(1, 1, 1, 0);
    }


    public void OnCardClicked()
    {
        Debug.Log("Carte cliquée ID : " + cardIndex);
        _MemoryManager.OnCardClicked(cardIndex, this);
    }

    public void ShowCard(bool pTurnCard = true)
    {
        if (pTurnCard) StartCoroutine(TurnCard());
        else _CardSprite.sprite = _ShowSprite;
    }

    public void HideCard(bool pTurnCard = true)
    {
        if (pTurnCard) StartCoroutine(TurnCard());
        else _CardSprite.sprite = _HideSprite;
    }

    public void WinCardReaction()
    {
        _CardButton.interactable = false;
    }

    private IEnumerator TurnCard()
    {
        float lElapsTime = 0f;
        float lProgress = 0f;
        bool lCanChangeImage = true;

        Sprite startSprite = isTured ? _ShowSprite : null;
        Sprite endSprite = isTured ? null : _ShowSprite;

        _CardSprite.sprite = startSprite;

        while (lElapsTime < _ShowRotationDelay)
        {
            lElapsTime += Time.deltaTime;
            lProgress = lElapsTime / _ShowRotationDelay;

            float lAngle = 180f * lProgress;

            if (lCanChangeImage && lProgress >= 0.5f)
            {
                lCanChangeImage = false;
                _CardSprite.sprite = endSprite;

                if (endSprite == null)
                {
                    _CardSprite.color = new Color(1, 1, 1, 0);
                    _CardAspect.sprite = _HideSprite;
                }
                else
                {
                    _CardSprite.color = new Color(1, 1, 1, 1);
                    _CardAspect.sprite = _ReveldSpriteAspeect;
                }
            }

            transform.localRotation = Quaternion.Euler(0, isTured ? 180f - lAngle : lAngle, 0);


            yield return null;
        }

        isTured = !isTured;
    }


    public void PlayWinAnimation(Vector3 direction)
    {
        StartCoroutine(MoveCardForScore(direction));
    }


    private float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;

        return c3 * t * t * t - c1 * t * t;
    }


    IEnumerator MoveCardForScore(Vector3 direction)
    {
        transform.SetParent(_AnimationCardContainer);

        RectTransform lRect = GetComponent<RectTransform>();

        Vector2 lStartPos = lRect.anchoredPosition;
        Vector2 lDir = ((Vector2)direction).normalized;

        Quaternion lStartRotation = transform.rotation;


        float lDistance = 800f;

        float lElapsed = 0f;

        float t;

        float lEased;

        while (lElapsed < _ExitRotationDelay)
        {
            lElapsed += Time.deltaTime;

            t = lElapsed / _ExitRotationDelay;

            lEased = EaseInBack(t);

            lRect.anchoredPosition = lStartPos + lDir * lDistance * lEased;
            transform.rotation = lStartRotation * Quaternion.Euler(_JuiceRotation * lEased);

            yield return null;
        }
    }

}
