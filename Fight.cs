using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Fight : MonoBehaviour 
{
    //싸우는거 템플릿 메소드 화 시킬거임
    public float delay = 0f;//공격까지 딜레이 타임 [wait Times]-> 공격하는 대상의 애니메이션 타임을 받아야함

    //
    public Fight(float delay)
    {
        this.delay = delay;
        //빈 생성자
    }


    public virtual IEnumerator Coroutine_Fight_Tamplate(Ch Execute_ch, Skill_Type sk, Ch receive_ch) //<- 교전 방식 오버라이드 해서 사용해줘야함
    {
        yield return null;
    }


    public virtual void Fight_M(Ch Execute_ch, Skill_Type sk, Ch receive_ch)//대미지 주는 얘 , 대미지 받는 얘
    {//코루틴으로 바꾸고 anime 훅 자리에 waitforseconds 넣어줘야함
        Debug.Log("딜 발동");
        sk.Skill_use(Execute_ch);//스킬을 사용한 대상에 대해 마나 코스트 처리
        Ch_anime_hook(Execute_ch);//(공격하는 CH)애니메이션 후크 -> 상속 받는 얘가 만들어주면 됨 
        Damage_ch(Execute_ch,sk, receive_ch);//1 대미지에 대한 처리
        Ch_anime_hook(receive_ch);//2(공격 받는 CH 후크)애니메이션 후크
        Fight_EndHook(receive_ch, Execute_ch);//해당 싸움이 끝났을시 일어나야할 이벤트가 있을지도 모르니 후크를 마련한다
    }
    //공격 계산식
    //기본 공격력 * 해당 타입 적응도 
    // 대미지 강화 [근접, 원거리 배율]
    // 0 = 1 배율 , 1 = 1.25 , 2 = 1.4. 3  = 1.6 ,4 = 1.7 5 = 2 
    // -1 = 0.9   , -2 = 0.75 , - 3  = 0.6  , - 4  = 0.5 ,-5 = 0.45  
    // 대미지 강화 [마법, 물리 배율]
    // 0 = 0.0 배율 , 1 = 0.1 , 2 = 0.2. 3  = 0.3 ,4 = 0.4 5 = 0.5 

    public  abstract void Ch_anime_hook(Ch ch);//공격 모션 [끝] -> 기본 상태로 돌림

    public void delay_Change(float delay_change)
    {
        delay = delay_change;
    }

    public virtual void Damage_ch(Ch Execute_ch, Skill_Type skill,Ch receive_ch)
    {
        bool Hit_Flag = true;//receive_ch 공격받을떄  TRUE or 회피 했을떄 false 
        int dmg =  Calculate_Dmg_Excute(skill, Execute_ch);//공격하는 얘의 계산된 값
        float DEF = Get_value_data_DF(receive_ch, skill);//대미지를 받아야하는 캐릭의 계수
        float Calculate_end_Damage = 0;//상대 방어 계수까지 계산된 대미지

        Debug.Log("대미지" + dmg);
        Debug.Log("방어 배율" + DEF);
        int Crit = Random.Range(0, 100);
        Debug.Log("크리티컬 확률" + Execute_ch.Ultara_instinct.Stat_out());
        Debug.Log("크리티컬 확률2"+Crit);
        if(Execute_ch.Ultara_instinct.Stat_out()>= Crit)
        {
            Calculate_end_Damage= (dmg* DEF)*1.2f;//크리티컬
        }
        else
        {
            Calculate_end_Damage = (dmg * DEF);//노 크리티컬
        }
        Debug.Log("대미지[크리티컬 적용]" + Calculate_end_Damage);
        //receive_ch.End_State_Flag = 2;
        //상대의 상태에 따라 대미지가 다름
        if (receive_ch.End_State_Flag.Get_Stat_names() == 0)//기본 상태
        {
            receive_ch.HP.Stat_Down((int)Math.Ceiling(Calculate_end_Damage), true);
        }
        else  if (receive_ch.End_State_Flag.Get_Stat_names() == 1)//회피 상태
        {
            if (Dodge_Check(receive_ch.Speed.Stat_out(), Execute_ch.Ultara_instinct.Stat_out()))//True면 회피 성공
            {
                Debug.Log("회피!!");
                receive_ch.Dodge_Flag_Success = true;//회피 성공 플래그
                Hit_Flag = false;
            }
            else // 회피 실패
            {
                Debug.Log("회피 실패");
                receive_ch.HP.Stat_Down((int)Math.Ceiling(Calculate_end_Damage * 1.2f), true);
            }
        }
        else if (receive_ch.End_State_Flag.Get_Stat_names() == 2)//방어 상태
        {
            Debug.Log("방어!!" + Calculate_end_Damage * 0.7f);
            receive_ch.HP.Stat_Down((int)Math.Ceiling(Calculate_end_Damage * 0.7f), true);
        }

        if(Hit_Flag == true)//대미지 입은거
        {
            skill.Skill_Hit(Execute_ch, receive_ch);// 해당 스킬에 대한 소유자를 지정해주면 여기서 입력 받지않아도 사용 가능
        }
        else if (Hit_Flag == true) //회피 한것
        {
            skill.Skill_Dodged(Execute_ch);//상대가 회피했을시 시전자에게 패널티가 있다면..
        }

        Debug.Log(receive_ch.HP.Stat_out());
    }

    public virtual void Fight_EndHook(Ch receive_ch, Ch Execute_ch)//후크
    {
        receive_ch.Dodge_Flag_Success = false;
        receive_ch.End_State_Flag.Set_Stat(0);
    }
    public virtual bool Dodge_Check(int receive_ch_Ultara_instinct, int Execute_ch_receive_ch_Ultara_instinct)
    //receive_ch_Ultara_instinct 는 대미지를 받는 얘의 회피 확률 
    {//Execute_ch_receive_ch_Ultara_instinct는 공격하는 얘의 명중률임
        float Dodge = 30f/*기본 확률*/ +(receive_ch_Ultara_instinct - Execute_ch_receive_ch_Ultara_instinct);
        if(Dodge >=70f)//회피 확률 고정
        {
            Dodge = 70f;
        }
        Debug.Log("회피 확률" + Dodge);
        float Random_dodge = Random.Range(0, 100);

        Debug.Log("회피 확률2=" + Random_dodge);
        if (Random_dodge <= Dodge)
        {
            return true;
        }else
        {
            return false;
        }


       
    }



    public virtual int Calculate_Dmg_Excute(Skill_Type sk, Ch Execute_ch)//스킬 + 캐릭터 스탯에 비례하여 사용한스킬의 정확한 대미지 뽑아줌[상대 방어 계산 X]
    {
        int sums2 = Get_value_data_ATK(Execute_ch, sk);
        return sums2;
    }


    private int Get_value_data_ATK(Ch Execute_ch, Skill_Type skill)//대미지 계산 된것
    {
        float Type_Value = 0;//근거리 , 원거리
        float Attribute_Value = 0;//물리 , 마법 , 특수
      

        if (skill.Skill_Type_name== "Melee")
        {
            Type_Value = BackGround_Manager.Back_value_Atk(skill.Skill_Type_name, Execute_ch.Melee.Stat_out());//해당 하는 타입의 계수 가져옴
        }
        else if (skill.Skill_Type_name== "Ranger")
        {
            Type_Value = BackGround_Manager.Back_value_Atk(skill.Skill_Type_name, Execute_ch.Ranger.Stat_out());//해당 하는 타입의 계수 가져옴
        }

        if(skill.skill_Attribute == "Physics")
        {
            Attribute_Value = BackGround_Manager.Back_value_Atk(skill.skill_Attribute, Execute_ch.Physics.Stat_out());//해당 하는 타입의 계수 가져옴
        }
        else if (skill.skill_Attribute == "Magic")
        {
            Attribute_Value = BackGround_Manager.Back_value_Atk(skill.skill_Attribute, Execute_ch.Magic.Stat_out());//해당 하는 타입의 계수 가져옴
        }
        else//EX 타입
        {
            Attribute_Value = BackGround_Manager.Back_value_Atk(skill.skill_Attribute, Execute_ch.abnormal_status.Stat_out());//해당 하는 타입의 계수 가져옴
        }
        Debug.Log("1-"+Type_Value);
        Debug.Log("2-"+Attribute_Value);
       
        double sums = (Execute_ch.Power.Stat_out() * (Type_Value + Attribute_Value)) * (skill.Dmg_Persent*0.01f);//[계산식(공격력 * (타입 배율+타입 배율) * 스킬 배율 = 대미지]
        return (int)Math.Ceiling(sums);//올림 처리
    }


    private float Get_value_data_DF(Ch receive_ch, Skill_Type skill)//방어 계수 [대미지 얼마만큼 경감 받을것인지]
    {
        float Type_Value = 0;//근거리 , 원거리
        float Attribute_Value = 0;//물리 , 마법 , 특수
        if (skill.Skill_Type_name == "Melee")
        {
            Type_Value = BackGround_Manager.Back_value_DF(skill.Skill_Type_name, receive_ch.Melee.Stat_out());//해당 하는 타입의 계수 가져옴
        }
        else if (skill.Skill_Type_name == "Ranger")
        {
            Type_Value = BackGround_Manager.Back_value_DF(skill.Skill_Type_name, receive_ch.Ranger.Stat_out());//해당 하는 타입의 계수 가져옴
        }

        if (skill.skill_Attribute == "Physics")
        {
            Attribute_Value = BackGround_Manager.Back_value_DF(skill.skill_Attribute, receive_ch.Physics.Stat_out());//해당 하는 타입의 계수 가져옴
        }
        else if (skill.skill_Attribute == "Magic")
        {
            Attribute_Value = BackGround_Manager.Back_value_DF(skill.skill_Attribute, receive_ch.Magic.Stat_out());//해당 하는 타입의 계수 가져옴
        }
        else//EX 타입
        {
            Attribute_Value = BackGround_Manager.Back_value_DF(skill.skill_Attribute, receive_ch.abnormal_status.Stat_out());//해당 하는 타입의 계수 가져옴
        }

        float sums = (100 / ((100 + receive_ch.Defence.Stat_out()) * (Attribute_Value + Type_Value)));
        return sums;
    }



    //Ex 어떠한 캐릭터가 공격력이 24고 사용한 스킬이 물리 , 근접 이고 해당 캐릭터가 근접 +3 ,물리 +1인 경우
    //EX) 24 * (1.6+0.1)= 41(40.8 지만 올림처리) =  [계산식(공격력 * (타입 배율+타입 배율) * 스킬 배율 = 대미지]


    //방어 타입은 물리 , 마법 > 근접.원거리 순 
    //방어 배율  [물리,마법]
    // 0 = 1 배율 , 1 = 1.25 , 2 = 1.4. 3  = 1.6 ,4 = 1.7 5 = 2 
    // -1 = 0.9   , -2 = 0.75 , - 3  = 0.6  , - 4  = 0.5 ,-5 = 0.45  
    //방어 배율 [근접 , 원거리] - 근접과 원거리는 - 쪽에는 방어계수가 없다
    // 0 = 0.0 배율 , 1 = 0.1 , 2 = 0.2. 3  = 0.3 ,4 = 0.4 5 = 0.5 



    //그리고 대미지를 받는 대상이 방어력이 50 에 물리 + 2 , 근접 1에 상대방의 공격이 근접이라면 
    // 41 *(100/(100+50)*(1.4+0.1))=  17(올림 처리 16.4)        [ 대미지 *( 100/((100+ 방어력)* (방어 타입 배율+방어타입 배율)))  ] 

    //[대미지 공식]-((공격력 * 타입 배율) * 스킬 배율) * ( 100/((100+ 방어력)* (방어 타입 배율+다른(상대방이 입히는 대미지 종류) 방어 타입)))
}
