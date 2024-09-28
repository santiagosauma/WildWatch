// **********************************************************************
// Script Name: AnfibioInfo.cs
// Description:
//      * Defines a scriptable object to hold information about an amphibian.
//      * This class is used to store and display data about amphibians,
//      * including their name, description, image, and ID. It also provides
//      * properties for displaying the common name and other details in a
//      * user interface.
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 20/07/2024 
// **********************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AnfibioInfo", menuName = "Animal Data/Anfibio Info")]
public class AnfibioInfo : ScriptableObject, IDataDisplayable
{
    /// <summary>
    /// The name of the amphibian.
    /// </summary>
    public string Name;

    /// <summary>
    /// The description of the amphibian.
    /// </summary>
    public string Description;

    /// <summary>
    /// The image of the amphibian.
    /// </summary>
    public Sprite Image;

    /// <summary>
    /// The unique identifier for the amphibian.
    /// </summary>
    public int id;

    /// <summary>
    /// The common name of the amphibian.
    /// </summary>
    public string nombreComun;

    /// <summary>
    /// Gets the common name of the amphibian for display purposes.
    /// </summary>
    public string DisplayCommonName => nombreComun;

    /// <summary>
    /// Gets the name of the amphibian for display purposes.
    /// </summary>
    public string DisplayName => Name;

    /// <summary>
    /// Gets the description of the amphibian for display purposes.
    /// </summary>
    public string DisplayDescription => Description;

    /// <summary>
    /// Gets the image of the amphibian for display purposes.
    /// </summary>
    public Sprite DisplayImage => Image;
}
