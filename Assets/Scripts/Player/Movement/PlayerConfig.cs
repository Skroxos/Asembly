using UnityEngine;

namespace DroneAssembly.Player.Movement
{
    [CreateAssetMenu(menuName = "Player/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public float Speed;
        public float MouseSensitivity;
        public float VerticalLookLimit;
    }
}
