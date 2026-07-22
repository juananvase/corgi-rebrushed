using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class WaterJump : MonoBehaviour
{
    [SerializeField] [FoldoutGroup("References")] private AbilitiyManager _manager;
    [SerializeField] [FoldoutGroup("References")] private Rigidbody _rigidbody;
    
    // Cache an array for non-allocating physics checks (Max 20 targets per hit)
    private readonly Collider[] hitBuffer = new Collider[20];

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    [Button("WaterImpulse")]
    private void WaterImpulse()
    {
        ApplyImpulseForce();
        ApplyDamage();
    }

    private void ApplyImpulseForce()
    {
        _rigidbody.AddForce(_manager.CharacterObject.up * _manager.AbilitiesData.WaterJumpImpulseForce, ForceMode.Impulse);
    }

    private void ApplyDamage()
    {
        Vector3 boxCenter = _manager.CharacterObject.position + _manager.CharacterObject.TransformDirection(_manager.AbilitiesData.WaterJumpAttackOffset);
        
        int hitCount = Physics.OverlapBoxNonAlloc(
            boxCenter,
            _manager.AbilitiesData.WaterJumpAttackBoxHalfExtents,
            hitBuffer,
            transform.rotation,
            _manager.AbilitiesData.EnemyLayer
        );
        
        for (int i = 0; i < hitCount; i++)
        {
            Collider enemyCollider = hitBuffer[i];
            
            // Try to get damageable component (adjust to your project's health script)
            Debug.Log($"Hit {enemyCollider.name}");
            
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 boxCenter = transform.position + transform.TransformDirection(_manager.AbilitiesData.WaterJumpAttackOffset);
        
        // Match matrix to character rotation so gizmo turns when player turns
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _manager.AbilitiesData.WaterJumpAttackBoxHalfExtents * 2f);
    }
#endif
    
}
