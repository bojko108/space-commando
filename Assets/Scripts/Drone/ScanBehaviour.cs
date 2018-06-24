using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScanBehaviour : BaseBehaviour
{
    private bool linesDrawn;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.DroneLogic.SignalLight.DronMode = enumDronMode.Scan;

        GameObject.FindGameObjectWithTag(Resources.Tags.CommandScan).GetComponent<UnityEngine.UI.Text>().color = Color.white;

        this.NavAgent.isStopped = true;

        // initiate scan
        this.DroneLogic.ScannerScript.InitiateScan();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.linesDrawn) return;

        if (Time.frameCount % 20 == 0)
        {
            if (this.DroneLogic.ScannerScript.ScanFinished)
            {
                for (int i = 0; i < this.DroneLogic.ScannerScript.Targets.Length; i++)
                {
                    // calculate path to found targets
                    
                    Transform target = this.DroneLogic.ScannerScript.Targets[i];

                    NavMeshPath path = new NavMeshPath();
                    this.NavAgent.CalculatePath(target.position, path);

                    if (path.status != NavMeshPathStatus.PathInvalid)
                    {
                        this.DroneLogic.ScannerScript.AddDirections(target, path.corners);

                        //for (int k = 1; k < path.corners.Length; k++)
                        //{
                        //    Debug.DrawLine(path.corners[k - 1], path.corners[k]);
                        //}   
                    }
                }

                this.linesDrawn = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // destroy calculated directions

        this.NavAgent.isStopped = false;

        this.DroneLogic.ScannerScript.RemoveDirections();
        
        this.linesDrawn = false;

        GameObject.FindGameObjectWithTag(Resources.Tags.CommandScan).GetComponent<UnityEngine.UI.Text>().color = Color.black;
    }
}
