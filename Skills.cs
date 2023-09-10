using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    //스킬들 모아둔거
}

public class mana_Gain : Skill_Type // 수급기 
{
    //기본적인 판정 자체는 파티 캐릭터 편성시 만드는걸로 ㅇㅇ
    public mana_Gain(float dmg_Persent, string skill_Type_name, string skill_Attribute,int skill_cost,string Skill_names,float skill_Times) :
    base(dmg_Persent, skill_Type_name, skill_Attribute, Skill_names, skill_Times)
    {
       cost_gain = skill_cost;

    }
    public override void Skill_use(Ch ch)//스킬 사용시 코스트 관련 처리하는것
    {//스킬을 사용하면 마나를 소모하는 부분
        Debug.Log("오버라이드 스킬 유즈"); //수급기라서 암것도 없음
    }//스킬 사용

    public override void Skill_Hit(Ch ch,Ch Hited_ch)//상대방이 맞으면 마나 수급
    {
        ch.Mana_cost.Stat_UP(1, true);//수급기를 통해 마나를 생산
    }
}



