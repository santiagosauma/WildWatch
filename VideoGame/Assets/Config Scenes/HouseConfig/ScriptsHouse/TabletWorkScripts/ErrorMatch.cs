// Script Name: ErrorMatch.cs
// Description:
//      * Represents an error match with a message and an identifier.
//      * This class is used to encapsulate information about errors
//      * or mismatches in the application.                
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

public class ErrorMatch
{
    /// <summary>
    /// Gets or sets the error or mismatch message.
    /// </summary>
    public string message { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the error or mismatch.
    /// </summary>
    public int MatchID { get; set; }
}
