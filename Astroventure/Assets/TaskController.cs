using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TaskController : MonoBehaviour
{
    [SerializeField]
    TMP_Text taskName, taskDescription;

    [SerializeField]
    Image taskImage;

    [SerializeField]
    List<Task> tasks;

    bool isIronMineCompleted, isCopperMineCompleted;

    int taskIndex = 0;

    bool[] istasksCompleted = new bool[5];

    public static TaskController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        taskName.text = tasks[0].taskName;
        taskDescription.text = tasks[0].taskDescription;

    }

    IEnumerator TaskComplatedAnim()
    {
        taskImage.DOColor(Color.green, 1);
        taskName.text = "Görev Tamamlandı";
        taskIndex++;
        taskDescription.text = "Tebrikler " + taskIndex + ". görev tamamlandı bir sonraki görevin geliyor.";
        yield return new WaitForSeconds(7);
        taskImage.DOColor(Color.white, 1);
        taskName.text = tasks[taskIndex].taskName;
        taskDescription.text = tasks[taskIndex].taskDescription;

        if (taskIndex == 1)
        {
            GameEvents.Instance.OnAllBuldingCanBuild?.Invoke();

        }

        if (taskIndex == 3)
        {
            GameEvents.Instance.OnEnemySpawnWave?.Invoke(30);

            yield return new WaitForSeconds(15);

            taskImage.DOFade(0, 1);
            taskName.text = "";
            taskDescription.text = "";


        }



    }

    public void TaskControl1(Enums.ResourceTypes resourceType)
    {
        if (istasksCompleted[0])
        {
            return;
        }

        if (resourceType == Enums.ResourceTypes.Demir)
        {
            isIronMineCompleted = true;
        }
        else if (resourceType == Enums.ResourceTypes.Bakir)
        {
            isCopperMineCompleted = true;
        }

        if (isIronMineCompleted && isCopperMineCompleted)
        {
            tasks[0].isCompleted = true;
            istasksCompleted[0] = true;
            StartCoroutine(TaskComplatedAnim());

        }


    }

    public void TaskControl2()
    {
        if (istasksCompleted[taskIndex])
        {
            return;
        }

        tasks[taskIndex].isCompleted = true;
        istasksCompleted[taskIndex] = true;
        StartCoroutine(TaskComplatedAnim());
    }

    public void TaskControl3()
    {
        if (istasksCompleted[taskIndex] || !istasksCompleted[taskIndex - 1])
        {
            return;
        }


        tasks[taskIndex].isCompleted = true;
        istasksCompleted[taskIndex] = true;
        StartCoroutine(TaskComplatedAnim());
    }

}
