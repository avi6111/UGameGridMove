using UnityEngine;
using Zenject;
/// <summary>
/// 单个关卡的注册，和KeyLevelInstaller.cs 不同
/// </summary>
public class LevelInstaller : MonoInstaller
{
    [Header("每个关卡都要配置，都挺麻烦的！！！  ")]
    public LevelSettings levelSettings;
    /// <summary>
    /// 一些移动步骤回退，或者提示
    /// </summary>
    public LevelHintSteps levelHintSteps;
    [Space]
    public IslandsProvider islandsProvider;
    //[SerializeField] private IslandsAnimator islandsAnimator;
    
    public override void InstallBindings(){
        Debug.LogWarning("看看流程---LevelInstaller ???levelSettings,levelHintSteps ");
        DeclareSignals();
        
        Container.BindInstance(levelSettings).AsSingle();

        Container.BindInstance(levelHintSteps).AsSingle();

        //Container.BindInstances(islandsAnimator, islandsProvider);
        Container.BindInstance(islandsProvider);
        
        //能绑定，但是不能绑定实体
        //Container.Bind<IslandsProvider>().AsSingle();
        Container.Bind<PathChecker>().AsSingle().NonLazy();
        Debug.LogWarning("看看 LevelInstaller ??? ended");
        
        
        //LoadLevelSignal.cs 已经有在处理
        // var signalBus = Container.Resolve<SignalBus>();
        // LevelStartSignal signal = new LevelStartSignal();
        // signal.currLevel = levelSettings.le;
        // signalBus.Fire<LevelStartSignal>(1);
    
    }
    
    private void DeclareSignals(){
        Container.DeclareSignal<LevelCompletedSignal>();
        
    }
}