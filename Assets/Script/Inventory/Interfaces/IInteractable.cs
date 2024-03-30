using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface IInteractable //interface à mettre sur le script d'un objet pour pouvoir intéragir
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool InteractSuccessful);
    public void EndInteraction();
}
