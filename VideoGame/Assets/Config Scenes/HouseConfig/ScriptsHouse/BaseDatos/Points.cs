// Script Name: Points.cs
// Description:
//      * Represents the score and performance
//      * details for a minigame.
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 23/07/2024 
// **********************************************************************


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points 
{
    /// <summary>
    /// The score achieved in the minigame.
    /// </summary>
    public int score { get; set; }

    /// <summary>
    /// The number of mistakes made during the minigame.
    /// </summary>
    public int mistakes { get; set; }

    /// <summary>
    /// The name or type of the minigame (e.g., "Birds", "Amphibians").
    /// </summary>
    public string minigame { get; set; }

    /// <summary>
    /// The amount of time spent on the minigame, typically in seconds.
    /// </summary>
    public int time { get; set; }
}
