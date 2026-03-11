using UnityEngine;

namespace DroneAssembly.Socket
{
    [CreateAssetMenu(menuName = "Config/GhostPreviewConfig")]
    public class GhostPreviewConfig : ScriptableObject
    {
        public Material defaultMat;
        public Material validMat;
    }
}