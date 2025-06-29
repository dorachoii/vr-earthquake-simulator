using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractorLayerFixer : MonoBehaviour
{
    void Start()
    {
        FixRayInteractorLayers();
    }
    
    void FixRayInteractorLayers()
    {
        // 모든 Ray Interactor 찾기
        var rayInteractors = FindObjectsOfType<XRRayInteractor>();
        
        Debug.Log($"[RayInteractorLayerFixer] Found {rayInteractors.Length} Ray Interactors");
        
        foreach (var rayInteractor in rayInteractors)
        {
            // Layer 6 (Interactable) 마스크 생성
            int layer6Mask = 1 << 6; // Layer 6
            
            // 현재 RaycastMask에 Layer 6이 포함되어 있는지 확인
            bool includesLayer6 = (rayInteractor.raycastMask.value & layer6Mask) != 0;
            
            Debug.Log($"[RayInteractorLayerFixer] {rayInteractor.name} - Current RaycastMask: {rayInteractor.raycastMask.value}");
            Debug.Log($"[RayInteractorLayerFixer] {rayInteractor.name} - Includes Layer 6: {includesLayer6}");
            
            if (!includesLayer6)
            {
                // Layer 6을 RaycastMask에 추가
                rayInteractor.raycastMask |= layer6Mask;
                Debug.Log($"[RayInteractorLayerFixer] Added Layer 6 to {rayInteractor.name}. New RaycastMask: {rayInteractor.raycastMask.value}");
            }
            else
            {
                Debug.Log($"[RayInteractorLayerFixer] {rayInteractor.name} already includes Layer 6");
            }
        }
    }
} 