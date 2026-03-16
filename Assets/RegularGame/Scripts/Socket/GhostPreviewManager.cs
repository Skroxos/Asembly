using UnityEngine;

namespace DroneAssembly.Socket
{
    public class GhostPreviewManager
    {
        private readonly Material _defaultMat;
        private readonly Material _validMat;
        public GameObject ghost { get; private set; }

        public GhostPreviewManager(Material defaultMat, Material validMat)
        {
            _defaultMat = defaultMat;
            _validMat = validMat;
        }

        public void ShowGhost(GameObject prefab, Transform snapPoint)
        {
            ghost = GameObject.Instantiate(prefab, snapPoint.position, snapPoint.rotation);
            ghost.GetComponent<Collider>().enabled = false;
            ghost.GetComponent<Rigidbody>().isKinematic = true;

            SetGhostMaterial(_defaultMat);
        }

        private void SetGhostMaterial(Material material)
        {
            if (ghost == null) return;
            var renderers = ghost.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers) renderer.material = material;
        }

        public void SetValidGhostMaterial()
        {
            SetGhostMaterial(_validMat);
        }

        public void DisableValidGhostMaterial()
        {
            SetGhostMaterial(_defaultMat);
        }


        public void HideGhost()
        {
            if (ghost != null)
            {
                GameObject.Destroy(ghost);
                ghost = null;
            }
        }
    }
}