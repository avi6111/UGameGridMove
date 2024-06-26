using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHintSteps : MonoBehaviour
{
    public List<HintSystem.Step> GetAllSteps => steps;
    [SerializeField] private List<HintSystem.Step> steps;

    public List<HintSystem.Step> GetSteps(){
        return new List<HintSystem.Step>(steps);
    }
}
