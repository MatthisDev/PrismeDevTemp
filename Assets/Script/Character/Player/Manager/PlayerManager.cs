using System;
using Script.Player.Movement;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Script.Player
{
    /*
     *  Permet de gerer tout ce qui relatif à un joueur
     *  Herite des proprietes de "{CharacterManager}"
     */
    public class PlayerManager : CharacterManager
    {
        // components
        [HideInInspector] public PlayerAnimatorManager PlayerAnimatorManager { get; set; }
        [HideInInspector] public PlayerMovementManager PlayerMovementManager { set; get; }
        [HideInInspector] public PlayerNetworkManager PlayerNetworkManager { set; get; }
       
        [Header("PREFABS")] 
        [SerializeField] private GameObject prefabCamera;
        [SerializeField] private GameObject prefabUI;
        [SerializeField] private GameObject sword;
        
        [Header("STATS PLAYER")] 
        [SerializeField]
        public PlayerStat maxPv;
        public PlayerStat strength; 
        public PlayerStat defense;
        public PlayerStat intelligence;
        public float pv;
        public float exp;
        public float necessaryExp = 100;
        public int level = 1;
        public int skillPoints;
        
        [Header("STATE VALUE")]
        public bool isDead = false; 
        public bool isEquipped = false;
        public bool isAttacking = false;

        public LayerMask layerMask;
        

        // Elle est appellée au tout debut
        protected override void Awake()
        {
            base.Awake();
            PlayerMovementManager = GetComponent<PlayerMovementManager>();
            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            PlayerNetworkManager = GetComponent<PlayerNetworkManager>();
            // l'epee est invisible au debut 
            sword.gameObject.SetActive(false);
            maxPv.BaseValue = 100f;
            pv = maxPv.Value;
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
                SpawnItems.Instance.Spawn();
                
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
            if(pv == -1.0f) HandleDie();
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
        
        
        public void EquipEquipment(InventoryItemData item) // Ajouter tous les bonus possibles venant de l'équipement
        {
            bool pvAtmMax = maxPv.Value == pv;
            EquippableItem equipement = (EquippableItem)item;
            maxPv.AddModifier(new StatsModifier(equipement.PvBonus,StatModType.Flat,item));
            maxPv.AddModifier(new StatsModifier(equipement.PvPercentBonus,StatModType.PercentMult,item));
            strength.AddModifier(new StatsModifier(equipement.StrengthBonus,StatModType.Flat,item));
            strength.AddModifier(new StatsModifier(equipement.StrengthPercentBonus,StatModType.PercentMult,item));
            defense.AddModifier(new StatsModifier(equipement.DefenseBonus,StatModType.Flat,item));
            defense.AddModifier(new StatsModifier(equipement.DefensePercentBonus,StatModType.PercentMult,item));
            intelligence.AddModifier(new StatsModifier(equipement.IntelligenceBonus,StatModType.Flat,item));
            intelligence.AddModifier(new StatsModifier(equipement.IntelligencePercentBonus,StatModType.PercentMult,item));
            if (pvAtmMax)
                pv = maxPv.Value;
        
            HandleAnimationEquipment(true);
        }
    
        public void Disarm(InventoryItemData item) // enlever tous les bonus octroyé par l'équipement
        {
            maxPv.RemoveAllModifiersFromSource(item);
            strength.RemoveAllModifiersFromSource(item);
            defense.RemoveAllModifiersFromSource(item);
            intelligence.RemoveAllModifiersFromSource(item);
            Debug.Log("disarm");
            if (pv > maxPv.Value) // mettre à jour les pv si ils sont décendu
                pv = maxPv.Value;
            
            HandleAnimationEquipment(false); 
        }
    
        private void HandleAnimationEquipment(bool activate)
        {
            if (!isPerformingAction)
            {
                if (activate)
                    this.PlayerAnimatorManager.PlayTargetActionAnimation("Sword_Equip_01", true, false);
                sword.gameObject.SetActive(activate);
                isEquipped = activate;
            }
        }
        
        public void AttemptToPerformAttack()
        {
            // condition pour attaquer : pas mort, arme en main, ne fait rien
            if(isPerformingAction)
                return;
            if (isDead)
                return;
            if (!isEquipped)
                return;
            Debug.Log("try to attack");
            
            this.PlayerAnimatorManager.PlayTargetActionAnimation("Sword_Attack_01", true, true);
            
            Ray r = new Ray(transform.position + new Vector3(0,1), transform.forward);
            if (Physics.Raycast(r, out RaycastHit hit, 10,layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out MonsterEntity monsterEntity))
                {
                    Debug.Log("bim damage");
                    monsterEntity.TakeDamage(this);
                }
            }
        }
        
        private void HandleDie()
        {
            // on evite de mourir en boucle
            if (isDead)
                return;
            Debug.Log("you are die");
            isDead = true;
            this.PlayerAnimatorManager.PlayTargetActionAnimation("Death_01", true); // animation mort
        }
        
        public void UnlockSkill(ScriptableSkill skill)
        {
            foreach (UpgradeData upgradeData in skill.UpgradeDatas)
            {
                if (upgradeData.StatType == StatType.PvMax)
                    maxPv.AddModifier(upgradeData.Modifier);
                else if (upgradeData.StatType == StatType.Force)
                    strength.AddModifier(upgradeData.Modifier);
                else if (upgradeData.StatType == StatType.Defense)
                    defense.AddModifier(upgradeData.Modifier);
                else if (upgradeData.StatType == StatType.Intelligence)
                    intelligence.AddModifier(upgradeData.Modifier);
            }
        }
        
        public void TakeDamage(MonsterData monsterData)
        {
            pv -= monsterData.AttackValue * (1 - defense.Value / 100);
            pv = (float)Math.Round(pv,2);
            CharacterAnimator.SetTrigger("Damage");
            
            if (pv<=0 && !isDead)
            {
                HandleDie();
            }
        }
        
        
        // gestion de l'exp
        public void GainExp(float xp)
        {
            exp += xp;
            while (exp>= necessaryExp)
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            exp -= necessaryExp; 
            level += 1;
            skillPoints += 1;
            necessaryExp *= 1.2f;
        }
        
        
    }
}