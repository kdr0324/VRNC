#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemObject : ScriptableObject
{
    [SerializeField] ItemLogic m_itemPrefab;

    public ItemLogic Item { get { return m_itemPrefab; } }
}

#endif