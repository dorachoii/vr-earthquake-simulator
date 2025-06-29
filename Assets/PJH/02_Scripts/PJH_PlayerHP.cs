using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PJH_PlayerHP : MonoBehaviour
{
    public float MaxHP = 0f;
    public Image sliderHP;
    bool slipperOn = false;
    public float FloorDamage = 0.3f; //�ٴ� ������
    public float CellingDamage = 0.2f; //õ�� ������
    //public GameObject Hit;
    public GameObject slipper;
    
    
    void Start()
    {
        //MAXHP();
    }
    void MAXHP()
    {
        sliderHP.fillAmount = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Damage(int a)
    {
        // 1 = �ٴ� ������
        // 2 = õ�� ������
        // �����۰� ���������� �ž���
        if(slipper.activeSelf == false) slipperOn = true;

        if (a == 1)
        {
            if (slipperOn == false) // ������ ����� ������ 0
            {
                sliderHP.fillAmount += FloorDamage;
                //Hit.SetActive(true);
                //Hit.SetActive(false);
            }
        }
        else if (a == 2)
        {
            sliderHP.fillAmount += CellingDamage;
            //Hit.SetActive(true);
            
            //Hit.SetActive(false);
        }        
    }
}
