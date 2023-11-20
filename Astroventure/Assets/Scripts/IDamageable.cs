using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{

    void TakeDamage(int damage);
    void Die();
   
}

public interface IEnemy 
{

    void TakeDamage(int damage);
    void Die();
   
}
