using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class WoolBallController : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")] private Rigidbody _rigidbody;
    [SerializeField, FoldoutGroup("References")] private Transform _respawnPoint;
    [SerializeField, FoldoutGroup("WoolBall")] private float _acceleration;
    [SerializeField, FoldoutGroup("WoolBall")] private Vector3 _direction;
    [SerializeField, FoldoutGroup("WoolBall")] private float _respawnTime;
    [SerializeField, FoldoutGroup("WoolBall")] private float _maxSpeed;
    [SerializeField, FoldoutGroup("WoolBall")] private float _damage;
    [SerializeField, FoldoutGroup("WoolBall")] private string[] _hitLayers;
    
    [ShowInInspector, FoldoutGroup("Testing")] public float CurrentAcceleration { get; set; }
    [ShowInInspector, FoldoutGroup("Testing")] public float CurrentMaxSpeed { get; set; }
    
    private float _nextReadyTime;
    private bool _isCooldownOver => Time.time >= _nextReadyTime;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        CurrentAcceleration = _acceleration;
        CurrentMaxSpeed = _maxSpeed;
    }

    private void Update()
    {
        TryRespawnToOrigin();
    }

    private void TryRespawnToOrigin()
    {
        if (!_isCooldownOver) return;
        
        transform.position = _respawnPoint.position;
        transform.rotation = _respawnPoint.rotation;
        
        _nextReadyTime = ResetCooldown(_respawnTime);
    }

    private void FixedUpdate()
    {
        ApplyForceToHorizontalMovement();
        CapVelocity();
    }
    
    protected float ResetCooldown(float cooldownDuration)
    {
        return Time.time + cooldownDuration;
    }

    private void ApplyForceToHorizontalMovement()
    {
        _rigidbody.AddForce(new Vector3(0,0,1) * CurrentAcceleration, ForceMode.Force);
    }
    
    private void CapVelocity()
    {
        Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if (horizontalVelocity.magnitude > CurrentMaxSpeed)
        {
            Vector3 cappedVelocity = horizontalVelocity.normalized * CurrentMaxSpeed;
            _rigidbody.linearVelocity = new Vector3(cappedVelocity.x, _rigidbody.linearVelocity.y, cappedVelocity.z);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                if (other.transform.root.gameObject.TryGetComponent(out IDamageable target))
                {
                    target.Damaged(new DamageInfo(_damage, other.gameObject, gameObject, gameObject, EDamageType.Wool));
                }
            }
        }
    }
}
