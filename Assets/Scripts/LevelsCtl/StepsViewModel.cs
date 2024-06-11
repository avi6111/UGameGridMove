using System;
using UnityEngine;

public class StepsViewModel : ViewModel
{
    public int StartStepsCount { get; private set; }

    public ObservableProperty<int> StepsLeft { get; private set; } = new ObservableProperty<int>();
    public ObservableProperty<int> StepsForBonus { get; private set; } = new ObservableProperty<int>();
    
    public DelegateCommand MoveToPreviousStepCommand { get; private set; }

    private bool isBonusReceivedEarlier;

    private StepsRecorder _stepsRecorder;
    private IslandsUpdater _islandsUpdater;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelSettings">需要：Level i.unity 场景中，有一个 LevelSettings(Mono(</param>
    /// <param name="islandsUpdater"></param>
    /// <param name="stepsRecorder">传入：LevelStepsRecorder；通过：InitCommands（）又再追加事件： _stepsRecorder.StepRecorded 更新：->是否可以Changed()</param>
    /// <param name="isBonusReceivedEarlier"></param>
    public StepsViewModel(LevelSettings levelSettings, IslandsUpdater islandsUpdater, StepsRecorder stepsRecorder, bool isBonusReceivedEarlier) {
        Debug.LogError("看看 StepsViewModel constrct()");
        _stepsRecorder = stepsRecorder;

        InitCommands();

        _islandsUpdater = islandsUpdater;

        this.isBonusReceivedEarlier = isBonusReceivedEarlier;

        StartStepsCount = StepsLeft.Value = levelSettings.Steps;
        StepsForBonus.Value = levelSettings.StepsForBonus;
    }

    [Zenject.Inject]
    private void InitSignals(Zenject.SignalBus signalBus){
        Debug.LogError("看看 StepsViewModel Inject");
        signalBus.Subscribe<IslandUpdatingSignal>(OnIslandUpdating);
    }

    private void InitCommands(){
        MoveToPreviousStepCommand = new DelegateCommand(OnMovedToPreviousStep, _stepsRecorder.CanMoveToPrevStep);
        _stepsRecorder.StepRecorded += MoveToPreviousStepCommand.InvokeCanExecuteChanged;
    }

    private void OnIslandUpdating(){
        _stepsRecorder.RecordStep();
        StepsLeft.Value--;

        if(StepsLeft == 0)
            _islandsUpdater.IsIslandsUpdatingAllowed = false;
    }

    private void OnMovedToPreviousStep(){
        StepsLeft.Value++;
        _stepsRecorder.MoveToPreviousStep();
        _islandsUpdater.IsIslandsUpdatingAllowed = true;
        _islandsUpdater.ExternalUpdateStarted(_stepsRecorder.IslandAnimationDuration);
        MoveToPreviousStepCommand.InvokeCanExecuteChanged();
    }

    public bool IsBonusReceived(){
        if(isBonusReceivedEarlier)
            return true;

        return StartStepsCount - StepsLeft == StepsForBonus;
    }
}
