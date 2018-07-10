using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : BaseBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.DroneLogic.SignalLight.DronMode = enumDronMode.Attack;

        GameObject.FindGameObjectWithTag(Resources.Tags.CommandAttack).GetComponent<UnityEngine.UI.Text>().color = Color.white;

        this.NavAgent.updateRotation = false;
        this.NavAgent.speed = this.DroneLogic.AttackSpeed;
        this.NavAgent.angularSpeed = this.DroneLogic.AttackAngularSpeed;
        this.NavAgent.acceleration = this.DroneLogic.AttackAcceleration;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.DroneLogic.SwitchTarget();

        if (Time.frameCount % 10 == 0)
        {
            if (this.DroneLogic.CurrentTarget == null)
            {
                if (this.DestinationReached())
                {
                    Vector3 destination = this.GetRandomDestination(this.PlayerTransform.position, this.DroneLogic.MaxDistance, NavMesh.AllAreas);
                    this.NavAgent.SetDestination(destination);
                }
            }
            else
            {
                if (this.DestinationReached())
                {
                    base.GetRandomDestination(this.DroneLogic.CurrentTarget.transform.position, 10f, NavMesh.AllAreas);
                    this.NavAgent.SetDestination(this.DroneLogic.CurrentTarget.transform.position);
                }
            }
        }

        if (this.DroneLogic.CurrentTarget != null)
        {
            Vector3 target = this.DroneLogic.CurrentTarget.transform.position;
            target.y += 2f;

            Debug.DrawLine(this.DroneTransform.position, target);


            Vector3 direction = target - this.DroneTransform.position;

            // look at target
            this.DroneTransform.rotation = Quaternion.Slerp(this.DroneTransform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.DroneLogic.AttackAngularSpeed);

            // shoot at target
            if (this.CanSeeTarget(target))
            {
                this.ShootingLogic.Shoot(this.DroneTransform.position, Quaternion.LookRotation(direction));
            }
        }

        //if (this.CanSeeTarget(target))
        //{
        //    Vector3 direction = target - this.DroneTransform.position;
        //    this.ShootingLogic.Shoot(this.DroneTransform.position, Quaternion.LookRotation(direction));
        //}
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.NavAgent.updateRotation = true;

        GameObject.FindGameObjectWithTag(Resources.Tags.CommandAttack).GetComponent<UnityEngine.UI.Text>().color = Color.black;
    }

    private bool CanSeeTarget(Vector3 target)
    {
        Vector3 direction = target - this.DroneTransform.position;
        if (Vector3.Angle(direction, this.DroneTransform.forward) < this.DroneLogic.MaxAttackAngle)
        {
            return Physics.Linecast(this.DroneTransform.position, target, LayerMask.GetMask(Resources.Layers.Buildings)) == false;
        }

        return false;

        //if (Physics.Linecast(this.DroneTransform.position, target, LayerMask.GetMask(Resources.Layers.Buildings)) == false)
        //{
        //    // if there is no buildings between the drone and the target
        //    Vector3 direction = target - this.DroneTransform.position;
        //    if (Vector3.Angle(direction, this.DroneTransform.forward) < this.DroneLogic.MaxAttackAngle)
        //    {
        //        return true;
        //    }
        //}

        //return false;
    }
}
