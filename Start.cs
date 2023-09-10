using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Sss();
        }
    }


    IEnumerator Corutine_1()
    {


        Debug.Log("코루틴1 돌아가는중");
      
       
        for(int i = 0; i < 10; i++)
        {
            Debug.Log(i + "카운팅");
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("코루틴1 끝");
    }


    IEnumerator Corutine_2()
    {


        Debug.Log("코루틴2 돌아가는중");
        yield return new WaitForSeconds(5f);

        Debug.Log("코루틴2 끝");
    }


    public void Sss()
    {
        StartCoroutine(Corutine_1());
        Debug.Log("코루틴2가 돌아갈 예정");
        StartCoroutine(Corutine_2());
    }
}
