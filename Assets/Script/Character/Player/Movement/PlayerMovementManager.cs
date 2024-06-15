using System;
using System.Net;
using Script.Character.Movement;
using Script.Inventory.UI_Scripts;
using UnityEngine;
using UnityEngine.Timeline;

namespace Script.Player.Movement
{
    /*
     *  Gere les deplacements du joueurs
     */
    public class PlayerMovementManager : CharacterMovementManager 
    {
        private PlayerManager PlayerManager { set; get; }
        private float VerticalMovement { get; set; } 
        private float HorizontalMovement { get; set; }
        [SerializeField] private float runningSpeed = 10;
        [SerializeField] private float sprintingSpeed = 11;
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] Vector3 rollDirection;
        
        //Jump
        [SerializeField] private Vector3 jumpDirection = Vector3.zero;
        [SerializeField] private float jumpingForwardSpeed = 5;
        [SerializeField] private float fallingForwardSpeed = 2;
        [SerializeField] private float jumpHeight = 2;
        public Vector3 MoveDirection { get; private set; }
        public float Velocity => runningSpeed;
        public Vector3 RotationDirection { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            // init PlayerManager
            PlayerManager = GetComponent<PlayerManager>();
        }

        // gere les mouvements
        public void HandleAllMovement()
        {
            if (PlayerManager.isPerformingAction)
                return;
            if (UIManager.Instance != null && !UIManager.Instance.IsOpenInventory)
            {
                HandleGroundMovement();
                HandleRotation();
                HandleJumpingMovement();
                HandleFreeFallMovement();
            }
        }

        protected override void Update()
        {
            base.Update();
            HandleAllMovement();
        }

        private void GetVerticalAndHorizontalInputs()
        {
            VerticalMovement = PlayerInputManager.Instance.verticalInput;
            HorizontalMovement = PlayerInputManager.Instance.horizontalInput;
        }
        private void HandleGroundMovement()
        {
            // update the input
            GetVerticalAndHorizontalInputs();
            
            var transform1 = PlayerCamera.Instance.transform;
            MoveDirection = transform1.forward * VerticalMovement;
            MoveDirection += transform1.right * HorizontalMovement;
            MoveDirection.Normalize();
            
            // on empeche le perso de voler :)
            var vector3 = MoveDirection;
            vector3.y = 0;
            MoveDirection = vector3;

            if (PlayerManager.PlayerNetworkManager.isSprinting.Value)
            {
                this.PlayerManager.CharacterController.Move(MoveDirection * (sprintingSpeed * Time.deltaTime));
            }
            else
            {

                if (PlayerInputManager.Instance.moveAmount > 0.5f)
                {
                    // on court
                    this.PlayerManager.CharacterController.Move(MoveDirection * (runningSpeed * Time.deltaTime));
                }
                else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
                {
                    // on marche 
                    PlayerManager.CharacterController.Move(MoveDirection * (walkingSpeed * Time.deltaTime));
                }
            }
        }
        private void HandleRotation()
        {
            RotationDirection = Vector3.zero;
            
            // On oriente le joueur en fonction de l'orientation de la camera
            RotationDirection =  PlayerCamera.Instance.CameraObject.transform.forward * VerticalMovement;
            RotationDirection += PlayerCamera.Instance.CameraObject.transform.right * HorizontalMovement;
            RotationDirection.Normalize();
            
            var vector3 = RotationDirection;
            vector3.y = 0;
            RotationDirection = vector3;

            // on ne change pas la rotation du joueur
            if (RotationDirection == Vector3.zero)
                RotationDirection = transform.forward;

            // Position qu'on vise
            Quaternion turn = Quaternion.LookRotation(RotationDirection);
            
            // Position final
            transform.rotation = Quaternion.Slerp(transform.rotation, turn, rotationSpeed * Time.deltaTime);
        }
        private void HandleJumpingMovement()
        {
            if (PlayerManager.isJumping)
            {
                PlayerManager.CharacterController.Move(Vector3.zero * (0 * Time.deltaTime));

            }
            
        }
        private void HandleFreeFallMovement()
        {
            if (!PlayerManager.isGrounded && !PlayerManager.isJumping)
            {
                var freeFallDirection = PlayerCamera.Instance.transform.forward *
                                        PlayerInputManager.Instance.horizontalInput;
                freeFallDirection += PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.verticalInput;
                freeFallDirection.y = 0;

                PlayerManager.CharacterController.Move(freeFallDirection * (fallingForwardSpeed * Time.deltaTime));
            }
        }
        
        public void HandleSprinting()
        {
            if (PlayerManager.isPerformingAction)
            {
                this.PlayerManager.PlayerNetworkManager.isSprinting.Value = false;
            }
    
            if (PlayerInputManager.Instance.moveAmount >= 0.5)
                PlayerManager.PlayerNetworkManager.isSprinting.Value = true;
            else
                this.PlayerManager.PlayerNetworkManager.isSprinting.Value = false;
        }
        //public void AttemptTo
        public void AttemptToPerformDodge()
        {
            if (PlayerManager.isPerformingAction)
                return;

            if (PlayerInputManager.Instance.moveAmount > 0)
            {
                Debug.Log("try to dodge");
                rollDirection = PlayerCamera.Instance.CameraObject.transform.forward *
                                PlayerInputManager.Instance.verticalInput;
                rollDirection += PlayerCamera.Instance.CameraObject.transform.right *
                                 PlayerInputManager.Instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                PlayerManager.transform.rotation = playerRotation;

                PlayerManager.PlayerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            else // on fait le "backstep" dans le cas ou on ne pas
            {
                Debug.Log("try to back step");
                PlayerManager.PlayerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }

        }
        public void AttemptToPerformJump()
        {
            if (PlayerManager.isPerformingAction)
                return;
            
            if (PlayerManager.isJumping)
                return;
            if (!PlayerManager.isGrounded)
                return;
            Debug.Log("try to jump");

            PlayerManager.PlayerAnimatorManager.PlayTargetActionAnimation("Main_Jump_Start_01", false, false);
            
            PlayerManager.isJumping = true;
            jumpDirection = PlayerCamera.Instance.CameraObject.transform.forward * 
                            PlayerInputManager.Instance.verticalInput;
            jumpDirection += PlayerCamera.Instance.CameraObject.transform.right *
                             PlayerInputManager.Instance.horizontalInput;
            jumpDirection.y = 0;
            jumpDirection.Normalize();

            if (jumpDirection != Vector3.zero)
            {
                // on ne va pas aussi loin en fonction de notre vitesse
                if (PlayerManager.PlayerNetworkManager.isSprinting.Value)
                    jumpDirection *= 1;
                else if (PlayerInputManager.Instance.moveAmount >= 0.5)
                    jumpDirection *= 0.5f;
                else if (PlayerInputManager.Instance.moveAmount < 0.5)
                    jumpDirection *= 0.25f;
            }
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
        
    }
}