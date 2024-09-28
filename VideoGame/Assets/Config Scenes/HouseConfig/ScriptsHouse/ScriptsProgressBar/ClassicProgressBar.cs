// Script Name: ClassicProgressBar.cs
// Description:
//      * This script manages a segmented progress bar in
//      * the IPadUI. It divides the progress bar into multiple
//      * segments, each of which can be partially filled
//      * based on a global progress value.         
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

public class ClassicProgressBar : MonoBehaviour 
{
    // Grouping variables under different headers for better organization
    [Header("Colors")]
    /// <summary>
    /// The main color used for the UI element.
    /// </summary>
    [SerializeField] private Color m_MainColor = Color.white;

    /// <summary>
    /// The color used to fill the segments of the progress bar.
    /// </summary>
    [SerializeField] private Color m_FillColor = Color.green;

    [Header("General")]
    /// <summary>
    /// The number of segments in the progress bar.
    /// </summary>
    [SerializeField] private int m_NumberOfSegments = 5;

    /// <summary>
    /// The size of the notch between segments in the progress bar.
    /// </summary>
    [SerializeField] private float m_SizeOfNotch = 5;

    /// <summary>
    /// The fill amount of the progress bar, ranging from 0 (empty) to 1 (full).
    /// </summary>
    [Range(0, 1f)][SerializeField] private float m_FillAmount = 0.0f;

    // Private variables used internally for managing the progress bar
    /// <summary>
    /// The RectTransform component of the UI element.
    /// </summary>
    private RectTransform m_RectTransform;

    /// <summary>
    /// The Image component used for the progress bar.
    /// </summary>
    private Image m_Image;

    /// <summary>
    /// A list of Image components representing the segments to be filled.
    /// </summary>
    private List<Image> m_ProgressToFill = new List<Image>();

    /// <summary>
    /// The size of each segment in the progress bar.
    /// </summary>
    private float m_SizeOfSegment;

    public void Awake()
    {
        // Initialize the RectTransform component
        InitializeRectTransform();

        // Initialize the Image component and set its color
        InitializeImage();

        // Calculate the size of each segment based on the total size and number of segments
        CalculateSegmentSize();

        // Create and initialize each segment of the progress bar
        CreateSegments();
    }

    /// <summary>
    /// Initializes the RectTransform component.
    /// </summary>
    private void InitializeRectTransform()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Initializes the Image component and sets its color.
    /// </summary>
    private void InitializeImage()
    {
        m_Image = GetComponentInChildren<Image>();
        m_Image.color = m_MainColor;
        m_Image.gameObject.SetActive(false);
    }

    /// <summary>
    /// Calculates the size of each segment based on the total size and number of segments.
    /// </summary>
    private void CalculateSegmentSize()
    {
        m_SizeOfSegment = m_RectTransform.sizeDelta.x / m_NumberOfSegments;
    }

    /// <summary>
    /// Creates and initializes each segment of the progress bar.
    /// </summary>
    private void CreateSegments()
    {
        for (int i = 0; i < m_NumberOfSegments; i++)
        {
            // Instantiate a new segment and set it active
            GameObject currentSegment = Instantiate(m_Image.gameObject,transform.position,
                                        Quaternion.identity, transform);
            currentSegment.SetActive(true);

            // Get the Image component and set its fill amount
            Image segmentImage = currentSegment.GetComponent<Image>();
            segmentImage.fillAmount = m_SizeOfSegment;

            // Adjust the size and position of the RectTransform
            RectTransform segmentRectTransform = segmentImage.GetComponent<RectTransform>();
            segmentRectTransform.sizeDelta = new Vector2(m_SizeOfSegment,
                                             segmentRectTransform.sizeDelta.y);
            segmentRectTransform.position += (Vector3.right * i * m_SizeOfSegment) -
                                             (Vector3.right * m_SizeOfSegment * 
                                             (m_NumberOfSegments / 2)) + 
                                             (Vector3.right * i * m_SizeOfNotch);

            // Set the color and size of the fill image
            Image segmentFillImage = segmentImage.transform.GetChild(0).GetComponent<Image>();
            segmentFillImage.color = m_FillColor;
            m_ProgressToFill.Add(segmentFillImage);
            segmentFillImage.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(m_SizeOfSegment,segmentFillImage.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    public void Update()
    {
        /// <summary>
        /// Updates the fill amount of each segment in the progress bar.
        /// </summary>
        for (int i = 0; i < m_NumberOfSegments; i++)
        {
            // Calculate the fill amount for each segment based on the overall fill amount and segment index
            m_ProgressToFill[i].fillAmount = m_NumberOfSegments * m_FillAmount - i;
        }
    }

    /// <summary>
    /// Converts a fragment (a value between 0 and 1) to a width based on the RectTransform's width.
    /// </summary>
    /// <param name="fragment">The fragment value to convert (between 0 and 1).</param>
    /// <returns>The corresponding width based on the RectTransform's width.</returns>
    private float ConvertFragmentToWidth(float fragment)
    {
        return m_RectTransform.sizeDelta.x * fragment;
    }
}
