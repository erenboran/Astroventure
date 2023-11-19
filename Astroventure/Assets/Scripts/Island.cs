using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Island
{
    public IslandType type;

    public Vector2 startPosition, endPosition;

    public GridObject[,] gridObjects;

    int islandAreaLenght = 0;




    public bool ControlisInIsland(Vector2 position)
    {
        if (position.x >= startPosition.x && position.x <= endPosition.x)
        {
            if (position.y >= startPosition.y && position.y <= endPosition.y)
            {
                return true;
            }
        }

        return false;
    }


    public Island(IslandType type, Vector2 startPosition, Vector2 endPosition)
    {
        this.type = type;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        islandAreaLenght = (int)(endPosition.x - startPosition.x) * (int)(endPosition.y - startPosition.y);
        gridObjects = new GridObject[(int)(endPosition.x - startPosition.x), (int)(endPosition.y - startPosition.y)];

        for (int i = 0; i < (int)(endPosition.x - startPosition.x); i++)
        {
            for (int j = 0; j < (int)(endPosition.y - startPosition.y); j++)
            {
                gridObjects[i, j] = GridBuildingSystem3D.Instance.grid.GetGridObject(i + (int)startPosition.x, j + (int)startPosition.y);
                
            }
        }


    }



}
