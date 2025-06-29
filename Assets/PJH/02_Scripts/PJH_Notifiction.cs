using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PJH_Notifiction : MonoBehaviour
{
    public static PJH_Notifiction instance;

    public Text Notifiction;
    public GameObject NOT;
    public int textTime = 5;
    AudioSource[] arrayAudio;
     

    private void Awake()
    {
        instance = this;
        arrayAudio = NOT.GetComponents<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartMessage();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void UnHideNotifiction()
    {
        NOT.SetActive(true);
    }
    public void HideNotifiction()
    {
        NOT.SetActive(false);
    }

    public void StartMessage()     //���� ���� �޼��� �־�ý��ϴ�. Start()���� �����ϰ�, ��������
    {
        UnHideNotifiction();
        Notifiction.text = "���ݺ��� �����ùķ��̼��� �����մϴ�.";
        arrayAudio[0].Play();
        Invoke("HideNotifiction", textTime);
    }

    public void Glass()             //��������
    {
        UnHideNotifiction();
        Notifiction.text = "�������� ������ ������ �� �����ϴ�.";
        arrayAudio[1].Play();
        Invoke("HideNotifiction", textTime);
    }

    public void Radio()             //����
    {
        UnHideNotifiction();
        Notifiction.text = "������ �Ѽ� ���� ��� �ൿ��Ģ�� �ľ��ϼ���.";
        arrayAudio[2].Play();
        Invoke("HideNotifiction", textTime);
    }

    public void RCD()               //�������ܱ�
    {
        UnHideNotifiction();
        Notifiction.text = "�������ܱ⸦ ���� ���⸦ �����ϼ���.";
        arrayAudio[3].Play();
        Invoke("HideNotifiction", textTime);
    }

    public void Exit()               //Ż�ⱸ ����
    {
        UnHideNotifiction();
        Notifiction.text = "���Թ��� �̸� ����, Ż��θ� Ȯ���ϼ���.";
        arrayAudio[4].Play();
        Invoke("HideNotifiction", textTime);
    }

    public void Velve()               //���� ��ױ�
    {
        UnHideNotifiction();
        Notifiction.text = "���긦 �ᰡ ������ �����ϼ���.";
        arrayAudio[5].Play();
        Invoke("HideNotifiction", textTime);
    }
    public void AfterShock()               //����
    {
        UnHideNotifiction();
        Notifiction.text = "������ �߻��Ͽ����ϴ�.";
        arrayAudio[6].Play();
        Invoke("HideNotifiction", textTime);
    }

    public void Tablet()               //�º�
    {
        UnHideNotifiction();
        Notifiction.text = "�º��� ���� �ǳ���Ҹ� Ȯ���ϼ���.";
        arrayAudio[7].Play();
        Invoke("HideNotifiction", textTime);
    }

}
