using UnityEngine;

public class GhostPreviewManager
{
    private GameObject ghostInstance;
    private Material defaultMat = Resources.Load<Material>("GhostPreviewMat");
    private Material validMat = Resources.Load<Material>("GhostValidPreviewMat");
    public GameObject ghost => ghostInstance;

    public void ShowGhost(GameObject prefab, Transform snapPoint)
    {
        if (ghostInstance != null) return;

        if (defaultMat == null)
        {
            defaultMat = Resources.Load<Material>("GhostPreviewMat");
        }

        ghostInstance = GameObject.Instantiate(prefab, snapPoint.position, snapPoint.rotation);
        ghostInstance.GetComponent<Collider>().enabled = false;
        ghostInstance.GetComponent<Rigidbody>().isKinematic = true;
        
        SetGhostMaterial(defaultMat);
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
    
    public void SetValidGhostMaterial()
    {
        if (validMat == null) 
        {
            validMat = Resources.Load<Material>("GhostValidPreviewMat");
        }
        SetGhostMaterial(validMat);
    }
    
    public void DisableValidGhostMaterial()
    {

        if (defaultMat == null)
        {
            defaultMat = Resources.Load<Material>("GhostPreviewMat");
        }
        SetGhostMaterial(defaultMat);
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