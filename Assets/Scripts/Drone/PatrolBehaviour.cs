using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : BaseBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.DroneLogic.SignalLight.DronMode = enumDronMode.Patrol;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.frameCount % 20 == 0)
        {
            if (this.IsCloseToPlayer())
            {
                this.NavAgent.speed = this.DroneLogic.PatrolSpeed;
                this.NavAgent.angularSpeed = this.DroneLogic.PatrolAngularSpeed;
                this.NavAgent.acceleration = this.DroneLogic.PatrolAcceleration;

                if (this.DestinationReached())
                {
                    Vector3 destination = this.GetRandomDestination(this.PlayerTransform.position, this.DroneLogic.MaxDistance, NavMesh.AllAreas);
                    this.NavAgent.SetDestination(destination);
                }
            }
            else
            {
                this.NavAgent.speed = this.DroneLogic.AttackSpeed;
                this.NavAgent.angularSpeed = this.DroneLogic.AttackAngularSpeed;
                this.NavAgent.acceleration = this.DroneLogic.AttackAcceleration;
                this.NavAgent.SetDestination(this.PlayerTransform.position + this.PlayerTransform.forward * 10f);
            }
        }
    }
}
