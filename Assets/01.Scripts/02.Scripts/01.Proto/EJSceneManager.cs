using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EJSceneManager : MonoBehaviour
{
    //public static EJSceneManager instance;


    private void Awake()
    {
        //instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9)) {

            Scene0to1();
        }
    }


    //0: ������� TVȭ��
    //1: ���� �Ͼ�� �� ���� (zoom out)
    //2: GameScene
    //3: GameScene2 (Respawn)
    //4: Ż���

    // ���� ����

    //1: ���� ��ŸƮ ��
    //2: ���� ���ξ� 
    //3. ���� ���� �� 

    public void Scene0to1()
    {
        //���� ����
        //GameStart ��ư ������ �������� �̵�
        SceneManager.LoadScene(1);
    }

    public void Scene1to2() 
    { 
        //���Ƽ���
        //ť�꿡 ������ ���������� �̵�
        SceneManager.LoadScene(2);
    }

    public void Scene2to3() 
    {
        //���ξ�2)
        SceneManager.LoadScene(3);
    }

    public void Scene3to4()
    {
        //Ż���
        SceneManager.LoadScene(4);
    }

    //������ �߰�
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Scene1to2();
            print("Player�� Trigger�� ��Ҵ�");
        }
    }

}
