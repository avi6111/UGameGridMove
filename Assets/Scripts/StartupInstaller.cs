using UnityEngine;
using Zenject;
/// <summary>
/// 启动页（必须）
/// </summary>
public class StartupInstaller : MonoInstaller
{
    private ProjectLoadedHandler _projectLoadedHandler;

    public override void InstallBindings(){
        Debug.LogError("InstallBindings");
        Container.Bind<ProjectLoadedHandler>().AsSingle().NonLazy();
    }

    public override void Start(){
        _projectLoadedHandler = Container.Resolve<ProjectLoadedHandler>();
        _projectLoadedHandler.Start();
    }
}