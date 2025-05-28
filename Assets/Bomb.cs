using System.Collections;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public TextMeshPro countdownText;
    public float radius = 5f;
    public string tagToExplode = "gate";
    IEnumerator Start()
    {
        for (int i = 5; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            countdownText.text = i.ToString();
        }
        Explode();
    }

    void Explode()
    {
        // Instantiate the explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        // Find all objects with the specified tag within the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tagToExplode))
            {
                // Destroy the object
                Destroy(collider.gameObject);
            }
        }
        // Destroy the bomb itself
        Destroy(gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
