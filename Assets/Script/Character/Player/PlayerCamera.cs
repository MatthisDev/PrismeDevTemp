using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Cinemachine;
using Script.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
   public static PlayerCamera Instance { get; private set; }
   public PlayerManager Player;
   public Camera CameraObject;
   public Transform cameraPivotTransform;

   [Header("Camera Settings")]
   [SerializeField] private float cameraSmoothSpeed = 10;
   [SerializeField] private float _sensitivityX = 1;
   [SerializeField] private float _sensitivityY = 1;
   [SerializeField] private float minimumPivot = -30;
   [SerializeField] private float maximumPivot = 60;
   [SerializeField] private float cameraCollisionRadius = 0.2f;
   [SerializeField] private LayerMask collideWithLayers;
   
   [Header("Camera Values")] 
   private Vector3 cameraVelocity;
   private Vector3 cameraObjectPosition;
   private float LeftAndRightLookAngle { get; set; }
   private float UpAndDownLookAngle { get; set; }
   
   private float defaultCameraZPosition;   
   private float targetCameraZPosition;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      else
         Destroy(gameObject);
   }

   private void Start()
   {
      DontDestroyOnLoad(gameObject);

      defaultCameraZPosition = CameraObject.transform.localPosition.z;
   }

   public void HandleAllCameraActions()
   {
      if (Player != null)
      {
         HandleFollowPlayer();
         HandleRotations();
         HandleCollisions();
      }
   }
   private void HandleFollowPlayer()
   {
      transform.position = Vector3.SmoothDamp(transform.position, Player.transform.position,
         ref cameraVelocity,cameraSmoothSpeed * Time.deltaTime);
   }
   
   private void HandleRotations()
   {
      
      LeftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * _sensitivityX) * Time.deltaTime;
      UpAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * _sensitivityY) * Time.deltaTime;

      UpAndDownLookAngle = Mathf.Clamp(UpAndDownLookAngle, minimumPivot, maximumPivot);

      Vector3 cameraRotation = Vector3.zero;

      cameraRotation.y = LeftAndRightLookAngle;
      transform.rotation = Quaternion.Euler(cameraRotation);

      cameraRotation = Vector3.zero;
      cameraRotation.x = UpAndDownLookAngle;
      cameraPivotTransform.localRotation = Quaternion.Euler(cameraRotation);
   }

   private void HandleCollisions()
   {
      targetCameraZPosition = defaultCameraZPosition;
      RaycastHit hit;
      Vector3 direction = CameraObject.transform.position - cameraPivotTransform.position;
      direction.Normalize();
      if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit,
             Mathf.Abs(targetCameraZPosition), collideWithLayers))
      {
         float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
         targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
      }

      if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
      {
         targetCameraZPosition = -cameraCollisionRadius;
      }

      cameraObjectPosition.z =
         Mathf.Lerp(CameraObject.transform.localPosition.z, targetCameraZPosition, cameraCollisionRadius);

      CameraObject.transform.localPosition = cameraObjectPosition;
   }
}
