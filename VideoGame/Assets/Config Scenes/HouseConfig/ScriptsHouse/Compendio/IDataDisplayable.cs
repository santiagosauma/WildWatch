// Script Name: IDataDisplayable.cs
// Description:
//      * Interface for objects that can be displayed in an
//      * animal compendium, ensuring consistent presentation
//      * of information across various categories such as
//      * amphibians, mammals, etc. 
//      * This interface is implemented by classes representing
//      * different animal categories (e.g., Amphibian, Mammal),
//      * allowing for a unified approach to handling and displaying
//      * animal data without the need for separate lists
//      * for each category.              
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

public interface IDataDisplayable
{
    /// <summary>
    /// Gets the name of the animal to be displayed 
    /// in the compendium.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Gets a description of the animal to be shown 
    /// in the user interface.
    /// </summary>
    string DisplayDescription { get; }

    /// <summary>
    /// Gets the image associated with the animal, used 
    /// for visual representation in the compendium.
    /// </summary>
    Sprite DisplayImage { get; }

    /// <summary>
    /// Gets the common or alternate name of the animal 
    /// for additional labeling or identification.
    /// </summary>
    string DisplayCommonName { get; }
}
