using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJFootPoint : MonoBehaviour
{
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!UI.activeSelf && other.CompareTag("Player"))
        {
            print("Player�� trigger�Ǿ����ϴ�");
            UI.gameObject.SetActive(true);
        }

        
    }
}
