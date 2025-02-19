using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float power = 10;
    public LayerMask attackableLayer;
    private void OnParticleCollision(GameObject other)
    {
        print("hitting");
        if(other.TryGetComponent<Health>(out Health obj))
        {
            if(obj.attackableLayer == attackableLayer)
                obj.TakeDamage(-power);
        }
    }
}
