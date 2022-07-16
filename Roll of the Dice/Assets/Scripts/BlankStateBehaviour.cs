using UnityEngine;

public class BlankStateBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        animator.SetBool("BlankOver", true);
    }
}