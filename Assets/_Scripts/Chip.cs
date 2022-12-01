using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] private int _chipValue;
    public static event Action<int> OnChipClicked;

    private void OnMouseDown()
    {
        OnChipClicked?.Invoke(_chipValue);
    }
}
