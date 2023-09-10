using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;






public class Singleton
{
    public int count = 0;
    public GameObject gms;
        public static Singleton Instance=null;
        private Singleton()
        {
        count = 0;
        }

    public static Singleton GetSingleton()
    {
        if (Instance == null)
        {
            lock (typeof(Singleton))// 하나 의 스레드만 접근 가능
            {
                Debug.Log("싱글톤 허락됐음");
                if (Instance == null)
                {
                    Instance = new Singleton();
                }
            }
        }

        return Instance;
    }
    
    public void Explain()
    {
        
        if(gms == null)
        {
            Debug.Log("나니 얏 뗀다 오마에");
        }else
        {
            Debug.Log("obj 존재중");
        }

    }

    public static void DeleteSingleton()
    {
        Instance = null;
    }
    public void Takahate()
    {
        
        count++;
        Debug.Log(count+"俺の名前わりくむ");
    }

    ~Singleton()
    {
        Debug.Log("소멸자 완료");

    }
}
