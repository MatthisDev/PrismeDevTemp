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
        private float moveAmount;
        
        [SerializeField] private float runningSpeed = 10;
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float rotationSpeed = 15;
        
        public Vector3 MoveDirection { get; private set; }
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
            if (!UIManager.Instance.IsOpenInventory)
            {
                HandleGroundMovement();
                HandleRotation();
            }
        }

        public void Update()
        {
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
            
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                // on court
                this.PlayerManager.CharacterController.Move(MoveDirection * (runningSpeed * Time.deltaTime));
            }else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                // on marche 
                PlayerManager.CharacterController.Move(MoveDirection * (walkingSpeed * Time.deltaTime));
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
    }
}