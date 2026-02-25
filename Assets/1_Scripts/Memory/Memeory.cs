using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Memeory : MonoBehaviour
{
    private const int CARD_TO_FIND_COUNT = 2;
    private const int PLAYER_IN_GAME = 2;
    [SerializeField] private List<MemoryCard> _GeneratedCardList;
    [SerializeField] private List<Sprite> _CardSpritList;

    [SerializeField] private Transform _CardContainer;
    [SerializeField] private Transform _AnimatinCardContainer;
    [SerializeField] private MemoryCard _CardFactory;

    [SerializeField] private MemoryPlayer _PlayerOne;
    [SerializeField] private MemoryPlayer _PlayerTow;
    int _CurrentPlayerIndex = 0;
    private int _PairRemoved = 0;


    private MemoryCard _GeneratedCard;
    private MemoryCard _FirstShowCard = null;
    private MemoryCard _SecondShowCard = null;

    [SerializeField] private float _ComparaisonTransition = .5f;
    private bool _CanInteract = true;



    void Start()
    {
        GenerateCards();
        Shuffle();

        ChooseAPlayer();
        StartGame();
    }

    //player gestion
    private void ChooseAPlayer()
    {
        _CurrentPlayerIndex = Random.Range(0, 2);
    }

    private MemoryPlayer GetCurrentPlayer() => _CurrentPlayerIndex == 0 ? _PlayerOne : _PlayerTow;
    
    private MemoryPlayer GetWaitingPlayer() => _CurrentPlayerIndex == 0 ? _PlayerTow : _PlayerOne;

    private void NexTurn()
    {
        _CurrentPlayerIndex ++;
        if (_CurrentPlayerIndex >= PLAYER_IN_GAME) _CurrentPlayerIndex = 0;
    }

    private void StartGame()
    {
        GetCurrentPlayer().UpdateScor(0);
        GetWaitingPlayer().UpdateScor(0);

        ChooseAPlayer();
        GetCurrentPlayer().StartTurn();
    }

    // generation
    private void GenerateCards()
    {

        for (int i = 0; i < _CardSpritList.Count; i++)
        {
            for (int j = 0; j < CARD_TO_FIND_COUNT; j++)
            {
                CreateCard(i);
            }
        }

    }


    private void CreateCard(int lCardIndex)
    {
        MemoryCard lCard = Instantiate(_CardFactory, _CardContainer);

        _GeneratedCardList.Add(lCard);

        lCard._ShowSprite = _CardSpritList[lCardIndex];
        lCard.cardIndex = lCardIndex;
        lCard._MemoryManager = this;
        lCard._AnimationCardContainer = _CardContainer;

        lCard.HideCard(false);
    }


    //  randomise l'ordre des cartes
    private void Shuffle()
    {
        int lRandomIndex;
        for (int i = 0; i < _GeneratedCardList.Count; i++)
        {
            lRandomIndex = Random.Range(i, _GeneratedCardList.Count);

            MemoryCard temp = _GeneratedCardList[i];
            _GeneratedCardList[i] = _GeneratedCardList[lRandomIndex];
            _GeneratedCardList[lRandomIndex] = temp;
        }

        for (int i = 0; i < _GeneratedCardList.Count; i++)
        {
            _GeneratedCardList[i].transform.SetSiblingIndex(i);
        }
    }

    public void OnCardClicked(int pCardTypeIndex, MemoryCard pCard)
    {
        if (_FirstShowCard == null)
        {
            _FirstShowCard = pCard;
            pCard.ShowCard();
        }
        else
        {
            _SecondShowCard = pCard;
            pCard.ShowCard();
            StartCoroutine(TileComparaisonTransition());
        }
    }


    private IEnumerator TileComparaisonTransition()
    {
        yield return new WaitForSeconds(_ComparaisonTransition);

        CompareCards();
    }


    private void CompareCards()
    {
        if (_FirstShowCard.cardIndex == _SecondShowCard.cardIndex)
        {
            WinACard();
        }
        else PassTurn();
    }

    private void WinACard()
    {
        print("wahahah j'ai une carte");
        
        Vector3 lAnimDirection = (GetCurrentPlayer() == _PlayerOne) ? Vector3.left : Vector3.right;

        _FirstShowCard.PlayWinAnimation(lAnimDirection);
        _SecondShowCard.PlayWinAnimation(lAnimDirection);
        
        _FirstShowCard.WinCardReaction();
        _SecondShowCard.WinCardReaction();

        _FirstShowCard = null;
        _SecondShowCard = null;

        GetCurrentPlayer().StartTurn();
        GetCurrentPlayer().UpdateScor(2);

        _PairRemoved ++;
        if ( _PairRemoved >= _CardSpritList.Count) FinishGame();
    }

    private void PassTurn()
    {
        print ("OhNon");
        
        HideCards();
        NexTurn();

        GetCurrentPlayer().StartTurn();
        GetWaitingPlayer().EndTurn();
    }

    private void HideCards()
    {

        _FirstShowCard.HideCard();
        _SecondShowCard.HideCard();

        _FirstShowCard = null;
        _SecondShowCard = null;
    }
    
    private void FinishGame()
    {
        Player Winer = _PlayerOne.score >= _PlayerTow.score ? GameManager.GetInstance().GetPlayerOne() :GameManager.GetInstance().GetPlayerTow();
        GameManager.GetInstance().WinGame(Winer);
    }
}
