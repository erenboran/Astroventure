using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    // Delegates;
    public delegate void OnSelectedChangedDelegate();
    public delegate void GridSelectedDelagate(PlacedObjectTypeSO objectTypeSO, GridObject gridObject);
    public delegate void PathFoundDelagate(GridObject gridObject);
    public delegate void ObjectPlacedDelegate(GridObject objectTypeSO, PlacedObjectTypeSO placedObjectTypeSO);
    public delegate void ResourceChangedDelegate(Enums.ResourceTypes resourceType, int count);

    public delegate void OnNewBuildingSelectedDelegate(PlacedObjectTypeSO objectTypeSO);

    public delegate bool OnResourceControlDelegate(BuildResources buildResources);

    public delegate void OnBuildMenuOpenedDelegate(bool isOpen);

    public delegate void OnToolTipActivatedDelegate(PlacedObjectTypeSO objectTypeSO,bool isActive);

    public delegate void OnResourceUsedDelegate(BuildResources buildResources);

        public delegate void OnOutOfResourcesDelegate();


    public delegate void OnBuildMenuClosedDelegate();

    // public delegate void ControlBuildings();


    // Delegate Instance
    public GridSelectedDelagate OnGridSelected;

    public OnNewBuildingSelectedDelegate OnNewBuildingSelected;

    public OnResourceUsedDelegate OnResourceUsed;

    public OnOutOfResourcesDelegate OnOutOfResources;

    public OnToolTipActivatedDelegate OnToolTipActivated;
    
    public OnResourceControlDelegate OnResourceControl;
    public OnBuildMenuOpenedDelegate OnBuildMenuOpened;
    public PathFoundDelagate OnPathFound;
    public ObjectPlacedDelegate OnObjectPlaced;
    // public ControlBuildings OnControlBuildings;
    public OnSelectedChangedDelegate OnSelectedChangedStackable;
    public OnSelectedChangedDelegate OnSelectedChanged;
    public ResourceChangedDelegate OnResourceChanged;

    public OnBuildMenuClosedDelegate OnBuildMenuClosed;




    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            OnResourceChanged?.Invoke(Enums.ResourceTypes.Iron, 1);
        }
    }



}
