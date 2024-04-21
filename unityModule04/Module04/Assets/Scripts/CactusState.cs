using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusState : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent<Cactus>(out Cactus cac))
		{
			cac.Fire();
		}
    }
}