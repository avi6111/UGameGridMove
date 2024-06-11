using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup graphics;
    [SerializeField] private float graphicsClosedPosition;
    [SerializeField] private float graphicsLockedAlpha = 0.45f;
    [Space]
    [SerializeField] private AnimatedButton button;
    [Space]
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject completedPanel;
    [Space]
    [SerializeField] private GameObject bonusPanel;
    [SerializeField] private GameObject bonusReceivedPanel;

    private int _levelNumber;
    // /// <summary>
    // /// Timer 在Project 目录下（基于 协程的一个Timer)
    // /// </summary>
    // private Timer _animationTimer;
    private bool isLevelLocked;

    private Zenject.SignalBus _signalBus;

    public void Init(in Zenject.SignalBus signalBus, int number){
        _signalBus = signalBus;

        _levelNumber = number;
        levelNumberText.text = number.ToString();
    }

    public void SetBonusActive(bool active) => bonusPanel.SetActive(active);
    public void BonusReceived() => bonusReceivedPanel.SetActive(true);

    /// <summary>
    /// 外部传进来的，打开关卡-入口代码
    /// </summary>
    public void LoadLevel()
    {
        _signalBus.Fire(new LoadLevelSignal(_levelNumber));
    } //这里需要 single 类，不知道另外写的用意是？？

    private Sequence _graphicsAnimationSequence;
    private const float GraphicsAnimationDuration = 0.2f;
    public void Animate(float delay){
        if(_graphicsAnimationSequence != null) _graphicsAnimationSequence.Kill();
        _graphicsAnimationSequence = DOTween.Sequence();

        graphics.transform.localPosition = new Vector3(graphics.transform.localPosition.x, graphicsClosedPosition, graphics.transform.localPosition.y);
        graphics.alpha = 0;

        _graphicsAnimationSequence.Insert(0, graphics.transform.DOLocalMoveY(0, GraphicsAnimationDuration).SetEase(Ease.OutCubic).SetDelay(delay));
        _graphicsAnimationSequence.Insert(0, graphics.DOFade(GetAlphaForGraphics(), GraphicsAnimationDuration).SetDelay(delay));
    } 

    public void SetLockedState(bool locked){
        isLevelLocked = locked;
        graphics.alpha = GetAlphaForGraphics();
        if(button!=null)
            button.Interactable = !isLevelLocked;
    }

    private float GetAlphaForGraphics() => isLevelLocked ? graphicsLockedAlpha : 1f;

    public void LevelCompleted(){
        SetLockedState(false);
        completedPanel.SetActive(true);
    }
}