using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public static ShakeManager Instance;

    [Header("Default Shake Settings")]
    [SerializeField] private float defaultDuration = 2f;
    [SerializeField] private float defaultIntensity = 0.1f;
    [SerializeField] private float defaultFrequency = 10f;

    [Header("Shake Config per State")]
    private Dictionary<GameState, (float duration, float intensity, float frequency)> stateShakeConfigs;

    [Header("Object Management")]
    [SerializeField] private List<GameObject> shakeObjects = new List<GameObject>();
    [SerializeField] private string[] shakeObjectTags = { "movable" };

    [Header("Physics Settings")]
    [SerializeField] private bool usePhysicsShake = true;
    [SerializeField] private bool useTransformShake = false;

    private Dictionary<GameObject, Rigidbody> objectRigidbodies = new();
    private Dictionary<GameObject, Vector3> originalPositions = new();
    private Dictionary<GameObject, Quaternion> originalRotations = new();

    private bool isShaking = false;
    private float shakeTimer = 0f;
    private float lastShakeTime = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetupShakeConfigs();
    }

    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    void Start()
    {
        InitializeShakeObjects();
    }

    void Update()
    {
        if (isShaking) UpdateShake();
    }

    private void SetupShakeConfigs()
    {
        stateShakeConfigs = new()
        {
            { GameState.Earthquake, (10f, 0.15f, 15f) },
            { GameState.Aftershock, (5f, 0.045f, 10.5f) },
        };
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Escape)
        {
            StopShake();
            Debug.Log("[ShakeManager] Stopped shake (Escape).");
            return;
        }

        if (stateShakeConfigs.TryGetValue(newState, out var config))
        {
            Debug.Log($"[ShakeManager] Shake started from state: {newState}");
            StartShakeWithSettings(config.duration, config.intensity, config.frequency);
        }
        else
        {
            Debug.LogWarning($"[ShakeManager] No shake config found for {newState}");
        }
    }

    private void InitializeShakeObjects()
    {
        foreach (string tag in shakeObjectTags)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in taggedObjects)
            {
                if (!shakeObjects.Contains(obj)) shakeObjects.Add(obj);
            }
        }

        foreach (GameObject obj in shakeObjects)
        {
            if (obj != null)
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    objectRigidbodies[obj] = rb;
                    originalPositions[obj] = obj.transform.position;
                    originalRotations[obj] = obj.transform.rotation;
                }
            }
        }
    }

    private void StartShakeWithSettings(float duration, float intensity, float frequency)
    {
        SetShakeSettings(duration, intensity, frequency);
        StartShake();
    }

    public void SetShakeSettings(float duration, float intensity, float frequency)
    {
        defaultDuration = duration;
        defaultIntensity = intensity;
        defaultFrequency = frequency;
    }

    public void StartShake()
    {
        if (isShaking) return;

        isShaking = true;
        shakeTimer = 0f;
        lastShakeTime = 0f;

        foreach (var rb in objectRigidbodies.Values)
        {
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }

    public void StopShake()
    {
        if (!isShaking) return;

        isShaking = false;
        foreach (var rb in objectRigidbodies.Values)
        {
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void UpdateShake()
    {
        shakeTimer += Time.deltaTime;

        if (shakeTimer >= defaultDuration)
        {
            StopShake();
            return;
        }

        if (Time.time - lastShakeTime >= 1f / defaultFrequency)
        {
            ApplyShakeForce();
            lastShakeTime = Time.time;
        }
    }

    private void ApplyShakeForce()
    {
        foreach (var kvp in objectRigidbodies)
        {
            Rigidbody rb = kvp.Value;
            if (rb != null && !rb.isKinematic)
            {
                Vector3 force = new(
                    Random.Range(-defaultIntensity, defaultIntensity),
                    Random.Range(-defaultIntensity, defaultIntensity),
                    Random.Range(-defaultIntensity, defaultIntensity)
                );
                rb.AddForce(force, ForceMode.Impulse);

                Vector3 torque = new(
                    Random.Range(-defaultIntensity * 0.5f, defaultIntensity * 0.5f),
                    Random.Range(-defaultIntensity * 0.5f, defaultIntensity * 0.5f),
                    Random.Range(-defaultIntensity * 0.5f, defaultIntensity * 0.5f)
                );
                rb.AddTorque(torque, ForceMode.Impulse);
            }
        }
    }

    public bool IsShaking() => isShaking;

    public float GetRemainingShakeTime() => Mathf.Max(0f, defaultDuration - shakeTimer);

    public void AddShakeObject(GameObject obj)
    {
        if (obj == null || shakeObjects.Contains(obj)) return;

        shakeObjects.Add(obj);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            objectRigidbodies[obj] = rb;
            originalPositions[obj] = obj.transform.position;
            originalRotations[obj] = obj.transform.rotation;
        }
    }

    public void RemoveShakeObject(GameObject obj)
    {
        if (!shakeObjects.Contains(obj)) return;

        shakeObjects.Remove(obj);
        objectRigidbodies.Remove(obj);
        originalPositions.Remove(obj);
        originalRotations.Remove(obj);
    }
}
