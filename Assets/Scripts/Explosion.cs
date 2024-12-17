using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float explosionRadius = 5f; 
    [SerializeField] public GameObject explosionEffect; 
    [SerializeField] public string targetLayerName = "Rat";
    public Animator animator;
    public GameObject sprite;
    
    private bool isExploding = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploding) // Prevent multiple explosions
        {
            Explode();
        }
    }
    
    
    private void Explode()
    {
        isExploding = true; 
        

        // Play the animation
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        DestroyAfterAnimation();
    }

    private void DestroyAfterAnimation()
    {
        
        // Damage nearby objects
        Collider2D[] colliders = Physics2D.OverlapCircleAll
            (
                transform.position, 
                explosionRadius, 
                1 << LayerMask.NameToLayer(targetLayerName)
            );
        
        Debug.Log(LayerMask.NameToLayer(targetLayerName));
        Debug.Log(1 << LayerMask.NameToLayer(targetLayerName));

        // We Get The Numbered layer with
        // LayerMask.NameToLayer(targetLayerName);
        
        // But the above needs a bitmasked version of it so we bit shift by that value
        // 1 << LayerMask.NameToLayer(targetLayerName);
        
        // This is just in case you want all the differnt layers EXCEPT the declared layer
        //~(1 << LayerMask.NameToLayer(targetLayerName));
        
        foreach (Collider2D collider in colliders)
        {
            Destroy(collider.gameObject);
        }
        
    }

    private void Killyourself() //kill the barrel
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void DisableBarelSprite()
    {            
        sprite.SetActive(false);
    }
}