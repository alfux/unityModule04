using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<PlayerController>().ReverseOrientation();
        animator.transform.localScale = new Vector3(
            -animator.transform.localScale.x,
            animator.transform.localScale.y,
            animator.transform.localScale.z
        );
        animator.gameObject.GetComponent<PlayerController>().UnsetTurn();
    }
}
