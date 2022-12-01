using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private List<GameObject> _cardsInPlay = new List<GameObject>();
    private List<GameObject> _cardsOutPlay = new List<GameObject>();    
    private Dictionary<GameObject, int> _cardsIdx = new Dictionary<GameObject, int>();
    private Sprite _flippedSprite,_dealerSprite1, _dealerSprite2;
    [SerializeField] private GameObject _flippedCard;
    [SerializeField] private List<GameObject> _spadeCards = new List<GameObject>(); 
    [SerializeField] private List<GameObject> _heartCards = new List<GameObject>(); 
    [SerializeField] private List<GameObject> _clubCards = new List<GameObject>(); 
    [SerializeField] private List<GameObject> _diamondCards = new List<GameObject>();
    


    public void InitializeCards()
    {
        foreach (var cards in _heartCards)
        {
            _cardsOutPlay.Add(cards);
        }

        foreach (var cards in _spadeCards)
        {
            _cardsOutPlay.Add(cards);
        }

        foreach (var cards in _clubCards)
        {
            _cardsOutPlay.Add(cards);
        }

        foreach (var cards in _diamondCards)
        {
            _cardsOutPlay.Add(cards);
        }

        _flippedSprite = _flippedCard.GetComponent<SpriteRenderer>().sprite;
        _dealerSprite1 = null;
        _dealerSprite2 = null;

    }

    public GameObject DealCard()
    {
        int index = Random.Range(0, _cardsOutPlay.Count);
        GameObject card = Instantiate(_cardsOutPlay[index]);
        _cardsInPlay.Add(_cardsOutPlay[index]);
        _cardsIdx[card] = _cardsInPlay.Count - 1;
        _cardsOutPlay.RemoveAt(index);
        return card;
    }


    public void DiscardCard(GameObject card)
    {
        _cardsOutPlay.Add(_cardsInPlay[_cardsIdx[card]]);
        _cardsInPlay.RemoveAt(_cardsIdx[card]);
        Destroy(card);
    }

    public void FlipCard(GameObject card)
    {
        var spR = card.GetComponent<SpriteRenderer>();
        if(spR.sprite == _flippedSprite)
        {
            if (_dealerSprite1 != null)
            {
                spR.sprite = _dealerSprite1;
                _dealerSprite1 = null;
            }
            else
            {
                spR.sprite = _dealerSprite2;
                _dealerSprite2 = null;

            }
        }
        else
        { 
            if (_dealerSprite1 == null)
            {
                _dealerSprite1 = spR.sprite;
                spR.sprite = _flippedSprite;
            }
            else
            {
                _dealerSprite2 = spR.sprite;
                spR.sprite = _flippedSprite;
            
            }
        }
            
        
    }


}
