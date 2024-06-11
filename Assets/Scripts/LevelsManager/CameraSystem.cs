using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 可能会和DOTS的命名有冲突；这个类掌管所有Level 镜头，可能会按不同Level 有不同的镜头偏移，但是暂时只是空演示“注入”的类;
/// 原：BaseCamera.cs
/// </summary>
public class CameraSystem : MonoBehaviour
{
    public Camera Camera;
    [Zenject.Inject]
    void Init(Zenject.SignalBus signalBus)
    {
        Debug.LogError("CameraSystem Inject 看看流程。。。注入入口 step 1??");
    }
}
