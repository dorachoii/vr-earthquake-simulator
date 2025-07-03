using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private float outlineWidth = 10f;

    private XRRayInteractor rayInteractor;
    private Outline currentOutline;

    private GameObject currentUI;

    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        if (rayInteractor == null)
        {
            rayInteractor = FindObjectOfType<XRRayInteractor>();
        }

        if (rayInteractor == null) return;

        rayInteractor.hoverEntered.AddListener(OnHoverEnter);
        rayInteractor.hoverExited.AddListener(OnHoverExit);

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

        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }

        EnableOutline(targetObject);
        OnButtonUI(targetObject, true);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        GameObject targetObject = args.interactableObject.transform.gameObject;

        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }

         OnButtonUI(targetObject, false);
    }

    private void EnableOutline(GameObject targetObject)
    {
        Outline outline = targetObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = targetObject.AddComponent<Outline>();
        }

        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = true;

        currentOutline = outline;

       
    }

    private void OnButtonUI(GameObject targetObject, bool isOn)
    {
        if (isOn)
        {
            Transform[] children = targetObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name.Contains("Canvas_Btn"))
                {
                    currentUI = child.gameObject;
                    currentUI.SetActive(true);
                    return;
                }
            }
        }
        else
        {
            if (currentUI != null)
            {
                currentUI.SetActive(false);
                currentUI = null;
            }
        }
    }
} 