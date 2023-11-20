using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridBuildingSystem3D : MonoBehaviour
{

    public static GridBuildingSystem3D Instance { get; private set; }



    [SerializeField]
    GameObject inventory;
    public GridXZ<GridObject> grid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    [SerializeField]
    public PlacedObjectTypeSO placedObjectTypeSO;
    [SerializeField]
    private PlacedObjectTypeSO.Dir dir;
    private GridObject startNode, endNode;
    [SerializeField]


    private void Awake()
    {
        Instance = this;

        int gridWidth = 500;
        int gridHeight = 500;
        float cellSize = 1;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));

        placedObjectTypeSO = null;// placedObjectTypeSOList[0];
    }

    private void OnEnable()
    {
        GameEvents.Instance.OnObjectPlaced += CreateNewBuilding;
        GameEvents.Instance.OnNewBuildingSelected += SelectNewBuild;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnObjectPlaced -= CreateNewBuilding;
        GameEvents.Instance.OnNewBuildingSelected -= SelectNewBuild;

    }




    public void CreateNewBuilding(GridObject gridObject, PlacedObjectTypeSO _placedObjectTypeSO)
    {
        placedObjectTypeSO = _placedObjectTypeSO;



        if (!GameEvents.Instance.OnResourceControl.Invoke(placedObjectTypeSO.buildResources))
        {


            GameEvents.Instance.OnWarningMessage?.Invoke("Yeterli Kaynak Yok!");

            placedObjectTypeSO = null;

            RefreshSelectedObjectType();
            return;
        }

        if (placedObjectTypeSO.isMiner)
        {
            if (!GameEvents.Instance.OnMinerBuildControl.Invoke())
            {


                placedObjectTypeSO = null;
                RefreshSelectedObjectType();
                GameEvents.Instance.OnWarningMessage?.Invoke("Taşınabilir Maden Sondajı Bir Maden Kaynağının Üzerine Kurulmalı!!");
                return;
            }


        }

        if ((gridObject.CanBuild() == 0) || (gridObject.CanBuild() == 1 && (placedObjectTypeSO.isUnderground)) || (gridObject.CanBuild() == 2 && (!placedObjectTypeSO.isUnderground)))
        {

            if (placedObjectTypeSO.isStackable)
            {
                dir = gridObject.dir;

            }

            Vector2Int placedObjectOrigin = new Vector2Int(gridObject.x, gridObject.y);
            //  placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO, gridObject);
            //placedObject.gridObject = gridObject;
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }




            GameEvents.Instance.OnResourceUsed?.Invoke(placedObjectTypeSO.buildResources);



        }

        placedObjectTypeSO = null;

        RefreshSelectedObjectType();


    }

    void SelectNewBuild(PlacedObjectTypeSO _newPlacedObjectType)
    {
        placedObjectTypeSO = _newPlacedObjectType;
        GameEvents.Instance.OnBuildMenuClosed?.Invoke();
        RefreshSelectedObjectType();
    }

    void BuildObject()
    {

        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

        // Test Can Build
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canBuild = true;

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            GridObject gridObject1 = grid.GetGridObject(gridPosition.x, gridPosition.y);

            if ((gridObject1.CanBuild() == 0) || (gridObject1.CanBuild() == 1 && (placedObjectTypeSO.isUnderground)) || (gridObject1.CanBuild() == 2 && (!placedObjectTypeSO.isUnderground)))
            {

            }
            else
            {

                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            GridObject gridObject = grid.GetGridObject(x, z);

            GameEvents.Instance.OnGridSelected?.Invoke(placedObjectTypeSO, gridObject);

            //DeselectObjectType();
        }

        else
        {
            // Cannot build here
            //  Utils.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
        }

    }



    private void Update()
    {




        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null)
        {
            BuildObject();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
            }


        }

        if (!inventory.activeSelf)
        {
           
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                placedObjectTypeSO = null; GameEvents.Instance.OnBuildMenuClosed?.Invoke();
                RefreshSelectedObjectType();
            }

           
        }







    }

    public List<GridObject> GetActiveGridObjectList()
    {
        List<GridObject> gridObjectList = new List<GridObject>();

        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

        grid.GetXZ(mousePosition, out int x, out int z);

        Vector2Int placedObjectOrigin = new Vector2Int(x, z);

        placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            gridObjectList.Add(grid.GetGridObject(gridPosition.x, gridPosition.y));
        }

        return gridObjectList;

    }

    public PlacedObjectTypeSO.Dir GetDirection()
    {
        return dir;
    }

    private void DeselectObjectType()
    {
        placedObjectTypeSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        GameEvents.Instance.OnSelectedChanged?.Invoke();
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null)
        {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObjectTypeSO != null)
        {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO()
    {
        return placedObjectTypeSO;
    }

}

[System.Serializable]
public class GridObject
{
    private GridXZ<GridObject> grid;
    public int x;
    public int y;
    public PlacedObject_Done placedObject;

    public PlacedObjectTypeSO.Dir dir;

    public int gCost;
    public int hCost;
    public int fCost;
    public int comeDirection;
    public int turnCost;
    public GridObject cameFromNode;





    public GridObject(GridXZ<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        placedObject = null;

    }


    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + ", " + y + "\n" + placedObject;
    }

    public void SetPlacedObject(PlacedObject_Done placedObject)
    {
        this.placedObject = placedObject;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearPlacedObject()
    {
        placedObject = null;
        grid.TriggerGridObjectChanged(x, y);
    }

    public PlacedObject_Done GetPlacedObject()
    {
        return placedObject;
    }

    public int CanBuild()
    {

        // 0  kare boş her şey kurulabilir
        // 1  kare dolu Underground olan kurulabilir
        // 2  kare dolu Underground olmayan kurulabilir

        if (placedObject == null)
        {
            return 0;
        }
        else
        {
            if (placedObject.placedObjectTypeSO.isUnderground)
            {
                return 2;

            }

            else
            {
                return 1;

            }

        }

    }

}

//public class Node
//{
//    public int x, z, gCost, hCost,fCost;
//    public PlacedObjectTypeSO.Dir dir;
//    public Node priviusNode;
//    public bool canBuild;

//    public Node(int x, int z)
//    {
//        this.x = x;
//        this.z = z;
//    }
//}
