using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GAMEMainMenu : MonoBehaviour
{
    public GAMELevelsView levelsView;
    private Zenject.SignalBus _signalBus;
    private LevelsInfoProvider _levelInfo;
    private PlayerData _data;
    /// <summary>
    ///  初始化 ProjectContext.prefab:ProjectContext.Instance.EnsureIsInitialized();
    ///  ProjectContext 预制体内，有 LevelsInfoProvier (mono)，所以自然会注入了
    /// </summary>
    /// <param name="signalBus"></param>
    /// <param name="levlesInfo">绑定方法：Container.BindInstances(LevelInfoProvider)</param>
    [Zenject.Inject]
    void Init(Zenject.SignalBus signalBus, LevelsInfoProvider levelsInfo,Settings set,PlayerData data)
    {
        _data = data;
        Debug.LogError(" 注入; level_count=" + levelsInfo.LevelsCount +" ;新增了注入：Settings");
        _signalBus = signalBus;
        _levelInfo = levelsInfo;
    }

    private void Awake()
    {
        ProjectContext.Instance.EnsureIsInitialized();
    }

    private void Start()
    {
        
        levelsView.Init(_signalBus, _levelInfo, _data.LastUnlockedLevel, null);
    }

    public void OpenLevelView()
    {
        levelsView.Open();
        //TODO:暂时UI，位置不是负数（没有划入），点击也会遮挡，所以这里暂时处理了点击问题，划入动画和其实位置问题，之后再看看
        //现在已在 Menu.unity -> Canvas -> LevelsView 先不勾选：blocksRaycasts 
        levelsView.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void ClaseLevelView()
    {
        levelsView.Close();
        levelsView.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
