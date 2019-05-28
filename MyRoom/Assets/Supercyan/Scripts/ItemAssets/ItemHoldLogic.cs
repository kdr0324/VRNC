using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHoldLogic : MonoBehaviour, IInitializable
{
    public void Initialize(GameObject character)
    {
        if (m_croucher == null) { m_croucher = character.GetComponent<Croucher>(); }
        if (m_aimScript == null) { m_aimScript = character.GetComponent<CharacterWeaponAnimator>(); }
        if (m_itemScriptR == null || m_itemScriptL == null)
        {
            CharacterItemAnimator[] animators = character.GetComponents<CharacterItemAnimator>();
            foreach (CharacterItemAnimator a in animators)
            {
                if (a.ThisHand == CharacterItemAnimator.Hand.Right) { m_itemScriptR = a; }
                else if (a.ThisHand == CharacterItemAnimator.Hand.Left) { m_itemScriptL = a; }
            }
        }

        Hand[] hands = character.GetComponentsInChildren<Hand>();

        for (int j = 0; j < hands.Length; j++)
        {
            switch (hands[j].GetHandSide)
            {
                case Hand.HandSide.Left:
                    m_handBoneL = hands[j].transform;
                    break;

                case Hand.HandSide.Right:
                    m_handBoneR = hands[j].transform;
                    break;

                default:
                    break;
            }
        }

        Animator animator = GetComponent<Animator>();
        m_animator = new AnimatorOverrideController(animator.runtimeAnimatorController);
        m_animator.name = animator.runtimeAnimatorController.name;
        animator.runtimeAnimatorController = m_animator;
    }

    private void Awake() {
        Initialize(gameObject);
    }

    [SerializeField] private Croucher m_croucher;

    [SerializeField] private CharacterWeaponAnimator m_aimScript;
    [SerializeField] private CharacterItemAnimator m_itemScriptL;
    [SerializeField] private CharacterItemAnimator m_itemScriptR;

    [SerializeField] private Transform m_handBoneL;
    [SerializeField] private Transform m_handBoneR;

    private AnimatorOverrideController m_animator;

    public CharacterWeaponAnimator AimScript { set { m_aimScript = value; } }
    public Transform HandBoneR
    {
        get { return m_handBoneR; }
        set { m_handBoneR = value; }
    }
    public Transform HandBoneL
    {
        get { return m_handBoneL; }
        set { m_handBoneL = value; }
    }

    public ItemLogic m_itemInHandL;
    public ItemLogic m_itemInHandR;

    private bool m_itemUsesBothHands = false;
    private bool m_isHoldingWeapon = false;

    void Start()
    {
        if (!m_handBoneR || !m_handBoneL)
        {
            Debug.LogError("Handbones not set. Can't hold items.");
            return;
        }

        if(m_itemInHandR == null && m_itemInHandL == null)
        {
            if (m_aimScript) { m_aimScript.SetGunInHand(false, -1); }
        }
        
        CheckHands();
        if (m_itemInHandR) { AttachItem(m_itemInHandR, CharacterItemAnimator.Hand.Right); }
        if (m_itemInHandL) { AttachItem(m_itemInHandL, CharacterItemAnimator.Hand.Left); }
    }

    private void CheckHands()
    {
        ItemLogic right = m_itemInHandR;
        ItemLogic left = m_itemInHandL;
        ItemLogic either = null;

        if (right)
        {
            if (right.m_useBothHands && m_itemInHandL)
            {
                Drop(CharacterItemAnimator.Hand.Left);
                left = null;
            }

            if (right.m_PreferredHand == ItemLogic.PreferredHand.Right) { m_itemInHandR = right; }
            else if (right.m_PreferredHand == ItemLogic.PreferredHand.Left)
            {
                m_itemInHandL = right;
                m_itemInHandR = null;
            }
            else if (right.m_PreferredHand == ItemLogic.PreferredHand.Either)
            {
                m_itemInHandR = right;
                either = right;
            }
        }

        if (left)
        {
            if(left.m_useBothHands)
            {
                if (m_itemInHandR) { Drop(CharacterItemAnimator.Hand.Right); }
                if (m_itemInHandL) { Drop(CharacterItemAnimator.Hand.Left); }
                either = null;
            }

            if (left.m_PreferredHand == ItemLogic.PreferredHand.Left) { m_itemInHandL = left; }
            else if (left.m_PreferredHand == ItemLogic.PreferredHand.Right)
            {
                if (m_itemInHandR == null) { m_itemInHandR = left; }
                else { Drop(CharacterItemAnimator.Hand.Left); }

                if (either != null)
                {
                    m_itemInHandR = left;
                    m_itemInHandL = either;
                }
            }
            else if (left.m_PreferredHand == ItemLogic.PreferredHand.Either)
            {
                if (m_itemInHandL != null && m_itemInHandL != left) { m_itemInHandR = left; }
                else { m_itemInHandL = left; }
            }
        }
    }

    public void AttachItem(ItemLogic item, CharacterItemAnimator.Hand handToAttach)
    {
        if (item.m_PreferredHand == ItemLogic.PreferredHand.Left) { handToAttach = CharacterItemAnimator.Hand.Left; }
        else if (item.m_PreferredHand == ItemLogic.PreferredHand.Right) { handToAttach = CharacterItemAnimator.Hand.Right; }

        if (m_itemUsesBothHands || item.m_useBothHands)
        {
            if (m_itemInHandL && m_itemInHandL != item) { Drop(CharacterItemAnimator.Hand.Left); }
            if (m_itemInHandR && m_itemInHandR != item) { Drop(CharacterItemAnimator.Hand.Right); }
        }

        if (item == m_itemInHandL && handToAttach == CharacterItemAnimator.Hand.Left) { m_itemInHandL = item; }
        else if (item == m_itemInHandR && handToAttach == CharacterItemAnimator.Hand.Right) { m_itemInHandR = item; }
        else if (m_itemInHandL == null && handToAttach == CharacterItemAnimator.Hand.Left) { m_itemInHandL = item; }
        else if (m_itemInHandR == null && handToAttach == CharacterItemAnimator.Hand.Right) { m_itemInHandR = item; }
        else if (item.m_PreferredHand == ItemLogic.PreferredHand.Right)
        {
            if (m_itemInHandR) { Drop(CharacterItemAnimator.Hand.Right); }
            m_itemInHandR = item;
        }
        else if (item.m_PreferredHand == ItemLogic.PreferredHand.Left ||
                 item.m_PreferredHand == ItemLogic.PreferredHand.Either)
        {
            if (m_itemInHandL) { Drop(CharacterItemAnimator.Hand.Left); }
            m_itemInHandL = item;
        }
        CheckHands();

        Transform dummyPoint = null;
        if (item.DummyPoint != null) { dummyPoint = item.DummyPoint; }

        bool isWeapon = false;
        if (item.ItemAnimations) { isWeapon = item.ItemAnimations.IsWeapon; }

        m_itemUsesBothHands = item.m_useBothHands;
        if (m_itemScriptR != null && item == m_itemInHandR) { m_itemScriptR.UseBothHands = m_itemUsesBothHands; }
        else if (m_itemScriptL != null && item == m_itemInHandL) { m_itemScriptL.UseBothHands = m_itemUsesBothHands; }

        if(item == m_itemInHandR) { Attach(item, m_handBoneR, dummyPoint); }
        else if(item == m_itemInHandL) { Attach(item, m_handBoneL, dummyPoint); }
        
        if (m_aimScript)
        {
            if (item.ItemTypeID <= 2) { m_aimScript.SetGunInHand(true, item.ItemTypeID); }
            else { m_aimScript.SetGunInHand(false, -1); }
        }

        if (m_itemScriptL || m_itemScriptR) { SetCorrectAnimations(); }
    }

    private void Attach(ItemLogic item, Transform hand, Transform dummyPoint)
    {
        item.transform.parent = hand;

        item.transform.localScale = new Vector3(
            Mathf.Abs(item.transform.localScale.x),
            Mathf.Abs(item.transform.localScale.y),
            Mathf.Abs(item.transform.localScale.z)
            );
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (m_itemScriptL != null && hand == HandBoneL && item.ItemAnimations) { m_isHoldingWeapon = item.ItemAnimations.IsWeapon; }
        if (m_itemScriptR != null && hand == HandBoneR && item.ItemAnimations) { if (!m_isHoldingWeapon) { m_isHoldingWeapon = item.ItemAnimations.IsWeapon; } }

        if (m_itemScriptL != null && hand == HandBoneL) { m_itemScriptL.SetHolding(true, m_isHoldingWeapon); }
        if (m_itemScriptR != null && hand == HandBoneR) { m_itemScriptR.SetHolding(true, m_isHoldingWeapon); }

        if (dummyPoint != null)
        {
            item.transform.position = dummyPoint.position;
            item.transform.localRotation *= dummyPoint.localRotation;
        }

        if (hand == m_handBoneR) { m_itemInHandR = item; }
        else if(hand == m_handBoneL) { m_itemInHandL = item; }

        item.OnPickup();
    }

    public void Drop(CharacterItemAnimator.Hand hand)
    {
        if (hand == CharacterItemAnimator.Hand.Right)
        {
            ItemLogic itemLogic = m_itemInHandR.GetComponent<ItemLogic>();
            m_itemInHandR.transform.parent = null;
            m_itemInHandR = null;
            if (itemLogic) { itemLogic.OnDrop(); }
        }
        else if (hand == CharacterItemAnimator.Hand.Left)
        {
            ItemLogic itemLogic = m_itemInHandL.GetComponent<ItemLogic>();
            m_itemInHandL.transform.parent = null;
            m_itemInHandL = null;
            if (itemLogic) { itemLogic.OnDrop(); }
        }
    }

    public void Toggle(CharacterItemAnimator.Hand hand)
    {
        if (hand == CharacterItemAnimator.Hand.Right)
        {
            if (m_itemInHandR.gameObject.activeSelf) { m_itemInHandR.gameObject.SetActive(false); }
            else
            {
                ItemLogic item = m_itemInHandR;
                item.gameObject.SetActive(true);
                m_itemInHandR = null;
                AttachItem(item, hand);
            }
        }
        else if (hand == CharacterItemAnimator.Hand.Left)
        {
            if (m_itemInHandL.gameObject.activeSelf) { m_itemInHandL.gameObject.SetActive(false); }
            else
            {
                ItemLogic item = m_itemInHandL;
                item.gameObject.SetActive(true);
                m_itemInHandL = null;
                AttachItem(item, hand);
            }
        }
    }

    private List<KeyValuePair<AnimationClip, AnimationClip>> m_overrides;

    private void SetCorrectAnimations()
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides;
        overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(m_animator.overridesCount);
        m_animator.GetOverrides(overrides);

        m_overrides = overrides;

        if (m_itemInHandR)
        {
            ItemLogic item = m_itemInHandR.GetComponent<ItemLogic>();
            if (item.ItemAnimations)
            {
                if(item.ItemAnimations.InteractionRightLoop != null)
                {
                    m_itemScriptR.LoopingAnimationStanding = true;
                    if(item.ItemAnimations.InteractionRightStart != null)
                    {
                        m_itemScriptR.InteractionStandingStartDuration = item.ItemAnimations.InteractionRightStart.length;
                        m_itemScriptR.LoopingDuration = item.ItemAnimations.InteractionRightLoopTime;
                    }
                }
                else { m_itemScriptR.LoopingAnimationStanding = false; }

                if (item.ItemAnimations.CrouchingInteractionRightLoop != null)
                {
                    m_itemScriptR.LoopingAnimationCrouching = true;
                    if (item.ItemAnimations.CrouchingInteractionRightStart != null)
                    {
                        m_itemScriptR.InteractionCrouchingStartDuration = item.ItemAnimations.CrouchingInteractionRightStart.length;
                        m_itemScriptR.LoopingDuration = item.ItemAnimations.CrouchingInteractionRightLoopTime;
                    }
                }
                else { m_itemScriptR.LoopingAnimationCrouching = false; }

                ReplaceAnimation("_ItemInteractionRStart", item.ItemAnimations.InteractionRightStart);
                ReplaceAnimation("_ItemInteractionRLoop", item.ItemAnimations.InteractionRightLoop);
                ReplaceAnimation("_ItemInteractionREnd", item.ItemAnimations.InteractionRightEnd);

                ReplaceAnimation("_ItemInteractionCrouchingRStart", item.ItemAnimations.CrouchingInteractionRightStart);
                ReplaceAnimation("_ItemInteractionCrouchingRLoop", item.ItemAnimations.CrouchingInteractionRightLoop);
                ReplaceAnimation("_ItemInteractionCrouchingREnd", item.ItemAnimations.CrouchingInteractionRightEnd);

                ReplaceAnimation("_ItemHoldR", item.ItemAnimations.HoldingRight);
                ReplaceAnimation("_ItemHoldCrouchingR", item.ItemAnimations.CrouchingHoldingRight);
                ReplaceAnimation("_ItemEquipR", item.ItemAnimations.EquipRight);
                ReplaceAnimation("_ItemUnEquipR", item.ItemAnimations.UnEquipRight);

                if (item.m_useBothHands)
                {
                    ReplaceAnimation("_ItemInteractionLStart", item.ItemAnimations.InteractionRightStart);
                    ReplaceAnimation("_ItemInteractionLLoop", item.ItemAnimations.InteractionRightLoop);
                    ReplaceAnimation("_ItemInteractionLEnd", item.ItemAnimations.InteractionRightEnd);

                    ReplaceAnimation("_ItemInteractionCrouchingLStart", item.ItemAnimations.CrouchingInteractionRightStart);
                    ReplaceAnimation("_ItemInteractionCrouchingLLoop", item.ItemAnimations.CrouchingInteractionRightLoop);
                    ReplaceAnimation("_ItemInteractionCrouchingLEnd", item.ItemAnimations.CrouchingInteractionRightEnd);

                    ReplaceAnimation("_ItemHoldB", item.ItemAnimations.HoldingRight);
                    ReplaceAnimation("_ItemHoldCrouchingB", item.ItemAnimations.CrouchingHoldingRight);
                    ReplaceAnimation("_ItemEquipB", item.ItemAnimations.EquipRight);
                    ReplaceAnimation("_ItemUnEquipB", item.ItemAnimations.UnEquipRight);
                }
            }
            else
            {
                ReplaceAnimation("_ItemInteractionRStart", null);
                ReplaceAnimation("_ItemInteractionRLoop", null);
                ReplaceAnimation("_ItemInteractionREnd", null);

                ReplaceAnimation("_ItemInteractionCrouchingRStart", null);
                ReplaceAnimation("_ItemInteractionCrouchingRLoop", null);
                ReplaceAnimation("_ItemInteractionCrouchingREnd", null);

                ReplaceAnimation("_ItemHoldR", null);
                ReplaceAnimation("_ItemHoldCrouchingR", null);
                ReplaceAnimation("_ItemEquipR", null);
                ReplaceAnimation("_ItemUnEquipR", null);
            }
        }

        if (m_itemInHandL)
        {
            ItemLogic item = m_itemInHandL.GetComponent<ItemLogic>();
            if (item.ItemAnimations)
            {
                if (item.ItemAnimations.InteractionLeftLoop != null)
                {
                    m_itemScriptL.LoopingAnimationStanding = true;
                    if (item.ItemAnimations.InteractionLeftStart != null)
                    {
                        m_itemScriptL.InteractionStandingStartDuration = item.ItemAnimations.InteractionLeftStart.length;
                        m_itemScriptL.LoopingDuration = item.ItemAnimations.InteractionLeftLoopTime;
                    }
                }
                else { m_itemScriptL.LoopingAnimationStanding = false; }

                if (item.ItemAnimations.CrouchingInteractionLeftLoop != null)
                {
                    m_itemScriptL.LoopingAnimationCrouching = true;
                    if (item.ItemAnimations.CrouchingInteractionLeftStart != null)
                    {
                        m_itemScriptL.InteractionCrouchingStartDuration = item.ItemAnimations.CrouchingInteractionLeftStart.length;
                        m_itemScriptL.LoopingDuration = item.ItemAnimations.CrouchingInteractionLeftLoopTime;
                    }
                }
                else { m_itemScriptL.LoopingAnimationCrouching = false; }

                ReplaceAnimation("_ItemInteractionLStart", item.ItemAnimations.InteractionLeftStart);
                ReplaceAnimation("_ItemInteractionLLoop", item.ItemAnimations.InteractionLeftLoop);
                ReplaceAnimation("_ItemInteractionLEnd", item.ItemAnimations.InteractionLeftEnd);

                ReplaceAnimation("_ItemInteractionCrouchingLStart", item.ItemAnimations.CrouchingInteractionLeftStart);
                ReplaceAnimation("_ItemInteractionCrouchingLLoop", item.ItemAnimations.CrouchingInteractionLeftLoop);
                ReplaceAnimation("_ItemInteractionCrouchingLEnd", item.ItemAnimations.CrouchingInteractionLeftEnd);

                ReplaceAnimation("_ItemHoldL", item.ItemAnimations.HoldingLeft);
                ReplaceAnimation("_ItemHoldCrouchingL", item.ItemAnimations.CrouchingHoldingLeft);
                ReplaceAnimation("_ItemEquipL", item.ItemAnimations.EquipLeft);
                ReplaceAnimation("_ItemUnEquipL", item.ItemAnimations.UnEquipLeft);

                if (item.m_useBothHands)
                {
                    ReplaceAnimation("_ItemInteractionRStart", item.ItemAnimations.InteractionLeftStart);
                    ReplaceAnimation("_ItemInteractionRLoop", item.ItemAnimations.InteractionLeftLoop);
                    ReplaceAnimation("_ItemInteractionREnd", item.ItemAnimations.InteractionLeftEnd);

                    ReplaceAnimation("_ItemInteractionCrouchingRStart", item.ItemAnimations.CrouchingInteractionLeftStart);
                    ReplaceAnimation("_ItemInteractionCrouchingRLoop", item.ItemAnimations.CrouchingInteractionLeftLoop);
                    ReplaceAnimation("_ItemInteractionCrouchingREnd", item.ItemAnimations.CrouchingInteractionLeftEnd);

                    ReplaceAnimation("_ItemHoldB", item.ItemAnimations.HoldingLeft);
                    ReplaceAnimation("_ItemHoldCrouchingB", item.ItemAnimations.CrouchingHoldingLeft);
                    ReplaceAnimation("_ItemEquipB", item.ItemAnimations.EquipLeft);
                    ReplaceAnimation("_ItemUnEquipB", item.ItemAnimations.UnEquipLeft);
                }
            }
            else
            {
                ReplaceAnimation("_ItemInteractionLStart", null);
                ReplaceAnimation("_ItemInteractionLLoop", null);
                ReplaceAnimation("_ItemInteractionLEnd", null);

                ReplaceAnimation("_ItemInteractionCrouchingLStart", null);
                ReplaceAnimation("_ItemInteractionCrouchingLLoop", null);
                ReplaceAnimation("_ItemInteractionCrouchingLEnd", null);

                ReplaceAnimation("_ItemHoldL", null);
                ReplaceAnimation("_ItemHoldCrouchingL", null);
                ReplaceAnimation("_ItemEquipR", null);
                ReplaceAnimation("_ItemUnEquipR", null);
            }
        }

        List<KeyValuePair<AnimationClip, AnimationClip>> newOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        for (int i=0; i<m_overrides.Count; i++)
        {
            if(m_overrides[i].Value == overrides[i].Value) { newOverrides.Add(m_overrides[i]); }
        }
        m_animator.ApplyOverrides(newOverrides);
    }

    private void ReplaceAnimation(string oldAnimation, AnimationClip newAnimation)
    {
        for (int i = 0; i < m_overrides.Count; ++i)
        {
            if (m_overrides[i].Key.name == oldAnimation)
            {
                m_overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(m_overrides[i].Key, newAnimation);
                return;
            }
        }
        Debug.LogWarningFormat("Didn't find {0} in the animator", oldAnimation);
    }
}
