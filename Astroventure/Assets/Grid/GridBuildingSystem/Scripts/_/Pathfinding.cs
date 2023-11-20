using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding : MonoBehaviour
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private GridXZ<GridObject> grid;
    [SerializeField]
    private List<GridObject> openList;
    [SerializeField]
    private List<GridObject> closedList;

    [SerializeField]
    List<GridObject> path = new List<GridObject>();

    PlacedObjectTypeSO placedObjectType;
    PlacedObjectTypeSO.Dir dir;

    GridObject startNode, endNode, selectedNode, pastEndNode;


    int safer;
    bool secondGridSelected;
    bool isWorking;

    private void Start()
    {
        grid = GridBuildingSystem3D.Instance.grid;

    }

    private void OnEnable()
    {
        GameEvents.Instance.OnGridSelected += CreateNewBuilding;
        GameEvents.Instance.OnBuildMenuClosed += ClearPath;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnGridSelected -= CreateNewBuilding;
        GameEvents.Instance.OnBuildMenuClosed -= ClearPath;
    }


    void ClearPath()
    {
        StopAllCoroutines();
        isWorking = false;


    }


    IEnumerator CreatingStackableBuild()
    {
        startNode = null;
        selectedNode = null;
        List<GridObject> finalPath = null;
        secondGridSelected = true;
        isWorking = true;

        while (selectedNode == null)
        {


            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

            if (grid.GetGridObject(mousePosition) != null)
            {
                grid.GetXZ(mousePosition, out int x, out int z);

                if (Input.GetMouseButtonDown(0))
                {

                    if (startNode == null)
                    {
                        startNode = grid.gridArray[x, z];

                        endNode = grid.gridArray[x, z];
                    }


                }

                endNode = grid.gridArray[x, z];

                if (endNode != pastEndNode)
                {

                    finalPath = FindPath(startNode, endNode);

                    Vector2Int rotationOffset = placedObjectType.GetRotationOffset(dir);

                    GameEvents.Instance.OnSelectedChangedStackable?.Invoke();

                    // Up 1
                    // Down 2
                    // Left 3
                    // Righ 4

                    if (finalPath != null)
                    {


                        for (int i = 0; i < finalPath.Count - 1; i++)
                        {



                            switch (finalPath[i + 1].comeDirection)
                            {
                                case 1:
                                    finalPath[i].dir = PlacedObjectTypeSO.Dir.Left;
                                    //Debug.Log("1");
                                    break;
                                case 2:
                                    finalPath[i].dir = PlacedObjectTypeSO.Dir.Right;
                                    //Debug.Log("2");
                                    break;
                                case 3:
                                    finalPath[i].dir = PlacedObjectTypeSO.Dir.Up;
                                    //Debug.Log("3");
                                    break;
                                case 4:
                                    finalPath[i].dir = PlacedObjectTypeSO.Dir.Up;
                                    //Debug.Log("4");

                                    break;

                                default:
                                    break;

                            }


                        }

                        if (finalPath.Count > 1)
                        {

                            finalPath[finalPath.Count - 1].dir = finalPath[finalPath.Count - 2].dir;

                        }
                        else
                        {
                            finalPath[0].dir = GridBuildingSystem3D.Instance.GetDirection();
                        }



                        for (int i = 0; i < finalPath.Count; i++)
                        {
                            dir = finalPath[i].dir;
                            GameEvents.Instance.OnPathFound?.Invoke(finalPath[i]);

                        }
                    }



                }






            }

            else
            {
                finalPath = null;
            }





            if (finalPath != null && !secondGridSelected)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < finalPath.Count; i++)
                    {
                        selectedNode = endNode;
                        secondGridSelected = true;
                        GameEvents.Instance.OnObjectPlaced?.Invoke(finalPath[i], placedObjectType);


                    }

                    GameEvents.Instance.OnSelectedChangedStackable?.Invoke();

                }


            }

            pastEndNode = endNode;

            yield return null;

            secondGridSelected = false;

        }

        isWorking = false;


    }




    public void CreateNewBuilding(PlacedObjectTypeSO objectTypeSO, GridObject gridObject)
    {

        placedObjectType = objectTypeSO;

        if (objectTypeSO.isStackable && !isWorking)
        {
            isWorking = true;
            StartCoroutine(CreatingStackableBuild());
        }

        else if (!objectTypeSO.isStackable)
        {
            GameEvents.Instance.OnObjectPlaced?.Invoke(gridObject, placedObjectType);
            GameEvents.Instance.OnSelectedChanged?.Invoke();

        }



    }




    public List<GridObject> FindPath(GridObject startNode, GridObject endNode)
    {

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        openList = new List<GridObject> { startNode };

        closedList = new List<GridObject>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                GridObject pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();


        while (openList.Count > 0)
        {
            GridObject currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                // Reached final node

                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GridObject neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                neighbourNode.cameFromNode = currentNode;

                int turnCost = CalculateDirectionCost(currentNode, neighbourNode);


                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode) + turnCost;

                if (tentativeGCost < neighbourNode.gCost)
                {

                    neighbourNode.gCost = tentativeGCost - turnCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {

                        openList.Add(neighbourNode);

                    }
                }

            }
        }


        // Out of nodes on the openList
        return null;
    }

    int CalculateDirectionCost(GridObject current, GridObject next)
    {
        // Up 1
        // Down 2
        // Left 3
        // Righ 4


        if (current.x > next.x)
        {
            next.comeDirection = 4;
        }

        else if (current.x < next.x)
        {

            next.comeDirection = 3;
        }

        else if (current.y > next.y)
        {
            next.comeDirection = 1;
        }

        else if (current.y < next.y)
        {

            next.comeDirection = 2;

        }


        if (current.comeDirection != next.comeDirection && current.comeDirection != 0)
        {

            next.turnCost = 1;
            return 1;
        }
        next.turnCost = 0;
        return 0;
    }

    private List<GridObject> GetNeighbourList(GridObject currentNode)
    {
        List<GridObject> neighbourList = new List<GridObject>();

        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));

        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

        }

        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));

        }

        if (currentNode.y + 1 < grid.GetHeight())
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        }

        return neighbourList;
    }

    public GridObject GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<GridObject> CalculatePath(GridObject endNode)
    {
        path.Clear();
        path.Add(endNode);

        GridObject currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(GridObject a, GridObject b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        //return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        return xDistance + yDistance;
    }

    private GridObject GetLowestFCostNode(List<GridObject> pathNodeList)
    {
        GridObject lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

}