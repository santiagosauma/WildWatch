// Script Name: CircularProgressBar.cs
// Description:
//      * This script manages a segmented circular progress 
//      * bar in the IPadUI. It divides the progress bar 
//      * into multiple segments, each of which can be 
//      * partially filled based on a global progress value.     
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 24/07/2024 
// **********************************************************************


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour
{
    [Header("Colors")]

    /// <summary>
    /// Main color for the progress bar segments.
    /// </summary>
    [SerializeField] private Color m_MainColor = Color.white;

    /// <summary>
    /// Fill color for the progress bar segments.
    /// </summary>
    [SerializeField] private Color m_FillColor = Color.green;

    [Header("General")]

    /// <summary>
    /// Number of segments in the progress bar.
    /// </summary>
    [SerializeField] private int m_NumberOfSegments = 5;

    /// <summary>
    /// Starting angle for the progress bar.
    /// </summary>
    [Range(0, 360)][SerializeField] private float m_StartAngle = 40;

    /// <summary>
    /// Ending angle for the progress bar.
    /// </summary>
    [Range(0, 360)][SerializeField] private float m_EndAngle = 320;

    /// <summary>
    /// Size of the notch between segments.
    /// </summary>
    [SerializeField] private float m_SizeOfNotch = 5;

    /// <summary>
    /// Overall fill amount (0 to 1).
    /// </summary>
    [Range(0, 1f)][SerializeField] private float m_FillAmount = 0.0f;

    /// <summary>
    /// Image component used for the segments.
    /// </summary>
    private Image m_Image;

    /// <summary>
    /// List to store the fillable images for each segment.
    /// </summary>
    private List<Image> m_ProgressToFill = new List<Image>();

    /// <summary>
    /// Calculated size of each segment.
    /// </summary>
    private float m_SizeOfSegment;

    private void Awake()
    {
        InitializeImage();
        CalculateNotches();
        CreateSegments();
    }

    /// <summary>
    /// Initializes the image component by configuring its color and setting it to inactive.
    /// </summary>
    private void InitializeImage()
    {
        m_Image = GetComponentInChildren<Image>();
        m_Image.color = m_MainColor;
        m_Image.gameObject.SetActive(false);
    }

    /// <summary>
    /// Calculates the notches and segment sizes based on the given angles and number of segments.
    /// </summary>
    private void CalculateNotches()
    {
        float startNormalAngle = NormalizeAngle(m_StartAngle);
        float endNormalAngle = NormalizeAngle(360 - m_EndAngle);
        float notchesNormalAngle = (m_NumberOfSegments - 1) * NormalizeAngle(m_SizeOfNotch);
        float allSegmentsAngleArea = 1 - startNormalAngle - endNormalAngle - notchesNormalAngle;

        m_SizeOfSegment = allSegmentsAngleArea / m_NumberOfSegments;
    }

    /// <summary>
    /// Creates and configures each segment, including setting the fill amount and rotation.
    /// </summary>
    private void CreateSegments()
    {
        for (int i = 0; i < m_NumberOfSegments; i++)
        {
            GameObject currentSegment = Instantiate(m_Image.gameObject, transform.position,
                                    Quaternion.identity, transform);
            currentSegment.SetActive(true);

            Image segmentImage = currentSegment.GetComponent<Image>();
            segmentImage.fillAmount = m_SizeOfSegment;

            Image segmentFillImage = segmentImage.transform.GetChild(0).GetComponent<Image>();
            segmentFillImage.color = m_FillColor;
            m_ProgressToFill.Add(segmentFillImage);

            float zRot = m_StartAngle + i * ConvertCircleFragmentToAngle(m_SizeOfSegment) + i * m_SizeOfNotch;
            segmentImage.transform.rotation = Quaternion.Euler(0, 0, -zRot);
        }
    }


    /// <summary>
    /// Sets the fill amount for the segments.
    /// </summary>
    /// <param name="num">The desired fill amount as a value between 0 and 1.</param>
    public void setFill(float num)
    {
        m_FillAmount = num;
    }

    private void Update()
    {
        UpdateSegmentFillAmounts();
    }

    /// <summary>
    /// Updates the fill amount of each segment based on the current fill amount and segment size.
    /// </summary>
    private void UpdateSegmentFillAmounts()
    {
        for (int i = 0; i < m_NumberOfSegments; i++)
        {
            m_ProgressToFill[i].fillAmount = CalculateSegmentFillAmount(i);
        }
    }

    /// <summary>
    /// Calculates the fill amount for a specific segment.
    /// </summary>
    /// <param name="segmentIndex">The index of the segment to calculate the fill amount for.</param>
    /// <returns>The calculated fill amount for the segment.</returns>
    private float CalculateSegmentFillAmount(int segmentIndex)
    {
        return (m_FillAmount * ((m_EndAngle - m_StartAngle) / 360)) - m_SizeOfSegment * segmentIndex;
    }


    /// <summary>
    /// Normalizes an angle by clamping it to a value between 0 and 1 based on a full circle (360 degrees).
    /// </summary>
    /// <param name="angle">The angle in degrees to normalize.</param>
    /// <returns>The normalized angle as a value between 0 and 1.</returns>
    private float NormalizeAngle(float angle)
    {
        return Mathf.Clamp01(angle / 360f);
    }

    /// <summary>
    /// Converts a circle fragment (a proportion) to an angle in degrees.
    /// </summary>
    /// <param name="fragment">The fragment of the circle as a proportion (0 to 1).</param>
    /// <returns>The angle in degrees corresponding to the given fragment.</returns>
    private float ConvertCircleFragmentToAngle(float fragment)
    {
        return 360 * fragment;
    }
}