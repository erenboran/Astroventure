using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private List<Transform> visuals = new List<Transform>();
    private Transform _visual;

    [SerializeField]
    Material cantBuildMaterial;
    GridObject _currentGridObject;
    List<GridObject> _gridObjects;

    GridBuildingSystem3D gridBuildingSystem3D;
    GridXZ<GridObject> grid;

    Vector3 targetPosition;

    List<Material[]> defaultMaterialsList = new List<Material[]>();
    Material[] _cantBuildMaterials;

    private void Start()
    {
        gridBuildingSystem3D = GridBuildingSystem3D.Instance;
        grid = gridBuildingSystem3D.grid;


        GameEvents.Instance.OnSelectedChanged += RefreshVisual;
        GameEvents.Instance.OnSelectedChangedStackable += ClearVisuals;
        GameEvents.Instance.OnPathFound += CreateVisual;
        GameEvents.Instance.OnMinerBuildControl += ControlMiner;

    }

    private void OnDisable()
    {
        GameEvents.Instance.OnSelectedChanged -= RefreshVisual;
        GameEvents.Instance.OnSelectedChangedStackable -= ClearVisuals;
        GameEvents.Instance.OnPathFound -= CreateVisual;
        GameEvents.Instance.OnMinerBuildControl -= ControlMiner;

    }

    bool ControlMiner()
    {
        MinerController minerController = _visual.GetComponentInChildren<MinerController>();

        if (minerController.canBuild)
        {
            return true;
        }
        else
        {
            return false;
        }

    }



    private void Update()
    {
        targetPosition = gridBuildingSystem3D.GetMouseWorldSnappedPosition();
        targetPosition.y = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, gridBuildingSystem3D.GetPlacedObjectRotation(), Time.deltaTime * 15f);

        if (_visual != null)
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

            grid.GetXZ(mousePosition, out int x, out int z);


            if (_currentGridObject != grid.GetGridObject(x, z))
            {
                _currentGridObject = grid.GetGridObject(x, z);

                _gridObjects = gridBuildingSystem3D.GetActiveGridObjectList();

                ChangeMaterial(_gridObjects);
            }

        }


    }



    void ChangeMaterial(List<GridObject> gridObjects)
    {

        for (int i = 0; i < _cantBuildMaterials.Length; i++)
        {
            _cantBuildMaterials[i] = cantBuildMaterial;
        }
        PlacedObjectTypeSO placedObjectTypeSO = gridBuildingSystem3D.GetPlacedObjectTypeSO();

        bool canBuild = true;

        foreach (GridObject gridObject in gridObjects)
        {
            int canBuildResult = gridObject.CanBuild();

            if ((canBuildResult == 0) ||
                (canBuildResult == 1 && placedObjectTypeSO.isUnderground) ||
                (canBuildResult == 2 && !placedObjectTypeSO.isUnderground))
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
            BuildMaterial buildMaterials = _visual.GetComponent<BuildMaterial>();


            for (int i = 0; i < buildMaterials.meshRenderers.Length; i++)
            {
                buildMaterials.meshRenderers[i].materials = defaultMaterialsList[i];
            }
        }

        else
        {
            foreach (MeshRenderer _visualMeshrenderer in _visual.GetComponent<BuildMaterial>().meshRenderers)
            {
                _visualMeshrenderer.materials = _cantBuildMaterials;
            }
        }

    }



    private void ClearVisuals()
    {
        for (int i = 0; i < visuals.Count; i++)
        {
            Destroy(visuals[i].gameObject);
        }

        visuals.Clear();

    }

    private void RefreshVisual()
    {


        if (_visual != null)
        {
            Destroy(_visual.gameObject);

        }
        ClearVisuals();

        PlacedObjectTypeSO placedObjectTypeSO = gridBuildingSystem3D.GetPlacedObjectTypeSO();

        if (placedObjectTypeSO != null)
        {
            Transform _newVisual = InstantiateVisual(placedObjectTypeSO);

            BuildMaterial buildMaterials = _newVisual.GetComponent<BuildMaterial>();

            foreach (MeshRenderer _visualMeshrenderer in buildMaterials.meshRenderers)
            {
                defaultMaterialsList.Add(_visualMeshrenderer.materials);
            }

            _cantBuildMaterials = new Material[6];

            for (int i = 0; i < _cantBuildMaterials.Length; i++)
            {
                _cantBuildMaterials[i] = cantBuildMaterial;
            }

            if (placedObjectTypeSO.isStackable)
            {
                visuals.Add(_newVisual);

            }

            else
            {
                _visual = _newVisual;

            }



        }
    }

    Transform InstantiateVisual(PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform visual;
        visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
        visual.parent = transform;
        visual.localPosition = Vector3.zero;
        visual.localEulerAngles = Vector3.zero;
        return visual;

    }



    private void CreateVisual(GridObject gridObject)
    {

        PlacedObjectTypeSO placedObjectTypeSO = gridBuildingSystem3D.GetPlacedObjectTypeSO();

        if (placedObjectTypeSO != null)
        {
            GridXZ<GridObject> grid = gridBuildingSystem3D.grid;
            Vector2Int placedObjectOrigin = new Vector2Int(gridObject.x, gridObject.y);

            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(gridObject.dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            Transform visual = PlacedObject_Done.CreateVisual(placedObjectWorldPosition, placedObjectOrigin, gridObject.dir, placedObjectTypeSO);

            bool canBuildCondition = (gridObject.CanBuild() == 0) || (gridObject.CanBuild() == 1 && placedObjectTypeSO.isUnderground) || (gridObject.CanBuild() == 2 && !placedObjectTypeSO.isUnderground);

            BuildMaterial buildMaterials = visual.GetComponent<BuildMaterial>();

            for(int i = 0; i < buildMaterials.meshRenderers.Length; i++)
            {
                buildMaterials.meshRenderers[i].materials = canBuildCondition ? defaultMaterialsList[i] : _cantBuildMaterials;
            }

            visuals.Add(visual.transform);
        }

    }



}

