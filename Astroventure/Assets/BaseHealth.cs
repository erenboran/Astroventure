using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int health = 100;

    [SerializeField] TMP_Text healthText;

    [SerializeField]
    GameObject gameOverPanel;


    public void Die()
    {

        gameOverPanel.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        healthText.text = health.ToString();
    }
}
