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
        [HideInInspector] public PlayerNetworkManager PlayerNetworkManager { set; get; }
        [SerializeField] GameObject prefabCamera;
        [SerializeField] GameObject prefabUI;

        // Elle est appellé au tout debut
        protected override void Awake()
        {
            base.Awake();
            PlayerMovementManager = GetComponent<PlayerMovementManager>();
            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            PlayerNetworkManager = GetComponent<PlayerNetworkManager>();
        }

        protected void Start()
        {
            transform.position = Vector3.zero;
            Cursor.visible = false;
        }

        // Action realiser lorsqu'on est connecté a la partie
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            // Si on est le proprietaire du perso on lui fait spawn une camera associe et l'initialise
            if (IsOwner)
            {
                
                Instantiate(prefabCamera, transform.position, transform.rotation);
                Instantiate(prefabUI, transform.position, transform.rotation);
                Debug.Log(this.transform.position);
                PlayerCamera.Instance.Player = this;
                PlayerInputManager.Instance.Player = this;
                //SpawnItems.Instance.Spawn();
                
            }
        }
        
        /*
         * On ne se base pas sur l'udpate dans CharacterManager
         * car les positions sont deja actualises grace ClientNetworkTransform
         */
        protected override void Update()
        {
            base.Update();
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