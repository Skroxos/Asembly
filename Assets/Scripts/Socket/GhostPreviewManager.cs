using UnityEngine;

public class GhostPreviewManager
{
    private GameObject ghostInstance;
    private Material mat;
    public GameObject ghost => ghostInstance;
    
    public void ShowGhost(GameObject prefab, Transform snapPoint)
    {
        if (ghostInstance != null) return;

        if (mat == null)
        {
            mat = Resources.Load<Material>("GhostPreviewMat");
        }
        
        ghostInstance = GameObject.Instantiate(prefab, snapPoint.position, snapPoint.rotation);
        ghostInstance.GetComponent<Collider>().enabled = false;
        ghostInstance.GetComponent<Rigidbody>().isKinematic = true;
        SetGhostMaterial(mat);
    }
    
    private void SetGhostMaterial(Material material)
    {
        if (ghostInstance == null) return;
        
        Renderer[] renderers = ghostInstance.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = material;
        }
    }

    
    
    public void HideGhost()
    {
        if (ghostInstance != null)
        {
            GameObject.Destroy(ghostInstance);
            ghostInstance = null;
        }
    }
    
}