using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class single_ton_use : MonoBehaviour
{
    Singleton singleton;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("싱글톤 접속");

        singleton = Singleton.GetSingleton();
        singleton.Takahate();
        singleton.Explain();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scene_warp()
    {
        Singleton.DeleteSingleton();
        SceneManager.LoadScene("Scene");
     
    }

}
