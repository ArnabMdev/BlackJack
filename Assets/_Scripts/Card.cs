using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cardSuite
{
    Heart = 0,
    Diamond = 1,
    Spade = 2,
    Club = 3,
    flipped = 4
}

public class Card : MonoBehaviour
{
    public int cardID;
    public int cardValue;
    public cardSuite suite;
    
}
