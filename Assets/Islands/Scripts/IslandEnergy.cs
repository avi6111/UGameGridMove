using UnityEngine;
public class IslandEnergy : MonoBehaviour
{
    public bool IsActive { get; private set; }
    
    public bool IsOutputEnabled { get; private set; } = true;
    public bool IsInputEnabled { get; private set; } = true;

    [HideInInspector, SerializeField] private Direction outputEnergyFlowDirection;
    [HideInInspector, SerializeField] private Direction inputEnergyFlowDirection;

#if UNITY_EDITOR
    [HideInInspector, SerializeField] private bool isInputMirrored;
    public string IsInputMirroredFieldName => nameof(isInputMirrored);

    public string OutputFieldName => nameof(outputEnergyFlowDirection);
    public string InputFieldName => nameof(inputEnergyFlowDirection);
#endif

    private Material _rendererMaterial;
    private Color _defaultEmissionColor;

    public void Init(Material rendererMaterial, bool isOutputEnabled, bool isInputEnabled){
#if UnityEditor
//#if UNITY_EDITOR

        if(
            Application.isEditor && UnityEditor.EditorApplication.isPlaying){
            _rendererMaterial = rendererMaterial;
            _defaultEmissionColor = _rendererMaterial.GetColor("_EmissionColor");
        }
#endif
        IsOutputEnabled = isOutputEnabled;
        IsInputEnabled = isInputEnabled;
    }

    public void Activate(){
        SetEnergyActive(true);
    }

    public void Deactivate(){
        SetEnergyActive(false);
    }

    private void SetEnergyActive(bool active){
        IsActive = active;
        //这里报错，会阻塞掉 IsLand->Updated()
        //如果放行，可能会有其他报错（一直爆），所以这里就算错了，还是放任 _rendererMaterial==null; 。。。。
        
        
        /* //DoTween.cs 的try catch 会把这个捕捉到，导致移动的时候“不会提示”报错，而回退操作“会提示”报错（这代码是肯定错的）
         *。。。。。。。。
         * OnTweenCallback
         * 
         */
        _rendererMaterial.SetColor("_EmissionColor", active ? _defaultEmissionColor : Color.black);
    }

    public Direction GetInputDirection(bool consideringRotation = true) => GetEnergyFlowDirection(inputEnergyFlowDirection, consideringRotation);
    public Direction GetOutputDirection(bool consideringRotation = true) => GetEnergyFlowDirection(outputEnergyFlowDirection, consideringRotation);

    private Direction GetEnergyFlowDirection(Direction direction, bool consideringRotation = true){
        if(consideringRotation == false)
            return direction;

        float angle = direction.ToDegrees() + transform.rotation.eulerAngles.y;
        return DirectionExtensions.GetDirectionFromAngle(angle);
    }
}
