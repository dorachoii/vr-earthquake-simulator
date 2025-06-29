using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleOutlineController : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private float outlineWidth = 2f;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private XRRayInteractor rayInteractor;
    private Outline currentOutline;
    
    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        if (rayInteractor == null)
        {
            rayInteractor = FindObjectOfType<XRRayInteractor>();
        }
        
        if (rayInteractor == null)
        {
            Debug.LogError("[SimpleOutlineController] XRRayInteractor not found!");
            return;
        }
        
        rayInteractor.hoverEntered.AddListener(OnHoverEnter);
        rayInteractor.hoverExited.AddListener(OnHoverExit);
        
        if (showDebugLogs)
            Debug.Log("[SimpleOutlineController] Ready to control outlines");
    }
    
    void OnDestroy()
    {
        if (rayInteractor != null)
        {
            rayInteractor.hoverEntered.RemoveListener(OnHoverEnter);
            rayInteractor.hoverExited.RemoveListener(OnHoverExit);
        }
        
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
    }
    
    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        GameObject targetObject = args.interactableObject.transform.gameObject;
        
        if (showDebugLogs)
            Debug.Log($"[SimpleOutlineController] Hover Enter: {targetObject.name}");
        
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }
        
        EnableOutline(targetObject);
    }
    
    private void OnHoverExit(HoverExitEventArgs args)
    {
        GameObject targetObject = args.interactableObject.transform.gameObject;
        
        if (showDebugLogs)
            Debug.Log($"[SimpleOutlineController] Hover Exit: {targetObject.name}");
        
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
    
    private void EnableOutline(GameObject targetObject)
    {
        Outline outline = targetObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = targetObject.AddComponent<Outline>();
            if (showDebugLogs)
                Debug.Log($"[SimpleOutlineController] Added Outline to {targetObject.name}");
        }
        
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = true;
        
        currentOutline = outline;
        
        if (showDebugLogs)
            Debug.Log($"[SimpleOutlineController] Enabled outline for {targetObject.name}");
    }
} 