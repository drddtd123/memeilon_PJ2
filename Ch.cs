using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

public class Ch 
{
    //GC 가이드 해당 캐릭터 데이터는 교전시에만 사용 
    //교전 시작-> (메모리 할당) -> 교전 끝 -> GC(로딩창과 함께)
    //로딩 과 게임 씬 사이에는 GC를 해주는 편이 메모리 관리에 더 편할것
    public string Ch_names = "A";//캐릭터명
    //게임 시작시 기본 마나가 있어야함 2 정도 

    public Stat_Get Lv;//레벨
    public Stat_Get AWK;//각성 단계

    public Stat_Get HP;//체력
    public Stat_Get Power;//공격력[파워]
    public Stat_Get Defence;//방어력
    public Stat_Get Speed;//스피드 [회피, 속도]
    public Stat_Get Ultara_instinct;//초감각[명중 크리티컬]
    public Stat_Get Mana_cost;//마나 코스트
    public Stat_Get Mana_regeneration;//[마나 리젠] 매턴 당 수급 -> 마나 리젠으로 바꿀것

    //[대미지 강화]
    //해당 옵션들은 버프가 불가
    //[
    public Type_Attribute Melee;//근접 
    public Type_Attribute Ranger;//원거리
    //[방어용]
    public Type_Attribute Physics;//물리
    public Type_Attribute Magic;//마법
    public Type_Attribute abnormal_status;//특수 [높으면 높을수록 상태이상 저항 상승] 
    //]
    //[강화 버프용]
    //강화는 직접적인 스탯 관련만 올려줌 Statget애들 
    public Buff[] Buffed = new Buff[10];// 버프 관리


    //약점을 찔릴시 방어불가 상태 및 회피 확률 감소 

    //캐릭터의 상태를 관리하는 거 만들어야함

    //플레이어 캐릭터만 얼티밋 차지 있음 궁극기 개념  - > [캐릭터 변신] ->   


    //[행동 상태 플래그]
    //[행동 선택 후]
    public State End_State_Flag;//[상태 플래그]0 = 기본 상태 , 1 = 회피 , 2 = 방어 상태
    //[행동 선택]
    public State State_Flag;//[상태 플래그]  0 = 아무것도 안한 상태, 1 = 공격  ,2= 행동 끝난 상태 ,3 = 아무것도 못하는 상태 , 4 = 파티 슬롯에 있는 상태 (살아있음) ,5 = 사망 상태(필드) ,6= 사망 상태(파티 슬롯)


    public bool Dodge_Flag_Success = false;//회피 플래그

    //스킬
    public Skill_Type Nomal_ATTACK;//기본 공격
    public List<Skill_Type> Skill_names;// 스킬들  이름들 기억할 놈



    public Ch(int Lv , string ch_names,int HP_,int ATK , int Speed , int Reflect_nerve,int Melee ,int Ranger,int Phy,int Magic ,int EX_Types,int Defence,int Mana_cost_,int Mana_regeneration, int Mana_regeneration_max,int Mana_cost_max,int CH_AWK,int stat_Flag_END,int Stat_Flag)//생성자
    {
        this.Lv = new Stat_Get(999,Lv,"LV",1);
        Ch_names = ch_names;
        HP = new Stat_Get(HP_, HP_, "HP",0);
        this.Power = new Stat_Get(ATK, ATK, "Power",0);
        this.Speed = new Stat_Get(Speed, Speed, "Speed", 0);
        this.Defence = new Stat_Get(Defence, Defence, "Defence", 0);
        this.Mana_regeneration = new(Mana_regeneration_max, Mana_regeneration, "Mana_regeneration", 0);
        this.Ultara_instinct = new Stat_Get(Reflect_nerve, Reflect_nerve, "Ultara_instinct", 0);
        this.Melee = new Type_Attribute(Melee,"Melee", Maxed_stats.Max_Melee,-Maxed_stats.Max_Melee);
        this.Ranger = new Type_Attribute(Ranger, "Ranger", Maxed_stats.Max_Ranger, -Maxed_stats.Max_Ranger);
        this.Physics = new Type_Attribute(Phy, "Physics", Maxed_stats.Max_Physics, -Maxed_stats.Max_Physics);
        this.Magic = new Type_Attribute(Magic, "Magic", Maxed_stats.Max_Magic, -Maxed_stats.Max_Magic);
        this.abnormal_status = new Type_Attribute(EX_Types, "Magic", Maxed_stats.Max_EX_Types, -Maxed_stats.Max_EX_Types);
        Mana_cost = new Stat_Get(Mana_cost_max, Mana_cost_, "Mana", 0);
        AWK =new(3,CH_AWK,"AWK",0);

        End_State_Flag.Set_Stat(stat_Flag_END);
        State_Flag.Set_Stat(Stat_Flag);
      
    }



    public void End_State_Flag_change(int Flag_changed)//상태 변경
    {
        End_State_Flag.Set_Stat(Flag_changed);
    }

    public void State_Flag_change_(int Flag_changed)//상태 변경
    {
        State_Flag.Set_Stat(Flag_changed);
    }


    //사실상 같은 역활 하는 얘들.. interface로 스탯을 따로 뺀 다음 Max , stat 따로 해줘도 된다 (리팩토링)
    public void Stat_change(int changed_stat, Stat_Get st,bool stat_change, bool Calculate_change)//스탯 변경  True 업 False 다운 ,연산자  True + -  , false * %
    {
        if (stat_change == true)
        {
            st.Stat_UP(changed_stat, Calculate_change);//만약 스탯업이 진행이 안된 정보가 필요하면 추상 클래스를 BOOL을 반환할수있게 변경 해줘야함
        }
        else
        {
            st.Stat_Down(changed_stat, Calculate_change);
        }

    }
    public void Max_Stat_change(int changed_stat, Stat_Get st, bool stat_change, bool Calculate_change)//스탯 변경  True 업 False 다운 ,연산자  True + -  , false * %
    {
        if (stat_change == true)
        {
            st.MaxStat_UP(changed_stat, Calculate_change);
        }
        else
        {
            st.MaxStat_Down(changed_stat, Calculate_change);
        }

    }

    ~Ch()//소멸자
    {
        
        HP = null;
        Power = null;
        Speed = null;
        Ultara_instinct = null;
        Melee = null;
        Physics = null;
        Magic = null;
        abnormal_status = null;
        Debug.Log(Ch_names+"소멸 완료");
        Ch_names = null;
    }

}
