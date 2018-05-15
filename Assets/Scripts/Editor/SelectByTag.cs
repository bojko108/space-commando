using UnityEditor;
using UnityEngine;

public class SelectByTag : ScriptableWizard
{
    [Tooltip("Set the tag you want to use for selection")]
    public string TagName = null;
    
    [MenuItem("Tools/Select by Tag")]
    private static void SelectByTagWizard()
    {
        // create a dropdown with:  UnityEditorInternal.InternalEditorUtility.tags
        
        ScriptableWizard.DisplayWizard<SelectByTag>("Select GameObjects by tag", "Select");
    }

    // when the "Select" button is pressed
    private void OnWizardCreate()
    {
        if (!string.IsNullOrEmpty(this.TagName))
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(this.TagName);
            Selection.objects = gameObjects;
        }
    }
}
