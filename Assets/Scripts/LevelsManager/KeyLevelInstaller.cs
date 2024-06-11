using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
/// <summary>
/// 原：BaseInstall.cs 在场景：Key Level-Base.unity;
/// 这和LevelInstaller 是有区别的，但是也是每关都触发！！！￥@#%#%
/// </summary>
public class KeyLevelInstaller : MonoInstaller
{
    //也是可以这么注入的。。。。
    // [Inject]
    // public void Init(Settings set)
    // {
    // }

    public CameraSystem cameraSys;
    public KeyBaseUI keyBaseUI;
    public override void InstallBindings()
    {
        //Container.Bind<PauseManager>().FromNew().AsSingle().NonLazy();//现在会报错。。。。。
        //1.注册回退
        Container.Bind<StepsRecorder>().To<LevelStepsRecorder>().AsSingle().NonLazy();//貌似会开，第4个 DIContainer，之后再查吧。。。。
        //Container.BindInstance(levelSettings).AsSingle();
        //Container.DeclareSignal()
        Debug.LogError("看看流程。。base(每个场景~=初始化一次）。。注入入口，bind baseUI");
        
        //2.注册Ui 和 Cameral
        //Container.BindInstances(cameraSys,baseUI);
        Container.BindInstances(cameraSys,keyBaseUI);
        
        //3.注册步骤，和 Signal
        Debug.LogWarning("Bind StepsViewModel");
        //StepsViewModel 依赖类实体：LevelInstance ，需要在 LevelInstaller.cs 初始化
        Container.Bind<StepsViewModel>().AsSingle().WithArguments(true);//必须带上 WithArguments(true)，否则不会初始化，false参数没测试
        //Bind IslandUpdater
        Container.BindInterfacesAndSelfTo<IslandsUpdater>().AsSingle().WithArguments((MonoBehaviour) this, cameraSys.Camera);

        Container.DeclareSignal<IslandUpdatedSignal>();
        Container.DeclareSignal<IslandUpdatingSignal>();
        Container.DeclareSignal<CantUpdateIslandSignal>();

        //4.注册PlayerData（在：PojrectInstaller处理
        // //Container.Bind<PlayerData>().AsSingle().WithArguments(2).NonLazy();
        // SaveSystem<PlayerData> ss = new SaveSystem<PlayerData>();
        // PlayerData data = ss.LoadData();
        // Container.BindInstance(data).AsCached().NonLazy();
        
        //5.注册，测试。。。。。
        //通过注册 BaseInitializer.cs，从而注册  SignalBus().Subscribe<>();
        int currentLevelNumber = Container.Resolve<ScenesLoader>().LastLoadedLevelNumber;
        Container.BindInterfacesAndSelfTo<BaseInitializer>().AsSingle().WithArguments(currentLevelNumber).NonLazy();
        Debug.LogWarning("Bind ended");

        //打印看看那些Provider 已经注册，（里面是数组包数值的，打印后法线，（不同场景，不同Installer之间要填 parent 才可能关联上））
        Container.PrintOutProviders();
        //bool isBonusReceivedEarlier = Container.Resolve<PlayerData>().CompletedLevelsWithBonus.Contains(currentLevelNumber);
        //Container.Bind<StepsViewModel>().AsSingle().WithArguments(isBonusReceivedEarlier);

    }
}
