using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ResourceParent : MonoBehaviour, IDamageable
{
    float health = 15;
    float currentHealth;

    [SerializeField] Image healthBarWhite, healthBarRed;

    [SerializeField]
    GameObject canvas;

    [SerializeField]
    Material whiteMaterial;

    Material[] defaultMaterials;
    Material[] whiteMaterials;

    [SerializeField]
    MeshRenderer meshRenderer;

    float _fillAmount=1;

    [SerializeField]
    GameObject collactablePrefab;



    enum DieType
    {
        Rotation,
        Scale
    }

    [SerializeField]
    DieType dieType;

    private void Start()
    {
        currentHealth = health;
        defaultMaterials = meshRenderer.materials;
        whiteMaterials = new Material[meshRenderer.materials.Length];
        collactablePrefab.transform.parent = null;
        for (int i = 0; i < whiteMaterials.Length; i++)
        {
            whiteMaterials[i] = whiteMaterial;
        }


    }




    public void Die()
    {
        switch (dieType)
        {
            case DieType.Rotation:
            
                transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, -90f), 0.5f).OnComplete(() =>
            {
                
                collactablePrefab.SetActive(true);
                Destroy(gameObject);
            });

                break;
            case DieType.Scale:
               
                collactablePrefab.SetActive(true);
                transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {

                Destroy(gameObject);
            });

                break;

            default:
                break;
        }

    }

    public void TakeDamage(int damage)
    {
        canvas.SetActive(true);

        currentHealth -= damage;

        transform.DOShakePosition(0.25f, 0.5f, 10, 90f, false, true);

        DOTween.To(() => _fillAmount, x => _fillAmount = x, currentHealth / health, 1.25f).OnUpdate(() =>
        {
            healthBarWhite.fillAmount = _fillAmount;
        });

        healthBarRed.fillAmount = currentHealth / health;

        StartCoroutine(ChangeMaterial());


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ChangeMaterial()
    {
        meshRenderer.materials = whiteMaterials;
        yield return new WaitForSeconds(0.1f);
        meshRenderer.materials = defaultMaterials;
    }
}
