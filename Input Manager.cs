using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
   PlayerControls playerControls;
   PlayerLocomotion playerLocomotion;
   AnimatorManager animatorManager;
   public Vector2 movementInput;
   public float moveAmount;
   public Vector2 cameraInput;
   public float cameraInputX;
   public float cameraInputY;
   public float verticalInput;
   public float horizontalInput;
   
   public bool b_Input;
   public bool x_Input;
   public bool jump_Input;
   private void Awake()
   {
     animatorManager = GetComponent<AnimatorManager>();
     playerLocomotion = GetComponent<PlayerLocomotion>();
   }
   private void OnEnable()
   {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        
            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;

            playerControls.PlayerActions.X.performed += i => x_Input = true;

            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
        }
        playerControls.Enable();
   }

   private void OnDisable()
   {
        playerControls.Disable();
   }

   public void HandleAllInputs()
   {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleDodgeInput();
   }
    private void HandleMovementInput()
   { 
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y; 
        cameraInputX = cameraInput.x;
        
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
   }

   private void HandleSprintingInput()
   {
       if (b_Input && moveAmount > 0.5f)
       {
           playerLocomotion.isSprinting = true;
       }
       else
       {
           playerLocomotion.isSprinting = false;
       }
   }

   private void HandleJumpingInput()
   {
     if(jump_Input)
     {
          jump_Input = false;
          playerLocomotion.HandleJumping();
     }
   }

   private void HandleDodgeInput()
   {
     if(x_Input)
     {
          x_Input = false;
          playerLocomotion.HandleDodge();
     }
   }
}
