using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Script for extruding selected polygon game objects
/// </summary>
public class Extrude : ScriptableWizard
{
    [Tooltip("Extrusion value")]
    public float Extrusion = 0f;
    [Tooltip("If the faces are not visible toggle this parameter")]
    public bool InvertFaces = true;
    
    [MenuItem("Tools/Extrude selected")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<Extrude>("Extrude selected Game Objects", "Extrude");
    }

    /// <summary>
    /// Called when the "Extrude" button is clicked
    /// </summary>
    private void OnWizardCreate()
    {
        if (this.Extrusion <= 0) return;

        try
        {
            Transform[] transforms = Selection.transforms;

            foreach (Transform transform in transforms)
            {
                GameObject go = transform.gameObject;
                
                if (go == null) continue;

                MeshExtrusion.Extrude(go, this.Extrusion, this.InvertFaces);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    private Matrix4x4[] CalculateExtrudeSections(Transform transform)
    {
        Matrix4x4[] finalSections = new Matrix4x4[2];

        //Matrix4x4 matrix = transform.localToWorldMatrix;
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = currentPosition;
        targetPosition.y += this.Extrusion;

        //Quaternion direction = Quaternion.LookRotation(targetPosition - currentPosition);
        Quaternion direction = Quaternion.LookRotation(transform.forward, transform.up);

        Matrix4x4 worldToLocal = transform.worldToLocalMatrix;

        finalSections[0] = worldToLocal * Matrix4x4.TRS(currentPosition, direction, transform.lossyScale);
        finalSections[1] = worldToLocal * Matrix4x4.TRS(targetPosition, direction, transform.lossyScale);

        return finalSections;
    }
}