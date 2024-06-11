using System.Linq;
using UnityEngine;
/// <summary>
/// Project Context.pefab|ProjectInstall.cs 初始化
/// </summary>
public class Settings : Zenject.IInitializable
{
    private Localization _localization;

    public bool IsMusicEnabled { 
        get => _data.IsMusicEnabled; 
        set{
            _data.IsMusicEnabled = value;
            _signalBus.Fire(new IsMusicEnabledChangedSignal() { IsEnabled = value });
        } 
    }
    public bool IsSoundsEnabled { 
        get => _data.IsSoundsEnabled;
        set{
            _data.IsSoundsEnabled = value;
            _signalBus.Fire(new IsSoundsEnabledChangedSignal() { IsEnabled = value });
        } 
    }

    private Zenject.SignalBus _signalBus;

    private Data _data;
    private SaveSystem<Data> _saveSystem;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="signalBus"></param>
    /// <param name="localization">如果没有会导致后面 CachedProvider报错？？？Zenject.ZenjectException: Found circular dependency when creating type 'Settings'. Object graph </param>
    public Settings(Zenject.SignalBus signalBus/*, Localization localization*/){
        Debug.LogError("PlayerData Construct()");
        _signalBus = signalBus;
        //_localization = localization;//暂时没有

        signalBus.Subscribe<OnQuitSignal>(OnQuit);
    }

    void Zenject.IInitializable.Initialize(){
        Debug.LogError("PlayerData Initialize()");
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        _saveSystem = new SaveSystem<Data>();
        _data = _saveSystem.LoadData();

        IsMusicEnabled = _data.IsMusicEnabled;
        IsSoundsEnabled = _data.IsSoundsEnabled;
    }

    public void ChangeLocalizationToNextLanguage() => _localization.ChangeToNextLanguage();

    public string GetDisplayingNameOfCurrentLanguage() 
        => _localization.Languages.FirstOrDefault(language => language.LanguageCode == _localization.CurrentLanguageCode).DisplayingName;

    public void OnQuit(){
        Debug.LogError("OnQuitEvent？？PlayerData 已经保存一次了，这事 OnQuit事件");
        //_saveSystem.SaveData(_data);//保存两次没必要
    }

    public class Data : ISaveable
    {
        public bool IsSoundsEnabled = true;
        public bool IsMusicEnabled = true;

        public string FileName => "settings";
    }

    public struct IsSoundsEnabledChangedSignal 
    { 
        public bool IsEnabled;
    }

    public struct IsMusicEnabledChangedSignal 
    { 
        public bool IsEnabled;
    }
}