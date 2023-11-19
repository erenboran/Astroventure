using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject_Done : MonoBehaviour
{

    // public PlacedObjectTypeSO objectTypeSO;
    public GridObject gridObject;
    private GridXZ<GridObject> grid;

    public PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;

    [SerializeField]
    GameObject build;

    [SerializeField]
    int state;



    // Üst 1 
    // Sag 2
    // Alt 4
    // Sol 8

    // 
    public void Control(bool isMe)
    {

        if (placedObjectTypeSO.isNeedControl)
        {


            int newState = 0;
            GridObject newGridobject = null;


            if (gridObject.x - 1 >= 0)
            {
                newGridobject = GetNode(gridObject.x - 1, gridObject.y);


                if (newGridobject.placedObject != null && newGridobject.placedObject.placedObjectTypeSO.name.Equals(placedObjectTypeSO.name))
                {

                    newState += 8;
                    if (isMe)
                    {
                        newGridobject.placedObject.Control(false);

                    }

                }


            }

            if (gridObject.x + 1 < grid.GetWidth())
            {

                newGridobject = GetNode(gridObject.x + 1, gridObject.y);


                if (newGridobject.placedObject != null && newGridobject.placedObject.placedObjectTypeSO.name.Equals(placedObjectTypeSO.name))
                {
                    newState += 2;
                    if (isMe)
                    {
                        newGridobject.placedObject.Control(false);

                    }

                }

            }

            if (gridObject.y - 1 >= 0)
            {
                newGridobject = GetNode(gridObject.x, gridObject.y - 1);
                //Debug.Log("GridObject " + gridObject.x + "." + gridObject.y + "NewGridObject" + newGridobject.placedObject);


                if (newGridobject.placedObject != null && newGridobject.placedObject.placedObjectTypeSO.name.Equals(placedObjectTypeSO.name))
                {
                    newState += 4;
                    if (isMe)
                    {
                        newGridobject.placedObject.Control(false);

                    }

                }

            }

            if (gridObject.y + 1 < grid.GetHeight())
            {
                newGridobject = GetNode(gridObject.x, gridObject.y + 1);

                if (newGridobject.placedObject != null && newGridobject.placedObject.placedObjectTypeSO.name.Equals(placedObjectTypeSO.name))
                {


                    newState += 1;

                    if (isMe)
                    {
                        newGridobject.placedObject.Control(false);

                    }

                }

            }



            // Debug.Log("GridObject " + gridObject.x + "." + gridObject.y + " State: " + state + " NewState: " + newState + " isMe " + isMe);

            if (state != newState)
            {
                state = newState;
                GameObject newBuild = Instantiate(placedObjectTypeSO.buildVariations[state], build.transform.position, Quaternion.identity);
                Destroy(build);
                build = newBuild;


            }



        }





    }

    GridObject GetNode(int x, int y)
    {
        GridObject gridObject = grid.GetGridObject(x, y);

        return gridObject;
    }

    public static PlacedObject_Done Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO, GridObject gridObject)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject_Done placedObject = placedObjectTransform.GetComponent<PlacedObject_Done>();

        placedObject.Setup(placedObjectTypeSO, origin, dir, gridObject);
        
        return placedObject;
    }

    public static Transform CreateVisual(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.visual, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));


        return placedObjectTransform;
    }




    public void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir, GridObject gridObject)
    {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
        this.dir = dir;
        this.grid = GridBuildingSystem3D.Instance.grid;
        this.gridObject = gridObject;
        this.gridObject.placedObject = this;


        if (placedObjectTypeSO.isNeedControl)
        {
            Control(true);
        }

    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return placedObjectTypeSO.nameString;
    }

}
