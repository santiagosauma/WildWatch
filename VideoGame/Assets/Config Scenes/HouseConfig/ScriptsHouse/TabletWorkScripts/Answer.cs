// Script Name: Answer.cs
// Description:
//      * This script defines the `Answer` class, which holds the properties 
//      * related to an answer in a minigame. The class stores identifiers 
//      * for the minigame, species, and image, as well as a quantity value. 
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 27/08/2024
// **********************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Answer
{
    /// <summary>
    /// Identifier for the associated minigame.
    /// </summary>
    public int minigameId;

    /// <summary>
    /// Identifier for the species involved in the answer.
    /// </summary>
    public int idEspecie;

    /// <summary>
    /// Identifier for the associated image.
    /// </summary>
    public int imgId;

    /// <summary>
    /// Quantity related to the answer (e.g., number of correct answers).
    /// </summary>
    public int quantity;
}
