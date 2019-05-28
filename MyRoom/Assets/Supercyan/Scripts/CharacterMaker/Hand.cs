using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public enum HandSide
    {
        Left,
        Right
    }

    [SerializeField] private HandSide m_side;

    public HandSide GetHandSide { get { return m_side; } }
}
