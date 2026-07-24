using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Abilitiy : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] protected Transform _characterObject;
    [SerializeField][FoldoutGroup("References")] protected AbilitiesDataSO _abilitiesData;
    [SerializeField][FoldoutGroup("References")] protected ECorgiHabilityEventAsset _abilityEventAsset;
}
