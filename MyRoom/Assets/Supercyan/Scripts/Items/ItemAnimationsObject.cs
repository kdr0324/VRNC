using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemAnimationsObject : ScriptableObject
{
    [Header("How the character will hold these items.")]
    [SerializeField] private AnimationClip m_holdingLeft;
    public AnimationClip HoldingLeft { get { return m_holdingLeft; } }

    [SerializeField] private AnimationClip m_holdingRight;
    public AnimationClip HoldingRight { get { return m_holdingRight; } }

    [Header("If Crouching poses are null, will use standing poses.")]
    [SerializeField] private AnimationClip m_crouchingHoldingLeft;
    public AnimationClip CrouchingHoldingLeft
    {
        get
        {
            if(m_crouchingHoldingLeft != null) { return m_crouchingHoldingLeft; }
            else { return m_holdingLeft; }
        }
    }

    [SerializeField] private AnimationClip m_crouchingHoldingRight;
    public AnimationClip CrouchingHoldingRight
    {
        get
        {
            if (m_crouchingHoldingRight != null) { return m_crouchingHoldingRight; }
            else { return m_holdingRight; }
        }
    }

    [Space(20)]
    [Header("Interaction animations with these items.")]
    [Header("Only set start animation if you don't need the animation to loop.")]
    [Space(10)]
    [Header("Standing")]
    [SerializeField] private AnimationClip m_interactionLeftStart;
    public AnimationClip InteractionLeftStart { get { return m_interactionLeftStart; } }
    [SerializeField] private AnimationClip m_interactionLeftLoop;
    public AnimationClip InteractionLeftLoop { get { return m_interactionLeftLoop; } }
    [SerializeField] private AnimationClip m_interactionLeftEnd;
    public AnimationClip InteractionLeftEnd { get { return m_interactionLeftEnd; } }
    [SerializeField] private float m_interactionLeftLoopTime = 1;
    public float InteractionLeftLoopTime { get { return m_interactionLeftLoopTime; } }

    [Space(10)]
    [SerializeField] private AnimationClip m_interactionRightStart;
    public AnimationClip InteractionRightStart { get { return m_interactionRightStart; } }
    [SerializeField] private AnimationClip m_interactionRightLoop;
    public AnimationClip InteractionRightLoop { get { return m_interactionRightLoop; } }
    [SerializeField] private AnimationClip m_interactionRightEnd;
    public AnimationClip InteractionRightEnd { get { return m_interactionRightEnd; } }
    [SerializeField] private float m_interactionRightLoopTime = 1;
    public float InteractionRightLoopTime { get { return m_interactionRightLoopTime; } }

    [Space(20)]
    [Header("Crouching")]
    [SerializeField] private bool m_useStandingInteractionAnimations;
    [SerializeField] private AnimationClip m_crouchingInteractionLeftStart;
    public AnimationClip CrouchingInteractionLeftStart
    {
        get
        {
            if (!m_useStandingInteractionAnimations) { return m_crouchingInteractionLeftStart; }
            else { return m_interactionLeftStart; }
        }
    }
    [SerializeField] private AnimationClip m_crouchingInteractionLeftLoop;
    public AnimationClip CrouchingInteractionLeftLoop
    {
        get
        {
            if (!m_useStandingInteractionAnimations) { return m_crouchingInteractionLeftLoop; }
            else { return m_interactionLeftLoop; }
        }
    }
    [SerializeField] private AnimationClip m_crouchingInteractionLeftEnd;
    public AnimationClip CrouchingInteractionLeftEnd
    {
        get
        {
            if (!m_useStandingInteractionAnimations) { return m_crouchingInteractionLeftEnd; }
            else { return m_interactionLeftEnd; }
        }
    }
    [SerializeField] private float m_crouchingInteractionLeftLoopTime = 1;
    public float CrouchingInteractionLeftLoopTime { get { return m_crouchingInteractionLeftLoopTime; } }

    [Space(10)]
    [SerializeField] private AnimationClip m_crouchingInteractionRightStart;
    public AnimationClip CrouchingInteractionRightStart
    {
        get
        {
            if (!m_useStandingInteractionAnimations) { return m_crouchingInteractionRightStart; }
            else { return m_interactionRightStart; }
        }
    }
    [SerializeField] private AnimationClip m_crouchingInteractionRightLoop;
    public AnimationClip CrouchingInteractionRightLoop
    {
        get
        {
            if (!m_useStandingInteractionAnimations) { return m_crouchingInteractionRightLoop; }
            else { return m_interactionRightLoop; }
        }
    }
    [SerializeField] private AnimationClip m_crouchingInteractionRightEnd;
    public AnimationClip CrouchingInteractionRightEnd
    {
        get
        {
            if (!m_useStandingInteractionAnimations) { return m_crouchingInteractionRightEnd; }
            else { return m_interactionRightEnd; }
        }
    }
    [SerializeField] private float m_crouchingInteractionRightLoopTime = 1;
    public float CrouchingInteractionRightLoopTime { get { return m_crouchingInteractionRightLoopTime; } }

    [Space(20)]
    [Header("Equipment animations with these items.")]
    [SerializeField] private AnimationClip m_equipLeft;
    public AnimationClip EquipLeft { get { return m_equipLeft; } }

    [SerializeField] private AnimationClip m_equipRight;
    public AnimationClip EquipRight { get { return m_equipRight; } }

    [SerializeField] private AnimationClip m_unEquipLeft;
    public AnimationClip UnEquipLeft { get { return m_unEquipLeft; } }

    [SerializeField] private AnimationClip m_unEquipRight;
    public AnimationClip UnEquipRight { get { return m_unEquipRight; } }

    [Space(20)]
    [Header("Does this item use the weapon movement animations?")]
    [SerializeField] private bool m_isWeapon;
    public bool IsWeapon { get { return m_isWeapon; } }
}