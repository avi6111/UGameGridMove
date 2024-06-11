using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// 检查类前缀：ValXXXXXXX;
/// Editor二次开发，公司内部PackageManager，含下载等功能， CoderZ1010小哥的出品，略屌：https://bbs.huaweicloud.com/blogs/374346
/// </summary>
public class ValLevelScaneCheck : EditorWindow
{
    [MenuItem("Tools/检查场景")]
    public static void StarShow()
    {
        GetWindow<ValLevelScaneCheck>();
    }

    private GUIStyle  _tempFontStyle = new GUIStyle();
    private GUIStyle _tempLabelStyle = new GUIStyle();
    private string _sceneName;
    /// <summary>
    /// 这个变量有些重叠的，和两个 GameObject 关联，都是true满足所有条件， 才输出 true
    /// </summary>
    private bool _hasLevelSettins;
    private bool _hasInBuildList;

    //private bool _hasLISettins;
    private bool _hasLIProvider;
    private void OnEnable()
    {
        //_tempFontStyle    .normal.textColor = Color.yellow;
        InitStyle();
        _sceneName = SceneManager.GetActiveScene().name;
    }

    void InitStyle()
    {
        _tempFontStyle.fontSize = 20;
        _tempLabelStyle.fontSize = 16;
    }

    void ReCheckScene()
    {
        //var set =   FindObjectsByType<LevelSettings>(FindObjectsSortMode.InstanceID);
        //_hasLevelSettins = set.Length > 0;
        var installer = FindObjectOfType<LevelInstaller>();
        if (installer)
        {
            _hasLIProvider = installer.islandsProvider != null;
            
            _hasLevelSettins = installer.levelSettings != null;
            if (_hasLevelSettins)
            {
                _hasLevelSettins = false;
         
                var sceneCtl = FindObjectOfType<SceneContext>();
                if (sceneCtl)
                {
                    foreach (var inst in sceneCtl.Installers)
                    {
                        if (inst.GetInstanceID() == installer.GetInstanceID())
                        {
                            _hasLevelSettins = true;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            _hasLIProvider  = false;
            _hasLevelSettins = false;
        }

        _hasInBuildList = false;
        if (string.IsNullOrEmpty(_sceneName) == false)
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.path.Contains(_sceneName))
                {
                    _hasInBuildList = true;
                    break;
                }
            }
        }


        
    }

    private void OnGUI()
    {
        if (_tempFontStyle == null)
        {
            InitStyle();
        }

        if (SceneManager.GetActiveScene().name != _sceneName)
        {
            _sceneName = SceneManager.GetActiveScene().name;
            
            ReCheckScene();
        }

        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("当前场景:  " + _sceneName);
        if (GUILayout.Button("重新检测"))
        {
            ReCheckScene();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //分割线
        GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
        
        GUILayout.Label("* <color=yellow>检查场景</color>中是否至少有一个<color=green> LevelSettings</color> by LevelInstaller.cs",_tempFontStyle);
        GUILayout.BeginHorizontal();
        GUILabelBoolean(_hasLevelSettins);
        if (!_hasLevelSettins)
        {
            if (GUILayout.Button("尝试修复关联"))
            {
                var targetObj = FixLevelSettings();
                if(targetObj)
                    EditorUtility.SetDirty(targetObj);
                ReCheckScene();
            }
        }

        GUILayout.EndHorizontal();

        //分割线
        GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
        GUILayout.Label("* <color=yellow>检查场景</color>是否已经添加到 BuildSettings",_tempFontStyle);
        GUILabelBoolean(_hasInBuildList);

        //LevelInstaller专用 gui
        OnCheckLevelInstallerGUI();
        
        GUILayout.Label("* 检查其他");
    }

    void OnCheckLevelInstallerGUI()
    {
        //分割线
        SpectorLine();
        GUILayout.Label("* <color=yellow>检查场景</color>LevelInstaller",_tempFontStyle);
        GUILayout.BeginHorizontal();
        GUILayout.Label("LSettings=");
        //带颜色的 boolean 输出
        GUILabelBoolean(_hasLevelSettins);
        GUILayout.Label("LIslands=");
        GUILabelBoolean(_hasLIProvider);
        GUILayout.EndHorizontal();
    }

    void SpectorLine()
    {
        GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
    }

    void GUILabelBoolean(bool val)
    {
        if(val)
            GUILayout.Label($"<color=green>{val}</color>",_tempLabelStyle);
        else
            GUILayout.Label( val.ToString() );
    }

    GameObject FixLevelSettings()
    {
        var installer = FindObjectOfType<LevelInstaller>();
        if (installer == null)
        {
            EditorUtility.DisplayDialog("", "可能-场景中并没有 LevelInstaller or LevelSettings or Both", "Ok");
            return null;
        }
        else 
        {
            if(installer.levelSettings==null)
                installer.levelSettings = FindObjectOfType<LevelSettings>();
            // //场景中，必须有一个 SceneContext. Installer[]
            var sceneCtl = FindObjectOfType<SceneContext>();
            foreach (var inst in sceneCtl.Installers)
            {
                if (inst==null || inst.GetInstanceID() != installer.GetInstanceID())
                {

                    var array = sceneCtl.Installers.ToArray();
                    array[0]= installer;
                    sceneCtl.Installers = array;
                    EditorUtility.SetDirty(sceneCtl.gameObject);
                    break;
                }
            }

            
            return installer.gameObject;
        }
  
    }
}
