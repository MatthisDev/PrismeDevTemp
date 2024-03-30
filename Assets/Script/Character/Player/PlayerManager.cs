using System;
using Script.Player.Movement;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Player
{
    /*
     *  Permet de gerer tout ce qui relatif à un joueur
     *  Herite des proprietes de "{CharacterManager}"
     */
    public class PlayerManager : CharacterManager
    {

        [HideInInspector] public PlayerAnimatorManager PlayerAnimatorManager { get; set; }
        [HideInInspector] public PlayerMovementManager PlayerMovementManager { set; get; }
        
        [SerializeField] GameObject PrefabCamera;

        // Elle est appellé au tout debut
        protected override void Awake()
        {
            base.Awake();
            
            PlayerMovementManager = GetComponent<PlayerMovementManager>();
            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        protected void Start()
        {
            transform.position = Vector3.zero;
        }

        // Action realiser lorsqu'on est connecté a la partie
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            // Si on est le proprietaire du perso on lui fait spawn une camera associe et l'initialise
            if (IsOwner)
            {
                Instantiate(PrefabCamera, transform.position, transform.rotation);
                PlayerCamera.Instance.Player = this;
                PlayerInputManager.Instance.Player = this;
                SpawnItems.Instance.Spawn();
            }
        }
        
        /*
         * On ne se base pas sur l'udpate dans CharacterManager
         * car les positions sont deja actualises grace ClientNetworkTransform
         */
        protected override void Update()
        {
            // si ce n'est pas notre perso alors on ne fait rien
            if (!IsOwner) return;
            PlayerMovementManager.HandleAllMovement();
        }
        
        /*
         * On realise des actions sur la camera apres le deplacement du joueur
         */
        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;
            
            base.LateUpdate();
            PlayerCamera.Instance.HandleAllCameraActions();
        }
    }
}