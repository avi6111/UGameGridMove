using UnityEngine;
using Zenject;
using System;

public class ProjectInstaller : MonoInstaller
{
    //[SerializeField] public AdsManager adsManager;
    [SerializeField] public LevelsInfoProvider levelsInfoProvider;
    //[SerializeField] public Localization localization;
    [SerializeField] public ScenesLoader scenesLoader;
    //[SerializeField] public MusicPlayer musicPlayer;
    [SerializeField] public ScenesTransitions scenesTransitions;

    private SaveSystem<PlayerData> _saveSystem;
    private PlayerData _data;

    public override void InstallBindings(){   
        Debug.LogError("InstallBindings  main main");
        DeclareSignals();

        BindSettings();
        //Container.DeclareSignal
        BindScenesLoader();//这个必须，终于走！！！，Declared "注入的事件"
        BindLocalization();
        BindAudio();

        BindPlayerData();
        
        //Container.BindInstances(adsManager, levelsInfoProvider);
        Container.BindInstances(levelsInfoProvider);//注册，可注入的参数类型（GAMEMainMenu 需要用到）
        
        //这个必须，绑定“待注入事件，类等”
        Container.Bind<ProjectInitializer>().AsSingle().NonLazy();
    }

    private void DeclareSignals(){
        SignalBusInstaller.Install(Container);//!!!!!重要
        
        Container.DeclareSignal<OnQuitSignal>();
    }

    private void BindSettings(){
        //本地个人数据存储等（Settings.cs 需要 OnQuitSignal）
        Container.BindInterfacesAndSelfTo<Settings>().AsSingle();
        
        //Container.DeclareSignal<Settings.IsMusicEnabledChangedSignal>();
       // Container.DeclareSignal<Settings.IsSoundsEnabledChangedSignal>();
    }

    private void BindScenesLoader(){
        Container.BindInstance(scenesLoader).AsSingle();//貌似是 UI 动画，基础，必须
        Container.Bind<ScenesTransitions>().FromInstance(scenesTransitions).WhenInjectedInto<ScenesLoader>();

        Container.DeclareSignal<LoadMenuSignal>();//现在是空？？
        Container.DeclareSignal<LoadNextLevelSignal>();//现在是空？？
        Container.DeclareSignal<ReloadLevelSignal>();//现在是空？？
        Container.DeclareSignal<LoadLevelSignal>();
    }

    private void BindLocalization(){
        //Container.BindInstance(localization).AsSingle();
     //   Container.BindInstance((Func<string, string>) localization.GetLocalizedValue).AsSingle();
    }

    private void BindAudio(){
   //     Container.BindInstance(musicPlayer).AsSingle();
    //    Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().NonLazy();
    }

    private void BindPlayerData(){ 
        _saveSystem = new SaveSystem<PlayerData>();
        _data = _saveSystem.LoadData();
        Container.BindInstance(_data).AsCached().NonLazy();
        Debug.LogError("Start Bind Event?? PlayerData.level=" + _data.LastUnlockedLevel);
    }

    private void OnApplicationQuit() {
        _saveSystem.SaveData(_data);

        Container.Resolve<SignalBus>().Fire<OnQuitSignal>();
    }
}