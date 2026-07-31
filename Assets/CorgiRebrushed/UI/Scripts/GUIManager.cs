using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance { get; private set; }
    
    private void InitiateSinglenton()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }
    
    [field: SerializeField, FoldoutGroup("References")] public Image WaterAbilityFillBar { get; set; }
    [field: SerializeField, FoldoutGroup("References")] public Image TreeAbilityFillBar { get; set; }
    [field: SerializeField, FoldoutGroup("References")] public Image ChilliAbilityFillBar { get; set; }
    [field: SerializeField, FoldoutGroup("References")] public Image HealthFillBar { get; set; }
    [field: SerializeField, FoldoutGroup("References")] public TextMeshProUGUI TreatsCount { get; set; }
    
    public float WaterAbilityProgress { get; set; }
    public float TreeAbilityProgress { get; set; }
    public float ChilliAbilityProgress { get; set; }
    public float HealthProgress { get; set; }

    private void Awake()
    {
        InitiateSinglenton();
    }

    private void Update()
    {
        if(WaterAbilityFillBar  != null) WaterAbilityFillBar.fillAmount = WaterAbilityProgress;
        if(TreeAbilityFillBar  != null) TreeAbilityFillBar.fillAmount = TreeAbilityProgress;
        if(ChilliAbilityFillBar  != null) ChilliAbilityFillBar.fillAmount = ChilliAbilityProgress;
        if(HealthFillBar  != null) HealthFillBar.fillAmount = HealthProgress;

        if (TreatsCount != null) TreatsCount.text = $"{GameManager.instance.TreatCount}X";
    }
}
