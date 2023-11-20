using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BuildMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject buildTypeMenu, buildMenu;

    [SerializeField]
    List<BuildType> buildTypes, totorialBuildTypes, allBuildTypes;

    [SerializeField] List<Image> buttonImages;
    [SerializeField] BuildElement buildElementPrefab;

    List<BuildElement> buildElements = new List<BuildElement>();

    List<Tween> currentTweens = new List<Tween>();

    [SerializeField] ToolTip toolTip;
    bool isToolTipActive = false;

    [SerializeField]
    bool isDebug;

    private void OnEnable()
    {
        GameEvents.Instance.OnBuildMenuOpened += OpenBuildTypeMenu;
        GameEvents.Instance.OnToolTipActivated += ToolTip;
        GameEvents.Instance.OnAllBuldingCanBuild += OpenAllBuildTypes;
        GameEvents.Instance.OnToolTipActivatedForTypes += ToolTipForType;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnToolTipActivated -= ToolTip;
        GameEvents.Instance.OnBuildMenuOpened -= OpenBuildTypeMenu;
        GameEvents.Instance.OnAllBuldingCanBuild -= OpenAllBuildTypes;
        GameEvents.Instance.OnToolTipActivatedForTypes -= ToolTipForType;
    }

    private void Start()
    {
        allBuildTypes = buildTypes;

        if (!isDebug)
        {


            buildTypes = totorialBuildTypes;

        }


    }

    void OpenAllBuildTypes()
    {
        buildTypes = allBuildTypes;
        SelectBuildTypeMenu(2);
    }

    void ToolTipForType(string typeName, bool isOn)
    {
        if (isOn)
        {
            toolTip.gameObject.SetActive(true);
            toolTip.itemName.text = typeName;
            toolTip.itemDescription.text = "Tüm binaları görmek için tıkla";
            toolTip.itemCost.text = "";
            isToolTipActive = true;
        }
        else
        {
            toolTip.gameObject.SetActive(false);
            isToolTipActive = false;
        }

    }


    void ToolTip(PlacedObjectTypeSO placedObjectTypeSO, bool isOn)
    {
        if (isOn)
        {
            toolTip.gameObject.SetActive(true);
            toolTip.Init(placedObjectTypeSO);
            isToolTipActive = true;
        }
        else
        {
            toolTip.gameObject.SetActive(false);
            isToolTipActive = false;
        }
    }

    private void Update()
    {
        if (isToolTipActive)
        {
            toolTip.transform.position = Input.mousePosition;
        }
    }

    void OpenBuildTypeMenu(bool isOpen)
    {
        toolTip.gameObject.SetActive(false);

        foreach (Tween tween in currentTweens)
        {
            tween.Kill();
        }



        if (isOpen)
        {
            buildTypeMenu.SetActive(true);
            buildTypeMenu.transform.localScale = Vector3.zero;
            currentTweens.Add(buildTypeMenu.transform.DOScale(1, 0.25f));
            buildMenu.transform.localScale = Vector3.zero;
            buildMenu.SetActive(false);
        }

        else
        {
            foreach (Image buttonImage in buttonImages)
            {
                buttonImage.color = Color.white;
            }

            currentTweens.Add(buildMenu.transform.DOScale(0, 0.25f).OnComplete(() =>
           {
               buildMenu.SetActive(false);
           }));

            currentTweens.Add(buildTypeMenu.transform.DOScale(0, 0.25f).OnComplete(() =>
            {
                buildTypeMenu.SetActive(false);
            }));
        }

    }

    public void SelectBuildTypeMenu(int index)
    {
        buildElements.ForEach(x => Destroy(x.gameObject));
        buildElements.Clear();
        buildMenu.transform.localScale = Vector3.zero;
        currentTweens.Add(buildMenu.transform.DOScale(1, 0.25f));
        buildMenu.SetActive(true);

        foreach (Image buttonImage in buttonImages)
        {
            buttonImage.color = Color.white;
        }


        buttonImages[index].color = Color.green;


        for (int i = 0; i < buildTypes[index].builds.Count; i++)
        {

            BuildElement newElement = Instantiate(buildElementPrefab, buildMenu.transform);

            newElement.buildType = buildTypes[index].builds[i];

            newElement.Init();

            buildElements.Add(newElement);


        }

    }

}


[System.Serializable]

public class BuildType
{
    public string name;

    public List<PlacedObjectTypeSO> builds;

}

[System.Serializable]
public class BuildResources
{
    public int resource1;
    public int resource2;
}