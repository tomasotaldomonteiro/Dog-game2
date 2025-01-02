using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownButton : MonoBehaviour
{
    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownTime = 3f;
    [SerializeField] private Image imageCooldown; 
    [SerializeField] private TMP_Text textCooldown; 
    private bool isCooldown = false;
    private float cooldownTimer;

    [Header("References")]
    [SerializeField] private PlayerController playerController; 

    private void Start() {
      
        imageCooldown.fillAmount = 0f;
        textCooldown.gameObject.SetActive(false);
    }

    private void Update() {
        
        if (SceneStartSpawning.IsSpawning) return;
        
        if (Input.GetKeyDown(KeyCode.Q) && !isCooldown && playerController.ThrowStone()) {
          
            isCooldown = true;
            cooldownTimer = cooldownTime;
            textCooldown.gameObject.SetActive(true);
            imageCooldown.fillAmount = 1f;
        }

      
        if (isCooldown) {
            ApplyCooldown();
        }
    }

    private void ApplyCooldown() {
       
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f) {
          
            isCooldown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0f;
            
        }else {
            
            textCooldown.text = Mathf.CeilToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }
    }
}