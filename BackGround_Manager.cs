using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround_Manager
{

    private static BackGround_Manager Back_m;//싱글톤
    public List<Dictionary<string, object>> Type_Attribute_value_ATK;//속성 배율[공격력]
    public List<Dictionary<string, object>> Type_Attribute_value_DF;//속성 배율[방어력]

    public static BackGround_Manager Start_manager()
    {
        if (Back_m == null)
        {
            lock (typeof(BackGround_Manager))// 하나 의 스레드만 접근 가능
            {
                Debug.Log("싱글톤 허락됐음");
                if (Back_m == null)
                {
                    Back_m = new BackGround_Manager();
                }
            }
        }

        return Back_m;
    }

    public void List_value_Read()//데이터 긁어오기
    {
        Type_Attribute_value_ATK = File_Readerr.Read("Type_Attribute_value_ATK");
        Type_Attribute_value_DF = File_Readerr.Read("Type_Attribute_value_DF");
    }







    public static float Back_value_Atk(string Statnames, int data)
    {
        float Value = 0;
        if (Back_m == null)//싱글턴이 생성되지않은 상태면
        {
            Start_manager();
            Back_m.List_value_Read();
        }
        
        for(int i=0;i< Back_m.Type_Attribute_value_ATK.Count; i++)
        {
            Debug.Log("디버구"+Back_m.Type_Attribute_value_ATK[i][data.ToString()].ToString());

            if (Back_m.Type_Attribute_value_ATK[i]["Type_name"].ToString()== Statnames)
            {
                Value = float.Parse(Back_m.Type_Attribute_value_ATK[i][data.ToString()].ToString());
                break;
            }

        }
            return Value;

    }
    public static float Back_value_DF(string Statnames, int data)
    {
        float Value = 0;
        if (Back_m == null)//싱글턴이 생성되지않은 상태면
        {
            Start_manager();
            Back_m.List_value_Read();
        }

        for (int i = 0; i < Back_m.Type_Attribute_value_DF.Count; i++)
        {
             
            if (Back_m.Type_Attribute_value_DF[i]["Type_name"].ToString() == Statnames)
            {
                Value = float.Parse(Back_m.Type_Attribute_value_DF[i][data.ToString()].ToString());
                break;
            }

        }
        return Value;

    }


    ~BackGround_Manager()//소멸자
    {
        Type_Attribute_value_ATK = null;
        Type_Attribute_value_DF = null; 

    }


    //어느 씬에도 구애받지않고 사용할수있는 스크립트




}
