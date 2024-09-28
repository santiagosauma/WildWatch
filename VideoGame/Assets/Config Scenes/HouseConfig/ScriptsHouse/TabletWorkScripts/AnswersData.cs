// **********************************************************************
// Script Name: AnswersData.cs
// Description:
//      * This script manages the data for quiz answers in the game.
//      * It is used to store a list of answers, which can be accessed 
//      * and modified as needed during gameplay.
// Authors:
//      * Luis Santiago Sauma Pe�aloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 28/09/2024
// **********************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnswersData", menuName = "Quiz/Answers Data", order = 1)]
public class AnswersData : ScriptableObject
{
    /// <summary>
    /// A list of answers used in the quiz. 
    /// This list holds Answer objects, each representing a possible answer 
    /// to a question in the quiz.
    /// </summary>
    public List<Answer> answers = new List<Answer>();
}