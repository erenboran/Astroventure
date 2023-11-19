using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class EnemySO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int health;
    public int damage;
    public int speed;
    public int reward;
    public int cost;
    
    public int level;

    public float healthMultiplierByLevel;
    public float damageMultiplierByLevel;
    public float speedMultiplierByLevel;
    public float rewardMultiplierByLevel;
    public float costMultiplierByLevel;

    public EnemySO ShallowCopy()
    {
        return (EnemySO)this.MemberwiseClone();
    }

    public void LevelUp()
    {
        level++;
        health = (int)(health * healthMultiplierByLevel);
        damage = (int)(damage * damageMultiplierByLevel);
        speed = (int)(speed * speedMultiplierByLevel);
        reward = (int)(reward * rewardMultiplierByLevel);
        cost = (int)(cost * costMultiplierByLevel);
    }
}
