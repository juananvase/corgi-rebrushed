using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")] private EnemyDataSO _enemyData;
    [SerializeField, FoldoutGroup("References")] private NavMeshAgent _navMeshAgent; 
    [SerializeField, FoldoutGroup("References")] private Rigidbody _rigidbody; 
    [SerializeField, FoldoutGroup("References")] private Animator _animator;
    
    private Transform _playerTransform;

    private Vector3 _currentPatrolPoint;
    private bool _hasPatrolPoint;

    private bool _isOnAttackCoolDown;
    private Coroutine _attackCooldownCoroutine;

    private bool _isPlayerVisible;
    private bool _isPlayerInRange;
    
    private int _speed;
    private int _attackTrigger;
    
    // Cache an array for non-allocating physics checks (Max 5 targets per hit)
    private readonly Collider[] hitBuffer = new Collider[5];

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _attackTrigger = Animator.StringToHash("Attack");
        _speed = Animator.StringToHash("Speed");
    }

    private void Start()
    {
        _playerTransform = GameManager.instance.PlayerTransform;
    }

    private void Update()
    {
        _animator.SetFloat(_speed, _navMeshAgent.velocity.magnitude);
        
        DetectPlayer();
        UpdateBehaviourState();
    }

    private void FixedUpdate()
    {
        ApplyStopingForce();
    }

    private void ApplyStopingForce()
    {
        if (_navMeshAgent.desiredVelocity.magnitude < 1f)
        {
            Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
            _rigidbody.AddForce(horizontalVelocity * -15f, ForceMode.Force);
        }
    }

    private void UpdateBehaviourState()
    {
        if (!_isPlayerVisible && !_isPlayerInRange)
        {
            PerformPatrol();
        }
        else if (_isPlayerVisible && !_isPlayerInRange)
        {
            PerfomChase();
        }
        else if (_isPlayerVisible && _isPlayerInRange)
        {
            PerfomAttack();
        }
    }

    private void PerformPatrol()
    {
        if(!_hasPatrolPoint) FindPatrolPoint();
        if(_hasPatrolPoint) _navMeshAgent.SetDestination(_currentPatrolPoint);
        if(Vector3.Distance(transform.position, _currentPatrolPoint) < 1f) _hasPatrolPoint = false;
    }

    private void PerfomChase()
    {
        if (_playerTransform != null)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
    }

    private void PerfomAttack()
    {
        _navMeshAgent.SetDestination(_playerTransform.position);

        if (_playerTransform != null)
        {
            transform.LookAt(_playerTransform);
        }

        if (!_isOnAttackCoolDown)
        {
            Attack();
            if(_attackCooldownCoroutine  != null) StopCoroutine(_attackCooldownCoroutine);
            _attackCooldownCoroutine = StartCoroutine(AttackCooldownRoutine());
        }
    }

    private void DetectPlayer()
    {
        _isPlayerVisible = Physics.CheckSphere(transform.position, _enemyData.VisionRange, _enemyData.PlayerLayer);
        _isPlayerInRange = Physics.CheckSphere(transform.position, _enemyData.EngagementRange, _enemyData.PlayerLayer);
    }

    private void Attack()
    {
        _animator.SetTrigger(_attackTrigger);
    }

    public void ApplyDamageOnCue()
    {
        ApplyDamage(_enemyData.AttackHalfExtents, _enemyData.AttackOffset, _enemyData.MeleeAttackDamage, EDamageType.Scratch);
    }
    
    private IEnumerator AttackCooldownRoutine()
    {
        float currentSpeed = _navMeshAgent.speed;
        
        _isOnAttackCoolDown =  true;
        _navMeshAgent.speed = 0f;
        
        yield return Tween.Delay(_enemyData.AttackCooldown).ToYieldInstruction();
        
        _animator.ResetTrigger(_attackTrigger);
        _isOnAttackCoolDown  = false;
        _navMeshAgent.speed = currentSpeed;
    }
    
    private void ApplyDamage(Vector3 halfExtents, Vector3 attackOffset, float damage, EDamageType damageType)
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(attackOffset);
        
        int hitCount = Physics.OverlapBoxNonAlloc(
            boxCenter,
            halfExtents,
            hitBuffer,
            transform.rotation,
            _enemyData.PlayerLayer
        );
        
        for (int i = 0; i < hitCount; i++)
        {
            Collider other = hitBuffer[i];
            
            if (other.transform.root.gameObject.TryGetComponent(out IDamageable target))
            {
                target.Damaged(new DamageInfo(damage, other.gameObject, this.gameObject, this.gameObject, damageType));
            }
            
        }
    }

    private void FindPatrolPoint()
    {
        float randomX = Random.Range(-_enemyData.PatrolRadius, _enemyData.PatrolRadius);
        float randomZ = Random.Range(-_enemyData.PatrolRadius, _enemyData.PatrolRadius);
        
        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(potentialPoint, -transform.up, 2f, _enemyData.GorundLayer))
        {
            _currentPatrolPoint = potentialPoint;
            _hasPatrolPoint = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyData.EngagementRange);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemyData.VisionRange);
        
        Gizmos.color = Color.blueViolet;
        Vector3 boxCenter = transform.position + transform.TransformDirection(_enemyData.AttackOffset);
        
        // Match matrix to character rotation so gizmo turns when player turns
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _enemyData.AttackHalfExtents * 2f);
    }
}
