﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : BaseBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.DroneLogic.SignalLight.DronMode = enumDronMode.Attack;

        this.DroneLogic.StartAttackMode();

        GameObject.FindGameObjectWithTag(Resources.Tags.CommandAttack).GetComponent<UnityEngine.UI.Text>().color = Color.white;

        this.NavAgent.updateRotation = false;
        this.NavAgent.speed = this.DroneLogic.AttackSpeed;
        this.NavAgent.angularSpeed = this.DroneLogic.AttackAngularSpeed;
        this.NavAgent.acceleration = this.DroneLogic.AttackAcceleration;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.DroneLogic.SwitchTarget();

        if (Time.frameCount % 10 == 0 && this.DestinationReached())
        {
            if (this.DroneLogic.CurrentTarget == null)
            {
                Vector3 destination = this.GetRandomDestination(this.PlayerTransform.position, this.DroneLogic.MaxDistance, NavMesh.AllAreas);
                this.NavAgent.SetDestination(destination);
            }
            else
            {
                base.GetRandomDestination(this.DroneLogic.CurrentTarget.transform.position, 10f, NavMesh.AllAreas);
                this.NavAgent.SetDestination(this.DroneLogic.CurrentTarget.transform.position);
            }
        }

        if (this.DroneLogic.CurrentTarget != null)
        {
            Vector3 target = this.DroneLogic.CurrentTarget.transform.position;
            target.y += 2f;

#if UNITY_EDITOR
            Debug.DrawLine(this.DroneTransform.position, target);
#endif

            Vector3 direction = target - this.DroneTransform.position;

            // look at target
            this.DroneTransform.rotation = Quaternion.Slerp(this.DroneTransform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.DroneLogic.AttackAngularSpeed);

            // shoot if target is visible
            if (this.CanSeeTarget(target))
            {
                this.ShootingLogic.Shoot(this.DroneTransform.position, Quaternion.LookRotation(direction));
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.NavAgent.updateRotation = true;

        GameObject.FindGameObjectWithTag(Resources.Tags.CommandAttack).GetComponent<UnityEngine.UI.Text>().color = Color.black;

        // used to manage drone battery
        this.DroneLogic.EndAttackMode();
    }

    private bool CanSeeTarget(Vector3 target)
    {
        Vector3 direction = target - this.DroneTransform.position;
        if (Vector3.Angle(direction, this.DroneTransform.forward) < this.DroneLogic.MaxAttackAngle)
        {
            return Physics.Linecast(this.DroneTransform.position, target, LayerMask.GetMask(Resources.Layers.Buildings)) == false;
        }

        return false;

        // check distance to target?
    }
}
