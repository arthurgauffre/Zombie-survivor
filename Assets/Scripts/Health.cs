using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 3;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health bar if it exists
        if (healthBar != null)
        {
            healthBar.value = health / 3;
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
    }
}