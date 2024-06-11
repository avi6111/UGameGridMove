using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// baseUI.cs
/// </summary>
public class KeyBaseUI : MonoBehaviour,IPauseHandler
{
    public BindableAnimatedButton prevButton;
    public BindableAnimatedButton hintButton;

    public BindableAnimatedButton viewAdButton;
    public float PanelsAnimationDuration = 0.3f;
    public PanelAnimator levelCompletedPanel;
    [Space] public Text stepCountText;

    public TextMeshProUGUI stepBonusText;
    public Image stepBonusImage;
    [Space] [SerializeField] private PanelAnimator pausePanel;
    private Zenject.SignalBus _signalBus;
    StepsViewModel _stepModel;

    [Header("暂停背景")]
    public CanvasGroup background;
    //PauseManager _pauseManager;
    [Zenject.Inject]
    void Init(Zenject.SignalBus signalBus, StepsViewModel stepsViewModel /*,PauseManager pauseManager*/)
    {
        Debug.LogWarning("KeyBaseUI??");
        Debug.LogError("<color=cyan>KeyBaseUI Init()</color>");
        _signalBus = signalBus;
        prevButton.Bind(stepsViewModel.MoveToPreviousStepCommand);

        //hintButton.OnClick.AddListener(HintUI.OpenUI);
        //viewAdButton.OnClick.AddListener(); //HintUI.ViewAd

        PauseManager.Inst.Subscribe(this);
        //pauseManager.Subscribe(this);
        //_pauseManager = pauseManager;
        
        stepsViewModel.StepsLeft.Changed += OnStepChanged;
        _stepModel = stepsViewModel;

        signalBus.Subscribe<IslandUpdatedSignal>(OnIslandOneStep);
    }

    private void Start()
    {
        if(stepCountText==null)
        {
            Debug.LogError("<color=cyan>场景没配 Step-text</color>");
        }
        stepCountText.text = _stepModel.StepsLeft.ToString();
        stepBonusText.text = $"Finish in {_stepModel.StepsForBonus} steps";
    }

    void OnIslandOneStep()
    {
        if (_stepModel.StartStepsCount - _stepModel.StepsLeft <= _stepModel.StepsForBonus)
        {
            var color = stepBonusImage.color;
            color.a = 1.0f;
            stepBonusImage.color = color;
        }
        else
        {
            var color = stepBonusImage.color;
            color.a = 0.33f;
            stepBonusImage.color = color;
        }

    }

    void OnStepChanged()
    {
        stepCountText.text = _stepModel.StepsLeft.ToString();
    }

    public void LevelCompleted(bool isAllLevelDone)
    {
        //isLevelCompleted = true;
        //ui canvas Handle
        // mainUI.DOFade(0, 0.2f).SetEase(DG.Tweening.Ease.OutCubic);
        // //ChangeBackgroundVisibility(true, PanelsAnimationDuration);

        //PanelAnimator panel = isLastLevelCompleted ? allLevelsCompletedPanel : levelCompletedPanel;
        PanelAnimator panel = levelCompletedPanel;
        Timer.StartNew(this, PanelsAnimationDuration, () => panel.gameObject.SetActive(true));

        //奖励Timer Handle
        // if(_stepsViewModel.IsBonusReceived()){
        //     bonusReceivedView.gameObject.SetActive(true);
        //     _bonusReceivedViewTimer = Timer.StartNew(this, PanelsAnimationDuration + 0.5f, () => bonusReceivedView.Show(_stepsViewModel.StepsForBonus));
        // }
    }

    /*
     * 按钮Event Listener 0-----------------------------------------
     */
    public void LoadMenu() => _signalBus.Fire<LoadMenuSignal>();
    public void LoadNextLevel() => _signalBus.Fire<LoadNextLevelSignal>();
    public void RestartLevel() => _signalBus.Fire<ReloadLevelSignal>();

    public void PauseGame()
    {
        pausePanel.OpenPanel();
        //这个硬加了一个pause Manager,和 IPauseListener，还是自我回调==自我感动，在游戏开发中，没什么卵用。。。。
        //_pauseManager.SetPaused(true);
        PauseManager.Inst.SetPaused(true);
        
    }
    //by IPauseHandler
    public void SetPauseUI(bool isPaused = true)
    {

        ChangeBackgroundVisibility(isPaused);
    }

    void ChangeBackgroundVisibility(bool visible,float delay=0)
    {
        background.gameObject.SetActive(true);
        
        background.DOFade(visible ? 1 : 0, 0.25f).SetDelay(delay).OnComplete(() => {
            if(visible == false) 
                background.gameObject.SetActive(false);
        });
    }

    public void ResumeGame()
    {
        pausePanel.ClosePanel(true);
        //_pauseManager.SetPaused(false);
        PauseManager.Inst.SetPaused(false);
    }

    public void SetPaused(bool isPaused)
    {
        ChangeBackgroundVisibility(isPaused);
    }
}
