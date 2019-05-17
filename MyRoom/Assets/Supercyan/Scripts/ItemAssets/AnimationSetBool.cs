using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSetBool : StateMachineBehaviour {

    [SerializeField] private string m_boolName;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(m_boolName, false);
    }
}
