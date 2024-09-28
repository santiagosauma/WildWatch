// **********************************************************************
// Script Name: CharacterControll.cs
// Description:
//      * This script handles the character's movement
//      * and animation in response to player input.
//      * It includes functionality for: Moving the
//      * character based on input from the user.
//      * Updating the character's position in the game world.
//      * Changing the character's animation to match
//      * the direction of movement.      
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


//NOT USED IN FINAL GAME VERSION


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    /// <summary>
    /// Reference to the Animator component that controls animations.
    /// </summary>
    Animator animatorController;

    /// <summary>
    /// Speed at which the object moves or performs actions.
    /// </summary>
    public float speed = 5.0f;

    /// <summary>
    /// Unity's Start method to initialize the Animator component.
    /// </summary>
    void Start()
    {
        // Get the Animator component attached to this GameObject.
        animatorController = GetComponent<Animator>();
    }

    /// <summary>
    /// Updates the character's animation based on the direction of movement.
    /// </summary>
    /// <param name="horizontal">The horizontal input value.</param>
    /// <param name="vertical">The vertical input value.</param>
    void UpdateAnimationDirection(float horizontal, float vertical)
    {
        // Determine if the vertical movement is greater than the horizontal movement.
        if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            // If moving upwards, play the walk up animation.
            if (vertical > 0)
            {
                UpdateAnimation(CharacterAnimation.WalkUp);
            }
            // If moving downwards, play the walk down animation.
            else
            {
                UpdateAnimation(CharacterAnimation.WalkDown);
            }
        }
        // If horizontal movement is significant.
        else if (Mathf.Abs(horizontal) > Mathf.Epsilon)
        {
            // Store the original scale of the character.
            float originalScaleX = Mathf.Abs(transform.localScale.x);

            // If moving right, play the walk right animation.
            if (horizontal > 0)
            {
                UpdateAnimation(CharacterAnimation.WalkRight);
            }
            // If moving left, play the walk left animation.
            else
            {
                UpdateAnimation(CharacterAnimation.WalkLeft);
            }
        }
    }

    /// <summary>
    /// Unity's FixedUpdate method to handle character movement and animation updates.
    /// This method is called at a fixed time interval and is used for physics-based updates.
    /// </summary>
    private void FixedUpdate()
    {
        // Get the horizontal and vertical input values.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on the input values.
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

        // Update the character's position based on the movement vector, speed, and delta time.
        transform.position += movement * speed * Time.deltaTime;

        // Update the character's animation based on the movement direction.
        UpdateAnimationDirection(moveHorizontal, moveVertical);
    }

    /// <summary>
    /// Enum representing the different character animations for walking in various directions.
    /// </summary>
    public enum CharacterAnimation
    {
        /// <summary>
        /// Animation for walking upwards.
        /// </summary>
        WalkUp,

        /// <summary>
        /// Animation for walking downwards.
        /// </summary>
        WalkDown,

        /// <summary>
        /// Animation for walking to the left.
        /// </summary>
        WalkLeft,

        /// <summary>
        /// Animation for walking to the right.
        /// </summary>
        WalkRight
    }

    /// <summary>
    /// Updates the animator controller to play the specified character animation.
    /// </summary>
    /// <param name="animation">The character animation to play.</param>
    void UpdateAnimation(CharacterAnimation animation)
    {
        // Switch between the different character animations.
        switch (animation)
        {
            case CharacterAnimation.WalkUp:
                // Set the animator parameters for walking upward.
                animatorController.SetBool("isUpward", true);
                animatorController.SetBool("isDownward", false);
                animatorController.SetBool("isRight", false);
                animatorController.SetBool("isLeft", false);
                break;

            case CharacterAnimation.WalkDown:
                // Set the animator parameters for walking downward.
                animatorController.SetBool("isUpward", false);
                animatorController.SetBool("isDownward", true);
                animatorController.SetBool("isRight", false);
                animatorController.SetBool("isLeft", false);
                break;

            case CharacterAnimation.WalkRight:
                // Set the animator parameters for walking to the right.
                animatorController.SetBool("isUpward", false);
                animatorController.SetBool("isDownward", false);
                animatorController.SetBool("isRight", true);
                animatorController.SetBool("isLeft", false);
                break;

            case CharacterAnimation.WalkLeft:
                // Set the animator parameters for walking to the left.
                animatorController.SetBool("isUpward", false);
                animatorController.SetBool("isDownward", false);
                animatorController.SetBool("isRight", false);
                animatorController.SetBool("isLeft", true);
                break;
        }
    }


}
