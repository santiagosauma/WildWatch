using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnswersData", menuName = "Quiz/Answers Data", order = 1)]
public class AnswersData : ScriptableObject
{
    public List<Answer> answers = new List<Answer>();
}