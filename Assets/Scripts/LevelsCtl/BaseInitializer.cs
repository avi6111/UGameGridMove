using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Zenject;

public class BaseInitializer : IInitializable
{
    //[Inject]
    //private BaseCamera _baseCamera;
    [Inject]
    private KeyBaseUI _baseUI;

    [Inject] private PlayerData _playerData;
    private int _currLevel;
    /// <summary>
    /// 继承 IInitializable ，应该是初始化即可调用
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Initialize()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// DiContainer->CallInjectMethodsTopDown();
    ///暂时只知道唯一入口（不确定是不是每关都进入）：
    ///     KeyLevelInstaller
    ///         Container.BindInterfacesAndSelfTo<BaseInitializer>().AsSingle().WithArguments(currentLevelNumber).NonLazy();
    /// </summary>
    /// <param name="levelNum"></param>
    [Inject]
    public void InitLevelStart(int levelNum)
    {
        Debug.LogError("Init Start(BaseInitializer) level=" + levelNum);
        _currLevel = levelNum;
    }

    private SignalBus _signalBus;
    /// <summary>
    /// 同；DiContainer->CallInjectMethodsTopDown()；
    /// 应该是写两个 [Inject]，能注入方法，两次 
    /// </summary>
    /// <param name="signalBus"></param>
    /// <param name="pathChecker"></param>
    [Inject]
    private void InitSignals(SignalBus signalBus, PathChecker pathChecker)
    {
        _signalBus = signalBus;
        Debug.LogError("Init Signal(BaseInitializer)  pathChecker=" +(pathChecker!=null));
        signalBus.Subscribe<IslandUpdatedSignal>(pathChecker.CheckPath);
        ////////////// TutorialHanlder ///////////////////////////
        //signalBus.Subscribe<LevelCompletedSignal>(OnTutorialFinished);
        ///////////// LevelCompleteHandler /////////////
        signalBus.Subscribe<LevelCompletedSignal>(OnLevelCompleted);
        //start 事件其实在 [Inject]InitLevelStart（）已处理;
        // //signalBus.Subscribe<ILevelStarterSignal>();
        // signalBus.Subscribe<LevelStartSignal>(OnLevelStart);
        // //signalBus.Subscribe<LoadLevelSignal>(OnLevelStart);//这个还是在LevelButton->LoadLevel（）之后，所以监听不到
    }
    
    private void OnTutorialFinished(){
        //Analytics.TutorialCompleted();
        //_baseCamera.PlayOutAnimation();
        //Timer.StartNew(_tutorial, _baseCamera.AnimationDuration, () => _soundsPlayer.PlayLevelCompletedSound());
    }
    //在 [Inject]InitLevelStart（）已处理;
    // void OnLevelStart(LevelStartSignal signal)
    // {
    //     Debug.LogError("every Level start lv.= " + signal.LevelNumber);
    //     //var loader = _signalBus.get.re
    // }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="signal">空参数，暂无任何数据包含（不知道怎么包含）</param>
    private void OnLevelCompleted(LevelCompletedSignal signal){
        
        //个人关卡数据
        _playerData.LastUnlockedLevel = _currLevel + 1;
        
        //奖励
        // if(_stepsViewModel.IsBonusReceived() && _playerData.CompletedLevelsWithBonus.Contains(_levelNumber) == false)
        //     _playerData.CompletedLevelsWithBonus.Add(_levelNumber);
    
        //_baseCamera.PlayOutAnimation();

        // //是否完成最后一关
        //bool isLastLevelCompleted = _playerData.LastUnlockedLevel > _levelsInfoProvider.LevelsCount;
        _baseUI.LevelCompleted(false);

        //_baseSoundsPlayer.PlayLevelCompletedSound(BaseUI.PanelsAnimationDuration);
    }
}
