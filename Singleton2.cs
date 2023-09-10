using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton2 : MonoBehaviour
{
    Singleton singleton;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("싱글톤 접속2");

        singleton = Singleton.GetSingleton();
        singleton.Takahate();
        singleton.Explain();

    }

}
