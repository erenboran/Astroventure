
using UnityEngine;


[CreateAssetMenu(fileName = "Task", menuName = "New Task", order = 1)]
public class Task : ScriptableObject
{
    public string taskName;
    public string taskDescription;

    public bool isCompleted;

}