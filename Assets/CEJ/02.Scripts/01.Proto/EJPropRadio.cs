using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

public class EJPropRadio : MonoBehaviour
{
    public Transform hand;
    public Transform tempHand1;
    public Transform tempHand2;

    #region dial Variables

    public GameObject dialAxis;
    public GameObject gridLine;

    float gridLinePosX;
    float gridLineStartPosX = 0;
    float gridLineEndPosX = -0.05f;

    bool bRotate;

    Quaternion startQ;
    Quaternion endQ;
    Vector3 startRot;
    Vector3 endRot;

    float rotSpeed = 1f;

    bool isOpen;

    #endregion

    #region SFX
    public AudioSource noise;
    public AudioSource broadCast;
    float noiseVolume;
    float broadcastVolume;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        noiseVolume = noise.volume;
        broadcastVolume = broadCast.volume;

        //���� ����
        startRot = new Vector3(0, 0, 0);
        startQ = Quaternion.Euler(startRot);
        endRot = new Vector3(0, 0, 90);
        endQ = Quaternion.Euler(endRot);

        
    }

    // Update is called once per frame
    void Update()
    {
        //float angle = Vector3.Angle(Vector3.up, dialAxis.transform.up);
        //angle = Mathf.Clamp(angle, 0, 180);

        //noiseVolume = 1 - (angle / noiseVolume);
        //broadcastVolume = angle / broadcastVolume;

        #region grabbable Rotation�� �ٲ� �ڵ�
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    bRotate = true;
        //    tempHand1.localEulerAngles = new Vector3(0, 0,hand.transform.localEulerAngles.z);
        //}

        //if (bRotate)
        //{
        //    //angle between temp1, temp2
        //    tempHand2.localEulerAngles = new Vector3(0, 0, hand.localEulerAngles.z);

        //    float angle = Vector3.Angle(tempHand1.up, tempHand2.up);
        //    print("RadioDialAngle��" + angle);

        //    //direction
        //    int dir = 1;

        //    //���������� ������ ���̰��� 90�� ������ �� �ۿ� ����.
        //    //��, �������� ���
        //    if (Vector3.Dot(tempHand2.up, tempHand1.right) > 0)
        //    {
        //        dir = -1;
        //    }

        //    dialAxis.transform.Rotate(0, 0, angle * dir);

        //    //������ ���� temp1�� temp2�� ���� �ϸ� �� �̻� ȸ������ ����
        //    tempHand1.localEulerAngles = tempHand2.localEulerAngles;
        //}
        #endregion

        #region ����
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    print("dial�� ������ �� rotation����" + dialAxis.transform.localRotation);
        //    print("dial�� ������ �� noise����" + noiseVolume);
        //    print("dial�� ������ �� broadcast����" + broadcastVolume);
        //    print("dial�� ������ �� gridLine�� positionX ����" + gridLinePosX);

        //    DialRot(dialAxis);

        //    print("dial�� ���� ���� rotation����" + dialAxis.transform.localRotation);
        //    print("dial�� ���� ���� noise����" + noiseVolume);
        //    print("dial�� ���� ���� broadcast����" + broadcastVolume);
        //    print("dial�� ���� ���� gridLine�� positionX ����" + gridLinePosX);
        //}
        #endregion

    }

    public IEnumerator DialRot(GameObject dial)
    {
        gridLinePosX = gridLine.transform.position.x;
        noise.Play();
        broadCast.Play();

        if (!isOpen)
        {
            while (true)
            {
                dial.transform.localRotation = Quaternion.Lerp(dial.transform.localRotation, endQ, Time.deltaTime * rotSpeed);
                

                gridLinePosX = Mathf.Lerp(gridLinePosX, gridLineEndPosX, Time.deltaTime * rotSpeed);

                noiseVolume = Mathf.Lerp(1, 0, Time.deltaTime * rotSpeed);
                broadcastVolume = Mathf.Lerp(0, 1, Time.deltaTime * rotSpeed);

                if (Quaternion.Angle(dial.transform.localRotation, endQ) < 0.5f)
                {
                    dial.transform.localRotation = endQ;
                    isOpen = true;

                    noiseVolume = 0;
                    broadcastVolume = 1;
                    break;
                }

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                dial.transform.localRotation = Quaternion.Lerp(dial.transform.localRotation, startQ, Time.deltaTime * rotSpeed);

                gridLinePosX = Mathf.Lerp(gridLinePosX, gridLineStartPosX, Time.deltaTime * rotSpeed);

                noiseVolume = Mathf.Lerp(0, 1, Time.deltaTime * rotSpeed);
                broadcastVolume = Mathf.Lerp(1, 0, Time.deltaTime * rotSpeed);

                if (Quaternion.Angle(dial.transform.localRotation, startQ) < 0.5f)
                {
                    dial.transform.localRotation = startQ;
                    isOpen = false;

                    noiseVolume = 1;
                    broadcastVolume = 2;

                    break;
                }
                yield return null;
            }
        }
    }
}