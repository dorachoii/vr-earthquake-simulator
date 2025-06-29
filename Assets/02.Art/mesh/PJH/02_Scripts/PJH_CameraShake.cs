using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_CameraShake : MonoBehaviour
{
    public Camera Camera;       //��鸮��� ī�޶�
    public Camera MainCamera;   //���ۿ� ī�޶�
    Vector3 cameraPos;

    [SerializeField]
    [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField]
    [Range(1f, 5f)] float duration = 3f;

    //public GameObject playerUI;

    public void Shake()
    {
        cameraPos = Camera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }
    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange *2 - shakeRange;
       // float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = Camera.transform.position;
        cameraPos.x += cameraPosX;
        //cameraPos.y += cameraPosY;
        Camera.transform.position = cameraPos;

    }
    void StopShake()
    {
        CancelInvoke("StartShake");
        Camera.transform.position = cameraPos;
        Camera.gameObject.SetActive(false);     //������ ���� �� ī�޶� off
        MainCamera.gameObject.SetActive(true);  //������ ���� �� mainCamera on
        //playerUI.SetActive(true);
    }

    public void Aftershocks()
    {
        Camera.transform.position = MainCamera.transform.position;
        Camera.transform.forward = MainCamera.transform.forward;
        Camera.gameObject.SetActive(true);     //��������, shakeCam on
        MainCamera.gameObject.SetActive(false);  //MainCam off

        
        //cameraPos = MainCamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }


    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shake", 6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
