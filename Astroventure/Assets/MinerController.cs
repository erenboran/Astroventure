using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerController : MonoBehaviour
{
    public bool canBuild = false;

    [SerializeField]
    MeshRenderer[] meshRenderers;

    List<List<Material>> defaultMaterials = new List<List<Material>>();

    [SerializeField]
    Material cantBuildMaterial;

    private void Start() {
        
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            List<Material> materials = new List<Material>();

            foreach (Material material in meshRenderer.materials)
            {
                materials.Add(material);
            }

            defaultMaterials.Add(materials);
        }

        ChangeMode();
    }

    void ChangeMode()
    {
        if (canBuild)
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].materials = defaultMaterials[i].ToArray();
            }
        }
        else
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                Material[] materials = new Material[meshRenderers[i].materials.Length];

                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j] = cantBuildMaterial;
                }

                meshRenderers[i].materials = materials;
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mine")
        {
            canBuild = true;
            ChangeMode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mine")
        {
            canBuild = false;
            ChangeMode();
        }
    }
}
