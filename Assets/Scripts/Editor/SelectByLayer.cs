using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectByLayer : ScriptableWizard
{

    [Tooltip("Set the layer name you want to select from")]
    public string LayerName = null;

    [MenuItem("Tools/Select by Layer")]
    private static void SelectByTagWizard()
    {
        ScriptableWizard.DisplayWizard<SelectByLayer>("Select GameObjects in Layer", "Select");
    }

    // when the "Select" button is pressed
    private void OnWizardCreate()
    {
        if (!string.IsNullOrEmpty(this.LayerName))
        {
            int layerIndex = LayerMask.NameToLayer(this.LayerName);
            
            List<GameObject> allGameObjects = new List<GameObject>();
            allGameObjects.AddRange(GameObject.FindObjectsOfType<GameObject>());
            
            Selection.objects = allGameObjects.FindAll(go => go.layer == layerIndex).ToArray();
        }
    }
}
