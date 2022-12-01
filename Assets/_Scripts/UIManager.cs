using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private int _playerHandValue = 0;
    private int _dealerHandValue;
    [SerializeField]private TextMeshProUGUI _playerHand;
    [SerializeField]private TextMeshProUGUI _playerBalance;
    [SerializeField]private TextMeshProUGUI _playerBets;
    [SerializeField] private TextMeshProUGUI _insufficientBalance;
    [SerializeField] private TextMeshProUGUI _playerWon;
    [SerializeField] private TextMeshProUGUI _playerLost;
    [SerializeField] private TextMeshProUGUI _playerTied;
    [SerializeField] private GameObject _dealButton;
    [SerializeField] private GameObject _standButton;
    [SerializeField] private GameObject _hitButton;
    [SerializeField] private GameObject _callButton;
    [SerializeField] private GameObject _titleContainer, _playContainer;



    private void Awake()
    {
        GameManager.DealerCardsDrawn += UpdateDealerHand;
        GameManager.PlayerCardsDrawn += UpdatePlayerHand;

    }
    void Start()
    {
        _playerHand.text = "Hand : 0";
        _playerBets.text = "Bet    : 0 $";
        _playerBalance.text = "Balance: 500 $";

    }

    public void Play()
    {
        _titleContainer.SetActive(false);
        _playContainer.SetActive(true);
        GameManager.instance.paused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }    

    public void Pause()
    {
        _titleContainer.SetActive(true);
        _playContainer.SetActive(false);
    }

    public void Stand()
    {
        var playerWin = GameManager.instance.DetermineWinner();
        FlashRoundResults(playerWin);
    }

    public void Call()
    {

    }   
    
    public void Hit()
    {
        GameManager.instance.HitCards();

    }

    public void Deal()
    {
        GameManager.instance.DistributeCards();
        _dealButton.SetActive(false);
        _standButton.SetActive(true);
        _hitButton.SetActive(true);

    }

    private void UpdatePlayerHand(int val)
    {
        _playerHandValue = val;
        _playerHand.text = $"Hand : {_playerHandValue}";
    }

    private void UpdateDealerHand(int val)
    {
        _dealerHandValue = val;
    }

    public void UpdateBalanceText(int balance)
    {
        _playerBalance.text = $"Balance : {balance}";

    }

    public void InsufficientBalance()
    {
        _insufficientBalance.enabled = true;
        StartCoroutine(flashInsufficientBalance());
    }

    public void UpdateBetText(int bet)
    {
        _playerBets.text = $"Bet : {bet}";

    }

    public void FlashRoundResults(int result)
    {
        if (result == 1)
        {
            _playerWon.enabled = true;
        }
        else if (result == 0)
        {
            _playerLost.enabled = true;
        }
        else
        {
            _playerTied.enabled = true;
        }
        StartCoroutine(flashRoundResult());

    }

    IEnumerator flashInsufficientBalance()
    {
        yield return new WaitForSeconds(2f);
        _insufficientBalance.enabled = false;
        yield break;
    }

    IEnumerator flashRoundResult()
    {
        yield return new WaitForSeconds(2f);
        _playerWon.enabled = false;
        _playerLost.enabled = false;
        _playerTied.enabled = false;
        _dealButton.SetActive(true);
        _standButton.SetActive(false);
        _hitButton.SetActive(false);
        yield break;
    }
}
