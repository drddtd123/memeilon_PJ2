using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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


    public Stat_Get(int Max_Stat_in,int Stat_in,string Type_namee,int Min_Stat)//최대 스탯 , 일반 스탯 , 타입 명 , 최소 스탯 (최소 보장)
    {
        Max_Stat = Max_Stat_in;
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
    public virtual void MaxStat_change(int Stat)//얻는 스탯의 양 ,획인 해야할 연산자 TRUE 는 +,- : FALSE는 *,%[추상 가능]
    {

        Max_Stat = Stat;

    }
    public virtual void Stat_change(int Stat)//얻는 스탯의 양 ,획인 해야할 연산자 TRUE 는 +,- : FALSE는 *,%[추상 가능]
    {

        STat = Stat;

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
        if(STat+ Stat< Max_Stat)
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
        else /* if (STat+ Stat >= Max_Stat)*/
        {
            Stat_change(Stat_out_maxed());
        }
       

    }
    //% or * 계산시 다른 방식으로 할건지 좀더 생각 필요
    public virtual void Stat_Down(int Stat, bool check_Calculate)//잃는 스탯의 양 ,획인 해야할 연산자 TRUE 는 +,- : FALSE는 *,%[추상 가능]
    {

        if (STat - Stat > Min_Stat)
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
        else 
        {
            Stat_change(Min_value_out());
        }

    }

}


public class Type_Attribute : Stat_Get
{

    public  Type_Attribute(int Stat_in, string Type_namee, int maxed_Int,int Minimum_type_stat) : base(maxed_Int,Stat_in, Type_namee, Minimum_type_stat)
    {

    }

    public override void Stat_UP(int Stat, bool check_Calculate)//한계치를 늘리는게 아닌 
    {
        if(Stat_out()+Stat <= Stat_out_maxed())
        {
            base.Stat_UP(Stat, check_Calculate);
        }
        else if (Stat_out() < Stat_out_maxed())
        {
            base.Stat_change(Stat_out_maxed());
        }
        else
        {
            //스탯 X
        }
    }

    public override void Stat_Down(int Stat, bool check_Calculate)
    {
        if (Stat_out() - Stat > Min_value_out())
        {
            base.Stat_Down(Stat, check_Calculate); //최저 수치
        }
        else if (Stat_out() > Min_value_out()) 
        {
            base.Stat_change(Min_value_out());
        }
        else
        {
           
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



public class Turn: Turn_begin// 턴 슈퍼 클래스
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

    public void Start_Turn()
    {// 턴 시작시 필드 효과가 있는 경우
      
    }

    public void End_Turn()
    {// 턴 종료시 필드 효과가 있는 경우

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
   public string Skill_names = "NULL";// 스킬 이름
   public string Skill_Type_name ="Skill_Type";//스킬의 타입 근접 Melee,원거리 Ranger
   public string skill_Attribute = "Skill_A";//스킬의 속성 물리 Physics, 마법 Magic, 특수 EX_Types
   public int cost_gain = 0;// 스킬 사용시 코스트를 어느정도 소모하는가?
     //스킬들의 명중률은 100%다 
    public Skill_Type(float dmg_Persent, string skill_Type_name, string skill_Attribute,string Skill_names)
    {
        Dmg_Persent = dmg_Persent;
        Skill_Type_name = skill_Type_name;
        this.skill_Attribute = skill_Attribute;
        this.Skill_names = Skill_names;
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

public class Attack_Types 
{
   private string Attack_Type = "A";//근접 , 원거리
   private string Attack_Property = "A";//물리 ,  마법 ,특수


    public Attack_Types(string attack_Type, string attack_Propertiy)
    {
        Attack_Type = attack_Type;
        Attack_Property = attack_Propertiy;
    }

    public string Type_out()
    {
        return Attack_Type;
    }
    public string Property_out()
    {
        return Attack_Property;
    }






}

public interface State_CH //캐릭터 상태 인터페이스
{
    //캐릭터의 상태를 표시
    public void Set_Stat(int State_names);
    public int Get_Stat_names();//상태 이름 가져오기

}


public interface State_Get_Skill
{// 상태에 따라 스킬을 얻는것 [캐릭터의 스킬칸에 스킬이 채워지는게 아닌 디버프 or 매턴 대미지류]
    public void Set_State(string State_names,Skill_Type Skill);
    public string Get_State_names();//상태 이름 가져오기
    public Skill_Type Get_State_Skill();//상태 이름 가져오기
    public bool Get_Effect_Awake_Turn();//해당 효과가  게임 시작시 일어나는 건가[첫턴]
    public bool Get_Effect_Start_Turn();//해당 효과가  매턴 시작시 일어나는 건가

    public bool Get_Effect_End_Turn();//해당 효과가  매턴 종료시 일어나는 건가




}



public interface Turn_begin
{//턴이 시작되고 나서 추가 효과들을 발동 시키는 친구들

    public void Start_Turn();// 턴 시작시 무엇이 있는가 

    public void End_Turn();// 턴 시작시 무엇이 있는가 
}


public interface Buff
{
    //버프 [스탯(방어력, 공격력,스피드, 초감각)류]
    public void Set_Buffed(string Buff_named, int Stat, int Persent)
    { // 해당 버프 명 , 해당 효과를 받는 스탯의 원 스탯 , 해당 버프의 축적도 , 효과 받을 스탯

    }
    public string Get_Buffed_names();// 해당 버프 명 가져오기

    public int Get_Buffed_Persent();// 해당 버프의 축적도 가져오기

    public int Get_Buffed_Base_stat();// 해당 버프를 받기전 원래 스탯



}







