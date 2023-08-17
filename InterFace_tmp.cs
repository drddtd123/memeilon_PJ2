using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InterFace_tmp : MonoBehaviour
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









public class Stat_Get // 스탯 얻는거
{
    int Max_Stat;//최대 
    int Min_Stat;//최소
    int STat;
    public string Stat_names;//HP,ATK,reflect_nerve,Speed 등등..


    public Stat_Get(int Stat_in,string Type_namee,int Min_Stat)
    {
        Max_Stat = Stat_in;
        STat = Stat_in;
        Stat_names = Type_namee;
        this.Min_Stat = Min_Stat;
    }
    public int Stat_out_maxed()
    {
        return  Max_Stat;
    }
    public int Stat_out()
    {
        return STat;
    }

    public string Stat_names_out()
    {
        return Stat_names;
    }

    public int Min_value_out()//최소치는 변경 불가
    {
        return Min_Stat;
    }

    public virtual void MaxStat_UP(int Stat,bool check_Calculate)//얻는 스탯의 양 ,획인 해야할 연산자 TRUE 는 +,- : FALSE는 *,%[추상 가능]
    {
      
        if (check_Calculate)
        {
            Max_Stat += Stat;

        }else
        {
            Max_Stat *= Stat;
        }

    }
    public virtual void MaxStat_Down(int Stat, bool check_Calculate)//잃는 스탯의 양 ,획인 해야할 연산자 TRUE 는 +,- : FALSE는 *,%[추상 가능]
    {
        if (check_Calculate)
        {
            Max_Stat -= Stat;

        }
        else
        {
            Max_Stat %= Stat;
        }
    }
    public virtual void Stat_UP(int Stat, bool check_Calculate)//얻는 스탯의 양 ,획인 해야할 연산자 TRUE 는 +,- : FALSE는 *,%[추상 가능]
    {

        if (check_Calculate)
        {
            STat += Stat;

        }
        else
        {
            STat *= Stat;
        }

    }
    //% or * 계산시 다른 방식으로 할건지 좀더 생각 필요
    public virtual void Stat_Down(int Stat, bool check_Calculate)//잃는 스탯의 양 ,획인 해야할 연산자 TRUE 는 +,- : FALSE는 *,%[추상 가능]
    { 
        if (check_Calculate)
        {
            STat -= Stat;

        }
        else
        {
            STat %= Stat;
        }
    }

}


public class Type_Attribute : Stat_Get
{
    int Maxed_Type_Stat = 0;//각 속성 별로 한계치가 존재하기에..
    bool Maxed_stat = false;//해당 스탯이 최대치 인가?
    int Minimum_type_stat = 0;//디버프 받았을시 최소  수치

    public  Type_Attribute(int Stat_in, string Type_namee, int maxed_Int,int Minimum_type_stat) : base(Stat_in, Type_namee, Minimum_type_stat)
    {
        
        Maxed_Type_Stat = maxed_Int;
       this.Minimum_type_stat = Minimum_type_stat;
    }

    public override void Stat_UP(int Stat, bool check_Calculate)//한계치를 늘리는게 아닌 
    {
        if(Maxed_stat==false)
        {
            base.Stat_UP(Stat, check_Calculate);
        }
     
        if (Stat_out() >= Maxed_Type_Stat)
        {
            Maxed_stat = true;
        }
    }

    public override void Stat_Down(int Stat, bool check_Calculate)
    {
        if(Stat_out() <= Minimum_type_stat)
        {
            //최저 수치
        }else
        {
            base.Stat_Down(Stat, check_Calculate);
        }

      
    }


}


public static class Maxed_stats// 최대 스탯 저장 해놓은거 
{
    public static int Max_Melee = 5;//최대
    public static int Max_Ranger = 5;//최대
    public static int Max_Physics = 7;//최대
    public static int Max_Magic = 7;//최대
    public static int Max_EX_Types = 3;//최대

}



public class Turn// 턴 슈퍼 클래스
{
    static Turn Turn_singleTon;//싱글톤

    int Turn_count = 0;//
   
    public static Turn Trun_on()
    {

        if (Turn_singleTon == null)
        {
            lock (typeof(Turn))// 하나 의 스레드만 접근 가능
            {
                Debug.Log("싱글톤 허락됐음");
                if (Turn_singleTon == null)
                {
                    Turn_singleTon = new Turn();
                }
            }
        }


        return Turn_singleTon;
    }


    public void Turn_UP(int Turn_Plus)
    {
        Turn_count += Turn_Plus;
    }
    public void Turn_Down(int Turn_Plus)
    {
        Turn_count -= Turn_Plus;
    }

    public void Turn_Reset()
    {
        Turn_count = 0;
    }
}



//타입 인터페이스
interface Attribute_Type//속성 - 근접 , 원거리 , 물리 , 마법 , 특수  
{

    public void Attribute_gain();
    public void Attribute_Out();

}


public  class Skill_Type
{//기본 마나 수급기 나 공격기등등은 얘를 상속해서 만들어질 예정
   //대미지 계산 순서
   //해당 캐릭터의 공격력 * 해당 스킬 or 수급기 의 배율 = 대미지 
   //각 캐릭터의 상성에 따라 대미지 배율이 달라짐
   public float Dmg_Persent = 0f;
   public string Skill_Type_name ="Skill_Type";//스킬의 타입 근접 Melee,원거리 Ranger
   public string skill_Attribute = "Skill_A";//스킬의 속성 물리 Physics, 마법 Magic, 특수 EX_Types
   public int cost_gain = 0;// 스킬 사용시 코스트를 어느정도 소모하는가?
     //스킬들의 명중률은 100%다 
    public Skill_Type(float dmg_Persent, string skill_Type_name, string skill_Attribute)
    {
        Dmg_Persent = dmg_Persent;
        Skill_Type_name = skill_Type_name;
        this.skill_Attribute = skill_Attribute;
    }


    public virtual void Skill_use(Ch ch)//스킬 사용시 코스트 관련 처리하는것
    {//스킬을 사용하면 마나를 소모하는 부분
        ch.Mana_cost.Stat_Down(cost_gain,true);
    }//스킬 사용

    public virtual void Skill_Hit(Ch ch, Ch Hited_ch)//스킬 맞을시 처리
    {//스킬을 사용하면 마나를 소모하는 부분
        ch.Mana_cost.Stat_Down(cost_gain, true);
    }//스킬 사용

    public virtual void Skill_Dodged(Ch ch)//상대방이 스킬을 회피한 경우
    {//패널티 요소 써야함
        ch.Mana_cost.Stat_Down(cost_gain, true);
    }

}


