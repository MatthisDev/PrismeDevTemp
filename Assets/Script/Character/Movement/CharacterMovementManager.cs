using Unity.Netcode;
using UnityEngine;

namespace Script.Character.Movement
{
    /*
     * Gere les mouvements generaux 
     */
    public class CharacterMovementManager : NetworkBehaviour 
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
    }
}