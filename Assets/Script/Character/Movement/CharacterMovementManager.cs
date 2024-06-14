using System;
using Unity.Netcode;
using UnityEngine;

namespace Script.Character.Movement
{
    /*
     * Gere les mouvements generaux 
     */
    public class CharacterMovementManager : NetworkBehaviour
    {
        
        CharacterManager character;
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity; // force de gravite (vitesse a laquel il tombe)
        [SerializeField] protected float groundedYVelocity = -20;
        [SerializeField] protected float fallStartYVelocity = -5;
        [SerializeField] protected bool fallingVelocityHasBeenSet = false;
        [SerializeField]protected float inAirTimer = 0;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            Debug.Log(character);
        }

        protected virtual void Update()
        {
            HandleGroundCheck();
            if (character.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if (!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTimer = inAirTimer + Time.deltaTime;
                character.CharacterAnimator.SetFloat("inAirTimer",inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
            }
            
            character.CharacterController.Move(yVelocity * Time.deltaTime);

        }
        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            character = GetComponent<CharacterManager>();
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}