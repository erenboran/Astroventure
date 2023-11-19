using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private IslandType[] islandTypes;

    [SerializeField]
    private List<Island> islands = new List<Island>();

    [SerializeField]
    private SpriteRenderer islandAreaSpritePrefab;

    private int mapWidth = 500;
    private int mapHeight = 500;

    Vector2 startPoint = new Vector2(200, 200);
    Vector2 endPoint = new Vector2(300, 300);

    int totalGroundCount = 0;

    int islandSize = 25;

    private void Start()
    {
        CreateRandomIslands();
    }



    void CreateRandomIslands()
    {
        // Main island [200x200 300x300]

        // Right side
        Vector2 _newIslanStartPoint = new Vector2(300, 200);
        Vector2 _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);

        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[1]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);


            }
            _newIslanStartPoint = new Vector2(300, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);
        }

        // Left Side

        _newIslanStartPoint = new Vector2(0, 200);
        _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);

        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[3]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);
            }

            _newIslanStartPoint = new Vector2(0, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);
        }




        // Top Side

        _newIslanStartPoint = new Vector2(200, 300);
        _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[4]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);
            }

            _newIslanStartPoint = new Vector2(200, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);
        }






        // Down Side

        _newIslanStartPoint = new Vector2(200, 0);
        _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[3]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);
            }

            _newIslanStartPoint = new Vector2(200, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);
        }



        // Top Right
        _newIslanStartPoint = new Vector2(300, 300);
        _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);


        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int randomIndex = Random.Range(0, 2) == 0 ? 1 : 4;
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[randomIndex]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);

            }

            _newIslanStartPoint = new Vector2(300, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);



        }

        // Top Left

        _newIslanStartPoint = new Vector2(0, 300);
        _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int randomIndex = Random.Range(3, 5);
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[randomIndex]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);

            }

            _newIslanStartPoint = new Vector2(0, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);



        }





        // Down Right


        _newIslanStartPoint = new Vector2(300, 0);
        _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int randomIndex = Random.Range(1, 3);
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[randomIndex]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);

            }

            _newIslanStartPoint = new Vector2(300, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);



        }






        // Down Left


        _newIslanStartPoint = new Vector2(0, 0);
        _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int randomIndex = Random.Range(2, 4);
                CreateIsland(_newIslanStartPoint, _newIslanEndPoint, islandTypes[randomIndex]);
                _newIslanStartPoint += new Vector2(islandSize, 0);
                _newIslanEndPoint += new Vector2(islandSize, 0);

            }

            _newIslanStartPoint = new Vector2(0, _newIslanStartPoint.y + islandSize);
            _newIslanEndPoint = _newIslanStartPoint + new Vector2(islandSize, islandSize);



        }






    }

    Island CreateIsland(Vector2 startPoint, Vector2 endPoint, IslandType type)
    {
        Island island = new Island(type, startPoint, endPoint);


        for (int x = 0; x < island.gridObjects.GetLength(0); x++)
        {
            for (int y = 0; y < island.gridObjects.GetLength(1); y++)
            {
                Vector3 groundSpawnPoint = new Vector3(island.gridObjects[x, y].x, 0, island.gridObjects[x, y].y);

                GameObject newGround = Instantiate(island.type.groundPrefab, groundSpawnPoint, Quaternion.identity);

                newGround.name = "Ground " + totalGroundCount;

                totalGroundCount++;

                newGround.transform.parent = transform;

               // newGround.SetActive(false);

            }


        }



        // SpriteRenderer islandAreaSprite = Instantiate(islandAreaSpritePrefab);
        // islandAreaSprite.transform.localScale = new Vector3(island.endPosition.x - island.startPosition.x, island.endPosition.y - island.startPosition.y, 1);
        // islandAreaSprite.transform.position = new Vector3((island.startPosition.x + island.endPosition.x) / 2, 0, (island.startPosition.y + island.endPosition.y) / 2);
        // islandAreaSprite.color = island.type.color;
        islands.Add(island);
        return island;

    }

}
