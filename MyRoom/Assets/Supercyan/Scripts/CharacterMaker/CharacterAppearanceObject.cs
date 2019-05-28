#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterAppearanceObject : ScriptableObject
{
    [SerializeField] GameObject m_model;

    public GameObject Model { get { return m_model; } }
}

#endif