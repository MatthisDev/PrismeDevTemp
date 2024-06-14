using Unity.Netcode;
using UnityEngine;

namespace Script.Character
{
    /*
     * Concerne tout ce qui touche a l affichage en ligne du personnager
     * - 1) Update de mouvement sur les ecrans de tout le monde
     */
    public class CharacterNetworkManager : NetworkBehaviour
    {   
        // Si on possede le perso alors on peut editer les positions sinon on peut juste le lire
        [Header("Position")] 
        public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }
}