using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseBehaviour : StateMachineBehaviour
{
    [HideInInspector]
    public GameObject Player;
    [HideInInspector]
    public Transform PlayerTransform;

    [HideInInspector]
    public GameObject Drone;
    [HideInInspector]
    public DroneScript DroneLogic;
    [HideInInspector]
    public Transform DroneTransform;

    [HideInInspector]
    public NavMeshAgent NavAgent;

    private void Awake()
    {
        this.Player = GameObject.FindGameObjectWithTag(Resources.Tags.Player);
        this.PlayerTransform = this.Player.transform;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.Drone = animator.gameObject;
        this.DroneTransform = this.Drone.transform;
        this.DroneLogic = this.Drone.GetComponent<DroneScript>();

        this.NavAgent = this.Drone.GetComponent<NavMeshAgent>();

        // set all area costs to 1
        this.NavAgent.SetAreaCost(3, 1f);
        this.NavAgent.SetAreaCost(4, 1f);
        this.NavAgent.SetAreaCost(5, 1f);
    }

    public bool IsCloseToPlayer()
    {
        return Vector3.Distance(this.DroneTransform.position, this.PlayerTransform.position) < this.DroneLogic.MaxDistance;
    }

    public bool DestinationReached()
    {
        return this.NavAgent.remainingDistance <= this.NavAgent.stoppingDistance;
    }

    public Vector3 GetRandomDestination(Vector3 origin, float maxDistance, int layerMask)
    {
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * maxDistance;
        randomPosition += origin;

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomPosition, out navMeshHit, maxDistance, layerMask))
        {
            return navMeshHit.position;
        }
        else { return Vector3.zero; }
    }








    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
