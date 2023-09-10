using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameM_chs
{//게임 매니저 [캐릭터 관리용]싱글톤
 //캐릭터 스킬도 같이 담당

    public string[] Propertiy_Ch_TYPE = { "Nomal_Attack" , "LowCost_Skill_0" ,
        "LowCost_Skill_1", "LowCost_Skill_2", "MiddleCost_Skill_0",
        "MiddleCost_Skill_1","MiddleCost_Skill_2","highCost_Skill_0",
        "highCost_Skill_1","highCost_Skill_2" };
    public string[] Propertiy_Skill_Values = { "Value_0", "Value_1", "Value_2" };
  




    // 게임 시작시 적 정보 와 아군 정보를 가져와서 배틀 씬의 캐릭터 배치를 담당
    //아군 파티를 따로 관리하는 매니저로 분리해야함 0825
    private static GameM_chs GameM_ = null;
    // Start is called before the first frame update

    public Ch[] Player_Party = new Ch[5];//아군 파티

    public Ch[] Enemy_Party = new Ch[9];//적군 파티 [MAX 9]

    public List<Dictionary<string, object>> Ch_Attack_Type;//캐릭터의 기본 공격 및 스킬 타입에 대한 문서
    public List<Dictionary<string, object>> CH_SKill_Table;//캐릭터의 기본 공격 및 스킬 타입에 대한 문서
    public List<Dictionary<string, object>> Skll_values;//캐릭터의 기본 공격 및 스킬 타입에 대한 배율 문서
    public List<Dictionary<string, object>> Skll_Names;//캐릭터의 스킬 명 문서



    public virtual void Start_setting()
    {
        Set();
        FileRead();

    }

    public void FileRead()
    {
        Ch_Attack_Type = File_Readerr.Read("Ch_Attack_Type");
        CH_SKill_Table = File_Readerr.Read("CH_SKill_Table");
        Skll_values = File_Readerr.Read("Skill_Values");
        Skll_Names = File_Readerr.Read("Skill_Names");
    }




    public void Set()//파티 인원 긁어 와서 세팅
    {
        for(int i = 0; i < Player_Party.Length; i++)
        {
            Player_Party[i] = null;

        }

        for (int i = 0; i < Enemy_Party.Length; i++)
        {
            Enemy_Party[i] = null;

        }
    
    }



    public List<Skill_Type> SKill_IN_CH(Ch ch)//캐릭터의 스킬 및 평타 목록을 뽑아주는 얘
    {
        List<Skill_Type> Temp=new();
            if (ch != null)
            {
                int index = Retrun_index_ch(ch.Ch_names);
                for (int j=0;j< Propertiy_Ch_TYPE.Length; j++)
                {
                    if(CH_SKill_Table[index][Propertiy_Ch_TYPE[j]].ToString() != "NULL")
                    {
                        Temp.Add(SKill_ReTurn(CH_SKill_Table[index][Propertiy_Ch_TYPE[j]].ToString()
                       , ch.AWK.Stat_out(),index, Skll_Names[index][Propertiy_Ch_TYPE[j]].ToString()
                       , ch.Ch_names));
                    }

                }


            }
        return Temp;

    }


    public int Retrun_index_ch(string ch_names)//캐릭터 열 가져오는 메소드
    {
        int Index = 0;

        for(int i = 0; i < CH_SKill_Table.Count;i++ )
        {
            if(ch_names == CH_SKill_Table[i]["CH_name"].ToString())
            {
                Index = i; break;   
            }

        }


        return Index;
    }


    
    public bool Check_CH_is_dead(string Ch_names,Ch[] CHs )
    {
        bool Dead_Flag=false;

        for(int i=0;i< CHs.Length; i++)
        {
            if (CHs[i].Ch_names == Ch_names)
            {
                bool ChISDEAD = CHs[i].State_Flag.Get_Stat_names() >= 5;
                if (ChISDEAD)
                {

                    Dead_Flag = true;
                }else
                {
                    Dead_Flag = false;
                }
                break;
            }

        }


        return Dead_Flag;
    }

    public bool Check_CH_iscan_Proceed_Game(string Ch_names, Ch[] CHs)
    {
        bool can_proceed = false;

        for (int i = 0; i < CHs.Length; i++)
        {
            if (CHs[i].Ch_names == Ch_names)
            {
                bool CanPlay = CHs[i].State_Flag.Get_Stat_names().Equals(0);
                if (CanPlay)
                {

                    can_proceed = true;
                }
                else
                {
                    can_proceed = false;
                }
                break;
            }

        }


        return can_proceed;
    }




    //맞는 스킬 타입 넣어주는 메소드
    public Skill_Type SKill_ReTurn(string Skill_name,int CH_AWK,int Ch_Index,string skill_names,string Ch_Names)//스킬 명 ,캐릭터 각성도 ,캐릭터 번호, 해당 캐릭터의 몇번쨰 스킬인지
    {
        Skill_Type Temp = null;
        int index = -1;
        int Skill_values = 0;

        for(int i =0; i< Skll_values.Count;i++)
        {
            if (Skll_values[i]["Skill_names"].ToString()== Skill_name)
            {
                index = i;
                break;
            }

        }
        if(index != -1)
        {
            for(int i=0; i< Propertiy_Skill_Values.Length;i++)
            {
                if (CH_AWK == i )//캐릭터의 각성 수치가 스킬의 수치랑 동일하면
                {
                    Skill_values = int.Parse(Skll_values[index][Propertiy_Skill_Values[i]].ToString());


                }

            }
            Attack_Types Nomal_ATK = Type_Property(Ch_Index, "ATTACK_TYPE");//기본 공격 타입 
            Attack_Types SKILL_ATK = Type_Property(Ch_Index, "Skill_Type");//스킬 공격 타입
            float Anime_Times = Anime_Times_out(Skill_name, Ch_Names);//해당 스킬의 이펙트 타임

            switch (index)// 스킬들 밑에다가 넣어주면 됨 이제
            {
                case 0://mana_Gain [수급기]
                    Temp = new mana_Gain(Skill_values, Nomal_ATK.Type_out(),Nomal_ATK.Property_out(),0, skill_names, Anime_Times);
                    break;
                default:
                    break;
            }


        }

        Debug.Log(skill_names+"스킬 명");


        return Temp;
    }





    public Attack_Types Type_Property(int Ch_Index,string Type_selected)//해당 스킬의 타입 가져오는거
    {
        Attack_Types ATK_TYPE = null;
        string[] Temp = Ch_Attack_Type[Ch_Index][Type_selected].ToString().Split('/');
        ATK_TYPE = new Attack_Types(Temp[0].ToString(), Temp[1].ToString());




        return ATK_TYPE;
    }



    public static GameM_chs SetSpawn()//생성
    {
    
        if (GameM_ == null)
        {
            lock (typeof(GameM_chs))// 하나 의 스레드만 접근 가능
            {
                Debug.Log("싱글톤 허락됐음");
                if (GameM_ == null)
                {
                    GameM_ = new GameM_chs();
                }
            }
            GameM_.Start_setting();
        }

       

        return GameM_;
    }




    public float Anime_Times_out(string SkillName,string  Ch_Names)
    {

        AnimationClip Anime_Clip;
        float Times = 0f;

        Debug.Log("찾으려고 하는 애니메이션 클립"+ Ch_Names+"의"+ SkillName+ "스킬");

        Anime_Clip = Resources.Load<AnimationClip>("CH/Animator/Animation_Clip" + Ch_Names + '-' + SkillName);
        if (null != Anime_Clip)
        {
            Times = Anime_Clip.length;
        }
    
        if(Times == 0f)
        {
            Debug.LogError("현재 찾으려고 하는 애니메이션 클립이 존재하지않습니다");
            Times = 5f;
            Debug.Log("임시 애니메이션  타임 부여 5초");
        }



        return Times;
    }


    public static void Off_Game()//
    {
        GameM_ = null;//비워주기
        GC.Collect();//가비지 돌리기
        GC.WaitForPendingFinalizers();
        //씬 변경
    }

    
    


}
