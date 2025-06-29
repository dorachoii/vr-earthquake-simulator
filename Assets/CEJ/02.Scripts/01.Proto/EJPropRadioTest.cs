using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPropRadioTest : MonoBehaviour
{
    // public GameObject radioPower;
    public GameObject radioAxis;
    public GameObject gridLine;
    public GameObject pwLight;

    //��ũ��Ʈ�� ���� ������Ʈ�� �ƴ�
    //��ũ��Ʈ���� �ҷ����� ������Ʈ�� trigger üũ�ϴ� ���

    AudioSource audiosource;
    AudioClip radioSFX;

    //01. ������ (����)
    #region Dial Variables

    Quaternion startQ;
    Quaternion endQ;
    Vector3 startRot;
    Vector3 endRot;

    float rotSpeed = 2f;

    bool isOpen;
    #endregion

    //02. line ��ġ �ű��(����)
    float startPos;
    float endPos;

    //03. ���� ����
    public AudioSource noise;
    public AudioSource music;

    float startVolume;
    float endVolume;

    bool isPlaying;
    bool isPowerON;
    bool canTurnVelve;

    //XR Toolkit Controller Variables

    // Start is called before the first frame update
    void Start()
    {  
        //dialRot
        startRot = new Vector3(0, 0, 0);
        startQ = Quaternion.Euler(startRot);

        endRot = new Vector3(0, 0, 90);
        endQ = Quaternion.Euler(endRot);

        //gridLine
        startPos = 0;
        endPos = -0.05f; ;

        //volume
        startVolume = 0;
        endVolume = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            powerONOFF();
        }
        if (canTurnVelve && Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(ControlFrequencyByDial(pwLight.activeSelf, false));
        }
    }

    public IEnumerator ControlFrequencyByDial(bool canTurnVelve, bool isOpen)
    {
        if (canTurnVelve)
        {
            if (!isOpen)
            {
                while (true)
                {
                    //01.dial
                    radioAxis.transform.localRotation = Quaternion.Lerp(radioAxis.transform.localRotation, endQ, Time.deltaTime * rotSpeed);

                    //02.grid
                    float x = Mathf.Lerp(gridLine.transform.localPosition.x, endPos, Time.deltaTime * rotSpeed);
                    gridLine.transform.localPosition = new Vector3(x, 0.09842661f, 0);

                    //03.volume
                    noise.volume = Mathf.Lerp(noise.volume, startVolume, Time.deltaTime * rotSpeed);
                    music.volume = Mathf.Lerp(music.volume, endVolume, Time.deltaTime * rotSpeed);

                    if (Quaternion.Angle(radioAxis.transform.localRotation, endQ) < 0.5f)
                    {
                        radioAxis.transform.localRotation = endQ;
                        isOpen = true;

                        x = endPos;
                        gridLine.transform.localPosition = new Vector3(x, 0.09842661f, 0);

                        noise.volume = 0;
                        music.volume = 1;

                        yield break;
                    }

                    yield return null;
                }
            }
            else
            {
                while (true)
                {
                    //01.dial
                    radioAxis.transform.localRotation = Quaternion.Lerp(radioAxis.transform.localRotation, startQ, Time.deltaTime * rotSpeed);

                    //02.grid
                    float x = Mathf.Lerp(gridLine.transform.localPosition.x, startPos, Time.deltaTime * rotSpeed);
                    gridLine.transform.localPosition = new Vector3(x, 0.09842661f, 0);

                    //03.volume
                    noise.volume = Mathf.Lerp(noise.volume, endVolume, Time.deltaTime * rotSpeed);
                    music.volume = Mathf.Lerp(music.volume, startVolume, Time.deltaTime * rotSpeed);

                    if (Quaternion.Angle(radioAxis.transform.localRotation, startQ) < 0.5f)
                    {
                        radioAxis.transform.localRotation = startQ;
                        isOpen = false;

                        x = startPos;
                        gridLine.transform.localPosition = new Vector3(x, 0.09842661f, 0);

                        noise.volume = 1;
                        music.volume = 0;

                        yield break;
                    }
                    yield return null;
                }
            }
        }
    }

    #region �����ڵ�
    void turnONradio()
    {
        print("turnONradio");
        noise.Play();
        music.Play();
    }

    public void turnOFFradio()
    {
        print("turnOFFradio");
        noise.Stop();
        music.Stop();
    }
    #endregion

    void RadioAction(bool isOn)
    {
        if (isOn)
        {
            noise.Play();
            music.Play();
        }
        else
        {
            noise.Pause();
            music.Pause();
        }
    }

    //radio Power
    public void powerONOFF()
    {
        print("������ power�� ������");

        pwLight.SetActive(!pwLight.activeSelf);
        isPowerON = !pwLight.activeSelf;
        canTurnVelve = pwLight.activeSelf;
        RadioAction(pwLight.activeSelf);

        GetComponentInChildren<EJRadioDial>().enabled = pwLight.activeSelf;

        //EJToDoManager.instance.TodolistON();

        #region �����ڵ�
        //if (!pwLight.activeSelf)
        //{
        //    pwLight.SetActive(true);
        //    isPowerON = true;
        //    canTurnVelve = true;
        //    turnONradio();

        //}
        //else
        //{
        //    pwLight.SetActive(false);
        //    isPowerON = false;
        //    canTurnVelve = false;
        //    turnOFFradio();
        //}
        #endregion
    }
}
