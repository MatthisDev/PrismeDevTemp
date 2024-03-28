using System;
using System.Collections;
using System.Collections.Generic;
using Script.Character;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour 
{
    public CharacterController CharacterController { set; get; }
    public Animator CharacterAnimator { get; set; }
    
    private CharacterNetworkManager CharacterNetworkManager { set; get; }
    
    protected virtual void Awake()
    {
       DontDestroyOnLoad(this);
       
       this.CharacterController = GetComponent<CharacterController>();
       this.CharacterAnimator = GetComponent<Animator>();
       this.CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
    }
    protected virtual void Update()
    {
        if (IsOwner)
        {
            // si on est le owner du perso alors on envoie notre position sur le network 
            CharacterNetworkManager.NetworkPosition.Value = this.transform.position;
            CharacterNetworkManager.NetworkRotation.Value = this.transform.rotation;
        }
        else if(!IsOwner)
        {
            // si ce n'est pas notre on recup la postion+rotation du perso
            transform.position = Vector3.SmoothDamp(this.transform.position, CharacterNetworkManager.NetworkPosition.Value,
                ref CharacterNetworkManager.networkPositionVelocity, CharacterNetworkManager.networkPositionSmoothTime);
            
            transform.rotation = Quaternion.Slerp(this.transform.rotation,
                CharacterNetworkManager.NetworkRotation.Value, CharacterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate() { }

}
