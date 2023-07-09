using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "task")]
public class TaskAnswers : ScriptableObject
{
    public List<string> Answers = new List<string>();
}
