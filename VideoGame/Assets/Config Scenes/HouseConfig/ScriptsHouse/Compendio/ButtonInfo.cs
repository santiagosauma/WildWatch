// **********************************************************************
// Script Name: ButtonInfo.cs
// Description:
//      * This identifier helps in determining the
//      * category of an item when selected in the UI, 
//      * allowing the corresponding list of items
//      * within that category to be accessed and processed.
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 22/07/2024 
// **********************************************************************


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    /// <summary>
    /// Unique identifier used to categorize and reference different groups of items, 
    /// such as mammals (id 0) or amphibians (id 1). 
    /// </summary>
    public int UniqueId;
}
