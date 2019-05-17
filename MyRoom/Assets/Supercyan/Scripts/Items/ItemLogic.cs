using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public enum PreferredHand {
        Right,
        Left,
        Either
    }

    public bool m_useBothHands;

    public PreferredHand m_PreferredHand;

    public enum ItemType {
        AssaultRifle,
        SniperRifle,
        Pistol,

        Other,
    }

    [SerializeField] private ItemType m_itemTypeId = ItemType.Other;
    public int ItemTypeID { get { return (int)m_itemTypeId; } }

    [SerializeField] private Transform m_dummyPoint;
    public Transform DummyPoint { get { return m_dummyPoint; } }

    [SerializeField] private ItemAnimationsObject m_itemAnimations;
    public ItemAnimationsObject ItemAnimations { get { return m_itemAnimations; } }

    public virtual void OnPickup() { }

    public virtual void OnDrop() { }
}
