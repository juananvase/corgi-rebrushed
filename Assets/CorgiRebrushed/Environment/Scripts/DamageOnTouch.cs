using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.root.gameObject.TryGetComponent(out IDamageable target))
        {
            target.Damaged(new DamageInfo(9999f, other.gameObject, gameObject, gameObject, EDamageType.Normal));
        }
    }
}
