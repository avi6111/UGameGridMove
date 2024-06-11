using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(IslandsProvider))]
public class IslandsProviderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("挂 Parent 会自动处理-自节点Islands(注入有用）");
        base.OnInspectorGUI();
    }
}
