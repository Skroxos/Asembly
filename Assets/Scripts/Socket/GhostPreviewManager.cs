using UnityEngine;

public class GhostPreviewManager
{
    private GameObject ghostInstance;
    
    public GameObject ghost => ghostInstance;
    
    public void ShowGhost(GameObject prefab, Transform snapPoint)
    {
        if (ghostInstance != null) return;
        
        ghostInstance = GameObject.Instantiate(prefab, snapPoint.position, snapPoint.rotation);
        ghostInstance.GetComponent<Collider>().enabled = false;
        ghostInstance.GetComponent<Rigidbody>().isKinematic = true;
        SetGhostTransparency(0.5f);
    }

    private void SetGhostTransparency(float alpha)
    {
        Renderer[] renderers = ghostInstance.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
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