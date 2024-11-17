using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float explosionRadius = 5f; 
    [SerializeField] public GameObject explosionEffect; 
    [SerializeField] public string targetLayerName = "Rat";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        int targetLayer = LayerMask.NameToLayer(targetLayerName);
        
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == targetLayer)
            {
                Destroy(collider.gameObject);
            }
        }
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}