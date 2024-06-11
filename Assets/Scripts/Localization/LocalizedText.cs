using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    // TODO: Change to auto-implemented property with a private setter or make this field private
    public string LocalizationKey;

    private TextMeshProUGUI _textMesh;
    private Zenject.SignalBus _signalBus;

    private void OnValidate() {
        _textMesh = GetComponent<TextMeshProUGUI>();    
    }

    [Zenject.Inject]
    private void Init(Zenject.SignalBus signalBus){
        _signalBus = signalBus;
        //TODO 这里其中一个报错，会导致整个 注入 链条的错误。。。后面不执行注入了。。。。
//        signalBus.Fire(new ObjectCreatedSignal<LocalizedText>(this));
    }

    public void UpdateText(string text) => _textMesh.text = text;

    private void OnDestroy() {
        _signalBus.Fire(new ObjectDestroyedSignal<LocalizedText>(this));
    }
}