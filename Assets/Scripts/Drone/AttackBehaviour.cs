using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : BaseBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.NavAgent.speed = this.DroneLogic.MaxSpeed;
        this.NavAgent.baseOffset = this.DroneLogic.Height * 2;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.DroneLogic.CurrentTarget == null) return;

        if (Time.frameCount % 20 == 0)
        {
            this.NavAgent.SetDestination(this.DroneLogic.CurrentTarget.transform.position);
        }

        Debug.DrawLine(this.DroneTransform.position, this.DroneLogic.CurrentTarget.transform.position);

        Vector3 direction = this.DroneLogic.CurrentTarget.transform.position - this.DroneTransform.position;
        this.DroneLogic.Shoot(Quaternion.LookRotation(direction));

        // fire on enemy until he is dead
    }
}
