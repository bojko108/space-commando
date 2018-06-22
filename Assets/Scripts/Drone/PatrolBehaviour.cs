using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : BaseBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.NavAgent.baseOffset = this.DroneLogic.Height;
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.frameCount % 20 == 0)
        {
            if (this.IsCloseToPlayer())
            {
                this.NavAgent.speed = this.DroneLogic.PatrolSpeed;
                if (this.DestinationReached())
                {
                    Vector3 destination = this.GetRandomDestination(this.PlayerTransform.position, this.DroneLogic.MaxDistance, NavMesh.AllAreas);
                    this.NavAgent.SetDestination(destination);
                }
            }
            else
            {
                this.NavAgent.speed = this.DroneLogic.MaxSpeed;
                this.NavAgent.SetDestination(this.PlayerTransform.position + this.PlayerTransform.forward * 5f);
            }
        }
    }
}
