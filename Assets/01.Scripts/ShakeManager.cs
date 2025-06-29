using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public static ShakeManager Instance;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 5f; // 흔들림 지속 시간
    [SerializeField] private float shakeIntensity = 0.1f; // 흔들림 강도
    [SerializeField] private float shakeFrequency = 10f; // 흔들림 빈도 (초당 횟수)
    
    [Header("Object Management")]
    [SerializeField] private List<GameObject> shakeObjects = new List<GameObject>(); // 흔들릴 오브젝트들
    [SerializeField] private string[] shakeObjectTags = { "movable" }; // 흔들릴 오브젝트 태그들
    
    [Header("Physics Settings")]
    [SerializeField] private bool usePhysicsShake = true; // 물리 기반 흔들림 사용
    [SerializeField] private bool useTransformShake = false; // Transform 기반 흔들림 사용
    
    private Dictionary<GameObject, Rigidbody> objectRigidbodies = new Dictionary<GameObject, Rigidbody>();
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> originalRotations = new Dictionary<GameObject, Quaternion>();
    
    private bool isShaking = false;
    private float shakeTimer = 0f;
    private float lastShakeTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeShakeObjects();
    }

    void Update()
    {
        if (isShaking)
        {
            UpdateShake();
        }
    }

    /// <summary>
    /// 흔들림 오브젝트들을 초기화합니다.
    /// </summary>
    private void InitializeShakeObjects()
    {
        // 태그로 오브젝트들을 찾아서 리스트에 추가
        foreach (string tag in shakeObjectTags)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in taggedObjects)
            {
                if (!shakeObjects.Contains(obj))
                {
                    shakeObjects.Add(obj);
                }
            }
        }

        // 각 오브젝트의 Rigidbody와 초기 위치/회전을 저장
        foreach (GameObject obj in shakeObjects)
        {
            if (obj != null)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    objectRigidbodies[obj] = rb;
                    originalPositions[obj] = obj.transform.position;
                    originalRotations[obj] = obj.transform.rotation;
                }
            }
        }
    }

    /// <summary>
    /// 흔들림을 시작합니다.
    /// </summary>
    public void StartShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            shakeTimer = 0f;
            lastShakeTime = 0f;
            
            // 모든 오브젝트의 Rigidbody를 활성화
            foreach (var kvp in objectRigidbodies)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.isKinematic = false;
                    kvp.Value.useGravity = true;
                }
            }
            
            Debug.Log("Shake started!");
        }
    }

    /// <summary>
    /// 흔들림을 즉시 중지합니다.
    /// </summary>
    public void StopShake()
    {
        if (isShaking)
        {
            isShaking = false;
            
            // 모든 오브젝트의 Rigidbody를 Kinematic으로 설정
            foreach (var kvp in objectRigidbodies)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.isKinematic = true;
                    kvp.Value.velocity = Vector3.zero;
                    kvp.Value.angularVelocity = Vector3.zero;
                }
            }
            
            Debug.Log("Shake stopped!");
        }
    }

    /// <summary>
    /// 흔들림을 업데이트합니다.
    /// </summary>
    private void UpdateShake()
    {
        shakeTimer += Time.deltaTime;
        
        // 흔들림 지속 시간이 끝나면 중지
        if (shakeTimer >= shakeDuration)
        {
            StopShake();
            return;
        }

        // 흔들림 빈도에 따라 물리 힘을 적용
        if (Time.time - lastShakeTime >= 1f / shakeFrequency)
        {
            ApplyShakeForce();
            lastShakeTime = Time.time;
        }
    }

    /// <summary>
    /// 흔들림 힘을 적용합니다.
    /// </summary>
    private void ApplyShakeForce()
    {
        foreach (var kvp in objectRigidbodies)
        {
            GameObject obj = kvp.Key;
            Rigidbody rb = kvp.Value;
            
            if (obj != null && rb != null && !rb.isKinematic)
            {
                // 랜덤한 방향의 힘을 적용
                Vector3 randomForce = new Vector3(
                    Random.Range(-shakeIntensity, shakeIntensity),
                    Random.Range(-shakeIntensity, shakeIntensity),
                    Random.Range(-shakeIntensity, shakeIntensity)
                );
                
                rb.AddForce(randomForce, ForceMode.Impulse);
                
                // 랜덤한 회전 힘도 적용
                Vector3 randomTorque = new Vector3(
                    Random.Range(-shakeIntensity * 0.5f, shakeIntensity * 0.5f),
                    Random.Range(-shakeIntensity * 0.5f, shakeIntensity * 0.5f),
                    Random.Range(-shakeIntensity * 0.5f, shakeIntensity * 0.5f)
                );
                
                rb.AddTorque(randomTorque, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// 오브젝트를 리스트에 추가합니다.
    /// </summary>
    public void AddShakeObject(GameObject obj)
    {
        if (obj != null && !shakeObjects.Contains(obj))
        {
            shakeObjects.Add(obj);
            
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                objectRigidbodies[obj] = rb;
                originalPositions[obj] = obj.transform.position;
                originalRotations[obj] = obj.transform.rotation;
            }
        }
    }

    /// <summary>
    /// 오브젝트를 리스트에서 제거합니다.
    /// </summary>
    public void RemoveShakeObject(GameObject obj)
    {
        if (shakeObjects.Contains(obj))
        {
            shakeObjects.Remove(obj);
            objectRigidbodies.Remove(obj);
            originalPositions.Remove(obj);
            originalRotations.Remove(obj);
        }
    }

    /// <summary>
    /// 흔들림 설정을 변경합니다.
    /// </summary>
    public void SetShakeSettings(float duration, float intensity, float frequency)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        shakeFrequency = frequency;
    }

    /// <summary>
    /// 현재 흔들림 상태를 반환합니다.
    /// </summary>
    public bool IsShaking()
    {
        return isShaking;
    }

    /// <summary>
    /// 남은 흔들림 시간을 반환합니다.
    /// </summary>
    public float GetRemainingShakeTime()
    {
        return Mathf.Max(0f, shakeDuration - shakeTimer);
    }

    // 디버그용 GUI (개발 중에만 사용)
    void OnGUI()
    {
        if (Application.isEditor)
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 150));
            GUILayout.Label("Shake Manager Debug");
            GUILayout.Label($"Is Shaking: {isShaking}");
            GUILayout.Label($"Remaining Time: {GetRemainingShakeTime():F1}s");
            GUILayout.Label($"Object Count: {shakeObjects.Count}");
            
            if (GUILayout.Button("Start Shake"))
            {
                StartShake();
            }
            
            if (GUILayout.Button("Stop Shake"))
            {
                StopShake();
            }
            
            GUILayout.EndArea();
        }
    }
} 