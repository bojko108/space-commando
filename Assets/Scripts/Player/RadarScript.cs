using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScript : MonoBehaviour
{
    [Tooltip("Max distance to display real not border objects")]
    public float SwitchDistance = 40f;

    // minimap targets
    private List<GameObject> targets;
    // layers to display in the minimap
    private List<string> layersToDisplay;

    private void Awake()
    {
        this.targets = new List<GameObject>();
        this.layersToDisplay = new List<string>();
    }

    private void Update()
    {
        foreach (GameObject go in this.targets)
        {
            Quaternion iconRotation;
            Vector3 iconPosition;

            // if the target is far away then display the border object - the icon will be shown at the border of the radar
            if (Vector3.Distance(go.transform.position, this.transform.position) > this.SwitchDistance)
            {
                // display border object instead of the real one
                iconRotation = Quaternion.LookRotation(go.transform.position - this.transform.position);
                iconPosition = this.transform.position + iconRotation * (Vector3.forward * this.SwitchDistance);
            }
            // display icon on the real position
            else
            {
                iconRotation = this.transform.rotation;
                iconPosition = go.transform.position;
            }

            this.SetMinimapIconPosition(go, iconPosition, iconRotation);
        }
    }

    /// <summary>
    /// adds new layer to the minimap
    /// </summary>
    /// <param name="layerName"></param>
    public void AddLayer(string layerName)
    {
        if (this.layersToDisplay.Contains(layerName) == false)
        {
            this.layersToDisplay.Add(layerName);

            int mask = LayerMask.GetMask(this.layersToDisplay.ToArray());
            this.gameObject.FindChildrenByName(Resources.Various.MinimapCamera)[0].GetComponent<Camera>().cullingMask = mask;
        }
    }

    /// <summary>
    /// adds target to the minimap
    /// </summary>
    /// <param name="target"></param>
    public void AddTarget(GameObject target)
    {
        if (this.targets.Contains(target) == false)
            this.targets.Add(target);
    }

    /// <summary>
    /// Sets the position of the minimap icon. Finds the minimap icon inside the childrens of the game object.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="newPosition"></param>
    /// <param name="rotation"></param>
    private void SetMinimapIconPosition(GameObject gameObject, Vector3 newPosition, Quaternion rotation)
    {
        GameObject foundObject = gameObject.FindChildrenByTag(Resources.Tags.MinimapIcon).ToArray()[0];
        foundObject.transform.position = newPosition;
        //// fix me pls
        //foundObject.transform.rotation = rotation;
    }
}
