using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Bot))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Bot myScript = (Bot)target;
        if(GUILayout.Button("Mover Bot"))
        {
            myScript.mover(new Vector3(myScript.nuevox, myScript.transform.position.y, myScript.nuevoz));
        }
    }
}
