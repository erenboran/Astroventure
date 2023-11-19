using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

   public BuildResources buildResources;


   [SerializeField]
   List<Resource> resources;

   void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      else
      {
         Destroy(gameObject);
      }

      resources.Add(new Resource());
   }

   private void OnEnable()
   {
      GameEvents.Instance.OnResourceControl += ControlResource;
      GameEvents.Instance.OnResourceUsed += UseResource;
   }

   private void OnDisable()
   {
      GameEvents.Instance.OnResourceControl -= ControlResource;
      GameEvents.Instance.OnResourceUsed -= UseResource;
   }

   void UseResource(BuildResources buildResources)
   {
      this.buildResources.resource1 -= buildResources.resource1;
      this.buildResources.resource2 -= buildResources.resource2;
   }

   bool ControlResource(BuildResources buildResources)
   {
      if (buildResources.resource1 <= this.buildResources.resource1 && buildResources.resource2 <= this.buildResources.resource2)
      {
         return true;
      }

      else
      {
         return false;
      }


   }


}
