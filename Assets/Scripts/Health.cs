using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 3f;
    public float healthRegenRate = 0.1f;
    public Slider healthBar;
    public GameObject playerDeathCanvas;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 3)
        {
            health += healthRegenRate * Time.deltaTime;
            if (health > 3)
            {
                health = 3;
            }
        }
        // Update the health bar if it exists
        if (healthBar != null)
        {
            healthBar.value = health / 3f;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Ensure health does not drop below zero
        if (health <= 0)
        {
            Zombies zombie = GetComponent<Zombies>();
            if (zombie != null)
            {
                zombie.Die();
            }
        }
        if (this.gameObject.CompareTag("Player") && health <= 0)
        {
            // Prefer centralized DeathManager if available
            if (DeathManager.Instance != null)
            {
                DeathManager.Instance.OnPlayerDeath();
            }
            else
            {
                // Fallback: show the player death canvas (ensure CanvasGroup exists)
                if (playerDeathCanvas != null)
                {
                    CanvasGroup cg = playerDeathCanvas.GetComponent<CanvasGroup>();
                    if (cg == null) cg = playerDeathCanvas.AddComponent<CanvasGroup>();

                    if (cg != null)
                    {
                        cg.interactable = true;
                        cg.blocksRaycasts = true;
                    }
                    playerDeathCanvas.SetActive(true);
                }
                Time.timeScale = 0f;
            }
        }
    }
}