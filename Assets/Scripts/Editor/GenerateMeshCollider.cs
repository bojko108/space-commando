using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateMeshCollider : ScriptableWizard
{
    [MenuItem("Tools/Generate Mesh Collider")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<GenerateMeshCollider>("Generate Mesh collider from all children", "Generate");
    }

    private void OnWizardCreate()
    {
        foreach(Transform transform in Selection.transforms)
        {
            this.GenerateCollider(transform.gameObject);
        }
    }

    private void GenerateCollider(GameObject gameObject)
    {
        try
        {
            MeshCollider meshCollider = null;

            if (gameObject.GetComponent<MeshCollider>() != null)
            {
                meshCollider = gameObject.transform.GetComponent<MeshCollider>();
            }
            else
            {
                meshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
            }

            if (meshCollider.sharedMesh == null)
            {
                meshCollider.sharedMesh = new Mesh();
            }

            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                //meshFilters[i].gameObject.SetActive(false);
                i++;
            }

            if (combine.Length > 1)
            {
                meshCollider.sharedMesh.CombineMeshes(combine);
            }
            else
            {
                if (meshFilters.Length > 0)
                {
                    meshCollider.sharedMesh = meshFilters[0].sharedMesh;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}