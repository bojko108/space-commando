using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Script for attaching material to selected GameObject
/// </summary>
public class AttachMaterial : ScriptableWizard
{
    [Tooltip("Material to add")]
    public Material MaterialToAdd;

    [MenuItem("Tools/Add Material")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<AttachMaterial>("Add Material selected Game Objects", "Add");
    }

    /// <summary>
    /// Called when the "Add" button is clicked
    /// </summary>
    private void OnWizardCreate()
    {
        if (this.MaterialToAdd == null) return;

        try
        {
            Transform[] transforms = Selection.transforms;

            foreach (Transform transform in transforms)
            {
                GameObject go = transform.gameObject;

                if (go == null) continue;

                MeshRenderer renderer = go.GetComponent<MeshRenderer>();
                renderer.material = this.MaterialToAdd;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}