using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TestInteractable : MonoBehaviour
{
    void Start()
    {
        // 테스트용 큐브 생성
        CreateTestCube();
    }
    
    void CreateTestCube()
    {
        // 큐브 생성
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "TestInteractableCube";
        cube.transform.position = new Vector3(-2, 1, 0); // slipper 근처에 배치
        
        // Layer 6 (Interactable)로 설정
        cube.layer = 6;
        
        // XRSimpleInteractable 추가
        var interactable = cube.AddComponent<XRSimpleInteractable>();
        
        // Outline 추가
        var outline = cube.AddComponent<Outline>();
        outline.enabled = false; // 처음에는 비활성화
        
        // Rigidbody 추가
        var rb = cube.AddComponent<Rigidbody>();
        rb.isKinematic = true; // 물리 시뮬레이션 비활성화
        
        Debug.Log("[TestInteractable] Created test cube with XRSimpleInteractable");
        
        // 이벤트 연결
        interactable.hoverEntered.AddListener((args) => {
            Debug.Log($"[TestInteractable] Hover Enter: {cube.name}");
            outline.enabled = true;
        });
        
        interactable.hoverExited.AddListener((args) => {
            Debug.Log($"[TestInteractable] Hover Exit: {cube.name}");
            outline.enabled = false;
        });
    }
} 