using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombineMeshes : ScriptableWizard
{
    [Tooltip("If true, all the Meshes are combined together into a single sub-mesh. Otherwise, each Mesh is placed in a different sub-mesh. If all Meshes share the same Material, this property should be set to true.")]
    public bool MergeSubMeshes = true;

    [MenuItem("Tools/Combine Selected Meshes")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<CombineMeshes>("Combines several Meshes into this Mesh", "Combine");
    }

    private void OnWizardCreate()
    {
        try
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            List<MeshFilter> meshFilters = new List<MeshFilter>();

            for (int i = 0; i < selectedObjects.Length; i++)
            {
                meshFilters.AddRange(selectedObjects[i].GetComponentsInChildren<MeshFilter>());
            }

            CombineInstance[] combine = new CombineInstance[meshFilters.Count];

            int j = 0;
            while (j < meshFilters.Count)
            {
                combine[j].mesh = meshFilters[j].sharedMesh;
                combine[j].transform = meshFilters[j].transform.localToWorldMatrix;
                //meshFilters[j].gameObject.active = false;
                j++;
            }

            GameObject combinedMesh = new GameObject();
            combinedMesh.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = combinedMesh.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();
            meshFilter.sharedMesh.CombineMeshes(combine);
            //transform.gameObject.active = true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}