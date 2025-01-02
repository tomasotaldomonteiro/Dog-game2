using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    
    private float explosionRadius = 3f;
    //[SerializeField] private string targetLayerName = "Rat"; 
    [SerializeField] private string barrelLayerName = "Barrel"; 

    private Animator animator;
    private SpriteRenderer sprite;
    private Collider2D explosionCollider; 

    private bool isExploding = false;

    private void Awake(){
        
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        explosionCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!isExploding) {
           
            Explode();
        }
    }

    public void Explode() {
        
        if (isExploding) return; 
        isExploding = true;
        explosionCollider.enabled = false;
        
        animator.SetTrigger("Explode");

        GetComponent<ExplosionPlayer>()?.triggerExplosion();

        TriggerNearbyExplosions();

        StartCoroutine(DestroyAfterAnimation());
    }

    private void TriggerNearbyExplosions() {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in colliders) {
            
            ExplosionDetection detection = collider.GetComponent<ExplosionDetection>();
            if (detection != null)
            {
                detection.OnExplosion();
            }
            
            if (collider.gameObject.layer == LayerMask.NameToLayer(barrelLayerName)){                        // Trigger other barrels
            
                Explosion otherBarrel = collider.GetComponent<Explosion>();
                
                if (otherBarrel != null && !otherBarrel.isExploding){                                     // Ensure the barrel hasn't already exploded
                    otherBarrel.Explode();                                                                 // Trigger explosion on the other barrel
                }
            }
        }
    }

    private IEnumerator DestroyAfterAnimation(){
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected(){
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
