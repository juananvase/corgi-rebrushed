using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class WaterJump : MonoBehaviour
{
    [SerializeField] [FoldoutGroup("References")] private AbilitiyManager _manager;
    [SerializeField] [FoldoutGroup("References")] private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void WaterImpulse()
    {
        //
    }
}
