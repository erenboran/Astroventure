using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSetter : MonoBehaviour
{

    public GridXZ<GridObject> grid;

    [SerializeField] PlacedObjectTypeSO placedObjectTypeSO;
    [SerializeField] PlacedObject_Done placedObject;

    private void Start()
    {
        grid = GridBuildingSystem3D.Instance.grid;

        BuildObject();
    }

    void BuildObject()
    {
        grid.GetXZ(transform.position, out int x, out int z);

        Vector2Int placedObjectOrigin = new Vector2Int(x, z);

        placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, PlacedObjectTypeSO.Dir.Up);
        bool canBuild = true;

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            GridObject gridObject1 = grid.GetGridObject(gridPosition.x, gridPosition.y);
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(PlacedObjectTypeSO.Dir.Up);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

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

            placedObject.Setup(placedObjectTypeSO, placedObjectOrigin, PlacedObjectTypeSO.Dir.Up, gridObject);

        }

        Destroy(this);
        

    }

}
