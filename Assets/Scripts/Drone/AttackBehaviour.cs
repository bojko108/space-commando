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

        this.NavAgent.speed = this.DroneLogic.AttackSpeed;
        this.NavAgent.angularSpeed = this.DroneLogic.AttackAngularSpeed;
        this.NavAgent.acceleration = this.DroneLogic.AttackAcceleration;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.DroneLogic.SwitchTarget();

        if (this.DroneLogic.CurrentTarget == null)
        {
            // no other targets so go to patrol mode....
            this.DroneLogic.SetInPatrolMode();
            return;
        }

        if (Time.frameCount % 10 == 0)
        {
            if (this.DestinationReached())
            {
                base.GetRandomDestination(this.DroneLogic.CurrentTarget.transform.position, 20f, NavMesh.AllAreas);
                this.NavAgent.SetDestination(this.DroneLogic.CurrentTarget.transform.position);
            }
        }

        Vector3 target = this.DroneLogic.CurrentTarget.transform.position;
        target.y += 2f;

        Debug.DrawLine(this.DroneTransform.position, target);

        if (this.CanSeeTarget(target))
        {
            Vector3 direction = target - this.DroneTransform.position;
            this.ShootingLogic.Shoot(this.DroneTransform.position, Quaternion.LookRotation(direction));
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.FindGameObjectWithTag(Resources.Tags.CommandAttack).GetComponent<UnityEngine.UI.Text>().color = Color.black;
    }

    private bool CanSeeTarget(Vector3 target)
    {
        //if (Physics.Linecast(this.DroneTransform.position, target) == false)
        //{
        Vector3 direction = target - this.DroneTransform.position;
        if (Vector3.Angle(direction, this.DroneTransform.forward) < this.DroneLogic.MaxAttackAngle)
        {
            return true;
        }
        //}

        return false;
    }
}
