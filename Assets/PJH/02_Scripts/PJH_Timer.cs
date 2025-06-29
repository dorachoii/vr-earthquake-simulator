using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PJH_Timer : MonoBehaviour
{
    

    // Ÿ�̸� �ؽ�Ʈ ������
    public GameObject timeText;
    TextMeshProUGUI timer;

    // Ÿ�̸� Ȱ�� ����
    bool isTimerStart;

    // �ð� ǥ��
    public Text[] textTime;

    // �ð�
    float maxTime;
    [SerializeField] float time;
    int min;
    int sec;


    

    private void Awake()
    {
        // Ÿ�̸� ������Ʈ ��������
        timer = timeText.GetComponent<TextMeshProUGUI>();
        timer.text = "05 : 00";

        time = 301f;
        maxTime = time;

     }
    // Start is called before the first frame update
    void Start()
    {
        isTimerStart = true;

    } 

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    // Ÿ�̸� �޼���
    void Timer()
    {
        if (isTimerStart == true)
        {
            // �ð��� �귯��
            time -= Time.deltaTime;

            min = (int)(time / 60); // ��
            sec = ((int)(time % 60)); // ��

            timer.text = string.Format("{0:D2} : {1:D2}", min, sec);

        }
    }

} 