using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Transform turret, target;
    public ParticleSystem destroyParticle;

    public ParticleSystem shootingParticle;
    public float takeAimDuration = 1f;
    public bool isDead;
    public void TakeAim()
    {
        StartCoroutine(TakingAim());
    }

    IEnumerator TakingAim()
    {
        while (!isDead)
        {
            turret.DODynamicLookAt(target.position, takeAimDuration);
            yield return new WaitForSeconds(takeAimDuration + 1);
            shootingParticle.Play();
            yield return new WaitForSeconds(1);
        }
    }
    

    public void DestroyOBJ()
    {
        destroyParticle.Play();
        destroyParticle.transform.parent = null;
        isDead = true;
        StopAllCoroutines();
    }
}
