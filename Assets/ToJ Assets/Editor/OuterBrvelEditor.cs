using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Bevel))]
public class OuterBrvelEditor : Editor
{
    private Bevel _target;
    private void Awake()
    {
        _target = target as Bevel;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("换颜色"))
        {
            SwithColor();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }

    void SwithColor()
    {
        (_target.highlightColor, _target.shadowColor) = (_target.shadowColor, _target.highlightColor);
        EditorUtility.SetDirty(_target.gameObject);
    }
}
