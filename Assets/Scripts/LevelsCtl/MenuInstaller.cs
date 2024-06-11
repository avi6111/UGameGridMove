using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    // 也是可以这么写的， MonoInstaller 注入Settings.cs
    // [Inject]
    // void StartInstaller(Zenject.SignalBus _signalBus,Settings settings)
    // {
    //     Debug.LogError("MenuInstaller ?? Init()");
    // }
    //只能是继承 这个事件 用
    public override void InstallBindings()
    {
    }

}
