using System;
using System.Collections;
using System.Collections.Generic;
using Script.Inventory.UI_Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour //script qui permet au joueur d'intéragir avec les Iinteractable
{
    //public Transform InteractionPoint;
    //public LayerMask InteractionLayer;
    //public float InteractionPointRadius = 1f;
    public Transform interactorSource;
    public float interactionRange = 1f;

    public void Update()
    {
        if (PlayerInputManager.Instance.interactInput)
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (!Physics.Raycast(r, out RaycastHit hitInfo, interactionRange)) return;
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                UIManager.Instance.OpenInputInventoryAction(true);
                StartInteraction(interactable);
            }
            //var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer); for (int i = 0; i < colliders.Length; i++) { var interactable = colliders[i].GetComponent<IInteractable>(); if (interactable != null) StartInteraction(interactable);}
        }
    }


    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
    }
}
