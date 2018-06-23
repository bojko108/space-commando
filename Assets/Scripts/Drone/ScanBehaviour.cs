using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBehaviour : BaseBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.NavAgent.baseOffset = this.DroneLogic.ScanHeight;
        
        GameObject.FindGameObjectWithTag(Resources.Tags.CommandScan).GetComponent<UnityEngine.UI.Text>().color = Color.white;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // initiate scan
        // calculate path to found targets: 
        // - computer room
        // - service room
        // - ship

        // all calculated paths will be destroyed when exiting this mode
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.FindGameObjectWithTag(Resources.Tags.CommandScan).GetComponent<UnityEngine.UI.Text>().color = Color.black;
    }
}
