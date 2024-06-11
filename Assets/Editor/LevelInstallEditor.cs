using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(LevelInstaller))]
public class LevelInstallEditor : Editor
{
    private int hintCount = -1;
    private int stepLimitCount = -1;
    private LevelInstaller _target;

    private void OnEnable()
    {
        _target = (LevelInstaller)target;
    }

    void Awake()
    {
        DoRelativeSettingInScene();
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        if (GUILayout.Button("自动(刷新)关联"))
        {
            DoRelativeSettingInScene();
        }
        
        EditorGUILayout.LabelField("stepLimit", "" + stepLimitCount);
        EditorGUILayout.LabelField("hintCount",""+hintCount + " -- 每一步偏移填 6.5（从Transform 也能看出是6.5）");
    }

    void DoRelativeSettingInScene()
    {
        
        
        var hint = FindObjectOfType<LevelHintSteps>();
        if (hint)
        {
            hintCount = hint.GetAllSteps.Count;
            if (_target.levelHintSteps == null) _target.levelHintSteps = hint;
        }
        
        var ls = Object.FindObjectOfType<LevelSettings>();
        if (ls)
        {
            stepLimitCount = ls.Steps;
            if (_target&& _target.levelSettings == null) _target.levelSettings = ls;
        }
    }
}
