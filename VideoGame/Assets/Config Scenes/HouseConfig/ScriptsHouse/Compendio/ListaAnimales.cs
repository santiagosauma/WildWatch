// Script Name: ListaAnimales.cs
// Description:
//      * A ScriptableObject that holds lists of different types
//      * of animal information, allowing for the organization
//      * and management of animal data within the Unity editor. 
//      * This class provides a way to aggregate and categorize
//      * animal data into distinct lists, which can be used for
//      * various purposes within the application, such as
//      * displaying information in a compendium or managing game
//      * content.                         
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

[CreateAssetMenu]
public class ListaAnimales : ScriptableObject
{
    /// <summary>
    /// A list of bird species information.
    /// </summary>
    public List<BirdInfo> Pajaros;

    /// <summary>
    /// A list of amphibian species information.
    /// </summary>
    public List<AnfibioInfo> Anfibios;

    /// <summary>
    /// A list of insect species information.
    /// </summary>
    public List<InsectoInfo> Insectos;

    /// <summary>
    /// A list of mammal species information.
    /// </summary>
    public List<MamiferosInfo> Mamiferos;

    /// <summary>
    /// A list of plant species information, represented by `TileData`.
    /// </summary>
    public List<TileData> Plantas;
}
