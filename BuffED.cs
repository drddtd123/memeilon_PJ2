using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffED : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Buff_get : Buff
{

    private string Buff_names;
    private int base_stat;
    private int buff_Stack;


    public Buff_get(string buff_names, int base_stat, int buff_Stack)
    {
        Set_Buffed(buff_names, base_stat, buff_Stack);
    }

    public void Set_Buffed(string Buff_named, int Stat, int Persent)
    { // 해당 버프 명 , 해당 효과를 받는 스탯의 원 스탯 , 해당 버프의 축적도 , 효과 받을 스탯
        Buff_names = Buff_named;
        base_stat = Stat;
        buff_Stack = Persent;

    }

    public string Get_Buffed_names()
    {
        return Buff_names;
    }

    public int Get_Buffed_Persent()
    {
        return buff_Stack;
    }

    public int Get_Buffed_Base_stat()
    {
      return base_stat;
    }
}

