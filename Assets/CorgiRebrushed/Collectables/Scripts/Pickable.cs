using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    [SerializeField] protected string[] _hitLayers;

    protected void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                OnCollected(other.gameObject);
            }
        }
    }

    protected virtual void OnCollected(GameObject other)
    {
        
    }
}
