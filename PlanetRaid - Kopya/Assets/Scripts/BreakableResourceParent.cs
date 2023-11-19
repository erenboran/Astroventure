using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class BreakableResourceParent : MonoBehaviour, IDamageable
{

    float health = 15;
    float currentHealth;

    [SerializeField] Image healthBarWhite, healthBarRed;

    [SerializeField]
    GameObject canvas;
    float _fillAmount;

    Transform playerTransform;

    [SerializeField]
    List<ICollectable> collectables;

    [SerializeField]
    GameObject collactablePrefab;

    private void Start()
    {
        currentHealth = health;
        playerTransform = PlayerController.Instance.collectPoint;
        collectables = new List<ICollectable>(collactablePrefab.GetComponentsInChildren<ICollectable>());

    }




    public void Die()
    {
        collactablePrefab.transform.parent = null;

        collactablePrefab.SetActive(true);
        
        foreach (var item in collectables)
        {
            item.Collect(playerTransform);
        }


        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
         {
            Destroy(gameObject);
         });
    }

    public void TakeDamage(int damage)
    {
        canvas.SetActive(true);

        currentHealth -= damage;

        transform.DOShakePosition(0.5f, 0.5f, 10, 90f, false, true);

        DOTween.To(() => _fillAmount, x => _fillAmount = x, currentHealth / health, 0.75f).OnUpdate(() =>
        {
            healthBarWhite.fillAmount = _fillAmount;
        });

        healthBarRed.fillAmount = currentHealth / health;


        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
