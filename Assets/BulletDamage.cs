using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float power = 10;
    private void OnParticleCollision(GameObject other)
    {
        print("hitting");
        if(other.TryGetComponent<Health>(out Health obj))
        {
            obj.TakeDamage(-power);
        }
    }
}
