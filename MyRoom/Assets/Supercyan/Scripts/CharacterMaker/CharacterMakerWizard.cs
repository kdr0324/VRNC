#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterMakerWizard : ScriptableWizard
{
    public CharacterAppearanceObject m_appearanceObject;
    public CharacterBehaviorObject m_behaviorObject;

    public ItemObject m_itemLeftHand;
    public ItemObject m_itemRightHand;

    [MenuItem("Supercyan/Character Maker")]
    static void CreateWizard()
    {
        DisplayWizard<CharacterMakerWizard>("Character Maker");
    }

    void OnWizardCreate()
    {
        GameObject character = Instantiate(m_appearanceObject.Model);

        CapsuleCollider collider = character.AddComponent<CapsuleCollider>();
        collider.radius = 0.1f;
        collider.direction = 1;
        collider.center = new Vector3(0.0f, 0.5f, 0.0f);

        Rigidbody rigidbody = character.AddComponent<Rigidbody>();
        rigidbody.angularDrag = 5.0f;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        m_behaviorObject.InitializeBehaviour(character);

        character.transform.position = Vector3.zero;

        ItemHoldLogic itemHoldLogic = character.GetComponent<ItemHoldLogic>();

        if ((itemHoldLogic == null) &&
            (m_itemLeftHand || m_itemRightHand))
        {
            itemHoldLogic = character.AddComponent<ItemHoldLogic>();
        }

        if(itemHoldLogic)
        {
            CharacterItemAnimator itemAnimator;
            itemAnimator = itemHoldLogic.gameObject.AddComponent<CharacterItemAnimator>();
            itemAnimator.ThisHand = CharacterItemAnimator.Hand.Left;

            itemAnimator = itemHoldLogic.gameObject.AddComponent<CharacterItemAnimator>();
            itemAnimator.ThisHand = CharacterItemAnimator.Hand.Right;

            if (m_itemLeftHand)
            {
                ItemLogic item = Instantiate(m_itemLeftHand.Item);
                item.transform.parent = itemHoldLogic.HandBoneL;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                itemHoldLogic.m_itemInHandL = item;
            }

            if (m_itemRightHand)
            {
                ItemLogic item = Instantiate(m_itemRightHand.Item);
                item.transform.parent = itemHoldLogic.HandBoneR;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                itemHoldLogic.m_itemInHandR = item;
            }
        }
    }
}
#endif