using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public static event Action<int> PlayerCardsDrawn,DealerCardsDrawn;
    public static event Action<int> UpdatePlayerBalance;

    public UIManager uiManager;
    public CardManager cardManager;
    [SerializeField] private int _playerValue;
    [SerializeField] private int _dealerValue;
    [SerializeField] private GameObject _playerHand;
    [SerializeField] private GameObject _dealerHand;


    private List<GameObject> _discardPile = new List<GameObject>();
    private List<Card> _playerCards = new List<Card>();
    private List<Card> _dealerCards = new List<Card>();
    private int _playerBalanceAmount = 500;
    private int _playerBetAmount = 0;
    public bool paused = true;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        uiManager = GetComponentInChildren<UIManager>();
        cardManager = GetComponentInChildren<CardManager>();
        Chip.OnChipClicked += UpdatePlayerBet;
        UpdatePlayerBalance += UpdateBalance;

    }
    void Start()
    {
        cardManager.InitializeCards();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == true)
            {
                uiManager.Play();
                paused = false;
            }
            else
            {
                uiManager.Pause();
                paused = true;
            }
            
        }

            
    }

    public void DistributeCards()
    {
        GameObject DealerCard1 = cardManager.DealCard();
        DealerCard1.transform.SetParent(_dealerHand.transform);
        DealerCard1.transform.localPosition = Vector3.zero + new Vector3(1f, 0);

        GameObject PlayerCard1 = cardManager.DealCard();
        PlayerCard1.transform.SetParent(_playerHand.transform);
        PlayerCard1.transform.localPosition = Vector3.zero + new Vector3(1f, 0);
        
        GameObject DealerCard2 = cardManager.DealCard();
        DealerCard2.transform.SetParent(_dealerHand.transform);
        DealerCard2.transform.localPosition = Vector3.zero + new Vector3(-1f,0);
        cardManager.FlipCard(DealerCard2);

        GameObject PlayerCard2 = cardManager.DealCard();
        PlayerCard2.transform.SetParent(_playerHand.transform);
        PlayerCard2.transform.localPosition = Vector3.zero + new Vector3(-1f, 0);

        _discardPile.Add(PlayerCard2);
        _discardPile.Add(PlayerCard1);
        _discardPile.Add(DealerCard1);
        _discardPile.Add(DealerCard2);

        _playerCards.Add(PlayerCard1.GetComponent<Card>());
        _playerCards.Add(PlayerCard2.GetComponent<Card>());

        _dealerCards.Add(DealerCard1.GetComponent<Card>());
        _dealerCards.Add(DealerCard2.GetComponent<Card>());

        UpdateHandValues();
    }

    public void HitCards()
    {
        GameObject PlayerCard1 = cardManager.DealCard();
        PlayerCard1.transform.SetParent(_playerHand.transform);
        PlayerCard1.transform.localPosition = Vector3.zero + new Vector3(3f, 0);

        GameObject DealerCard1 = cardManager.DealCard();
        DealerCard1.transform.SetParent(_dealerHand.transform);
        DealerCard1.transform.localPosition = Vector3.zero + new Vector3(3f, 0);
        cardManager.FlipCard(DealerCard1);

        _discardPile.Add(PlayerCard1);
        _discardPile.Add(DealerCard1);
        _playerCards.Add(PlayerCard1.GetComponent<Card>());
        _dealerCards.Add(DealerCard1.GetComponent<Card>());

        UpdateHandValues();
    }

    public int DetermineWinner(bool playerBust = false,bool dealerBust = false)
    {
        for(int i = 1;i < _dealerCards.Count; i++)
        {
            cardManager.FlipCard(_dealerCards[i].gameObject);
        }
        if(playerBust && !dealerBust)
        {
            ResetHands();
            return 0;
        }
        else if(dealerBust && !playerBust)
        {
            UpdatePlayerBalance?.Invoke(-2 * _playerBetAmount);
            ResetHands();
            return 1;
        }   
        else if(dealerBust && playerBust)
        {
            UpdatePlayerBalance?.Invoke(-1 * _playerBetAmount);
            ResetHands();
            return -1;
        }
        if (21-_playerValue > 21-_dealerValue)
        {
            ResetHands();
            return 0;
        }
        else if(21-_playerValue < 21-_dealerValue)
        {
            UpdatePlayerBalance?.Invoke(-2 * _playerBetAmount);
            ResetHands();
            return 1;
        }
        else
        {
            UpdatePlayerBalance?.Invoke(-1 * _playerBalanceAmount);
            ResetHands();
            return -1;
        }

    }

    private void UpdateHandValues()
    {
        _playerValue = 0;
        _dealerValue = 0;
        foreach (var item in _playerCards)
        {
            _playerValue += item.cardValue;
        }

        foreach (var item in _dealerCards)
        {
            _dealerValue += item.cardValue;
        }
        if(_playerValue > 21 && _dealerValue <= 21)
        {
            uiManager.FlashRoundResults(DetermineWinner(true,false));
        }
        else if(_dealerValue > 21 && _playerValue <= 21)
        {
            uiManager.FlashRoundResults(DetermineWinner(false, true));
        }
        else if(_dealerValue > 21 && _playerValue > 21)
        {
            uiManager.FlashRoundResults(DetermineWinner(true, true));
        }
        PlayerCardsDrawn?.Invoke(_playerValue);
        DealerCardsDrawn?.Invoke(_dealerValue);

    }

    private void ResetHands()
    {
        StartCoroutine(ResetRound());
    }

    private void UpdateBalance(int amt)
    {
        _playerBalanceAmount -= amt;
        uiManager.UpdateBalanceText(_playerBalanceAmount);

    }

    private void UpdatePlayerBet(int bet)
    {
        if (bet > _playerBalanceAmount)
        {
            return;
        }
        _playerBetAmount += bet;
        UpdateBalance(bet);
        uiManager.UpdateBetText(_playerBetAmount);

    }

    private IEnumerator ResetRound()
    {
        yield return new WaitForSeconds(2f);
        _playerValue = 0;
        _dealerValue = 0;
        _playerBetAmount = 0;
        uiManager.UpdateBetText(0);
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < _dealerHand.transform.childCount; i++)
        {
            children.Add(_dealerHand.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < _playerHand.transform.childCount; i++)
        {
            children.Add(_playerHand.transform.GetChild(i).gameObject);
        }

        foreach (GameObject child in children)
        {
            child.transform.SetParent(null);
            Destroy(child);
        }
        _playerCards.Clear();
        _dealerCards.Clear();
        UpdateHandValues();
        yield break;
    }

}
