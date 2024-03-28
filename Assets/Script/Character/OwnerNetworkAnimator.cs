using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;


/*
 *  La seule utilisation est pour que le client puisse actualiser/synchroniser les animations des joueurs
 */
public class OwnerNetworkAnimator: NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
