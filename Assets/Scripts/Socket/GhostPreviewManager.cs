using UnityEngine;

public class GhostPreviewManager
{
    private Material defaultMat = Resources.Load<Material>("GhostPreviewMat");
    private Material validMat = Resources.Load<Material>("GhostValidPreviewMat");
    public GameObject ghost { get; private set; }

    public void ShowGhost(GameObject prefab, Transform snapPoint)
    {
        if (ghost != null) return;

        if (defaultMat == null) defaultMat = Resources.Load<Material>("GhostPreviewMat");

        ghost = GameObject.Instantiate(prefab, snapPoint.position, snapPoint.rotation);
        ghost.GetComponent<Collider>().enabled = false;
        ghost.GetComponent<Rigidbody>().isKinematic = true;

        SetGhostMaterial(defaultMat);
    }

    private void SetGhostMaterial(Material material)
    {
        if (ghost == null) return;

        var renderers = ghost.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) renderer.material = material;
    }

    public void SetValidGhostMaterial()
    {
        if (validMat == null) validMat = Resources.Load<Material>("GhostValidPreviewMat");
        SetGhostMaterial(validMat);
    }

    public void DisableValidGhostMaterial()
    {
        if (defaultMat == null) defaultMat = Resources.Load<Material>("GhostPreviewMat");
        SetGhostMaterial(defaultMat);
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