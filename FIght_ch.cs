using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIght_ch : Fight
{//플레이어 공격-
    public FIght_ch(float delay) : base(delay)
    {

    }

    public override IEnumerator Coroutine_Fight_Tamplate(Ch Execute_ch, Skill_Type sk, Ch receive_ch)
    {
        
        //이펙트 발동 (캐릭터 애니메이션)

        //행동 선택 -> UI OFF > 스킬 이펙트 ON >
        //
        Debug.Log("시작");
        yield return delay-0.5f;
        Debug.Log("딜레이 끝");
        //행동 선택 -> UI OFF > 스킬 이펙트 off
        //대미지 폰트 생성은 및 표시는  Fight_M 에서 해줘야함
        Fight_M(Execute_ch, sk, receive_ch);
        yield return 1f;//대미지 폰트 생성 및 잔류 시간
        Debug.Log("익스큐트" + Execute_ch.Mana_cost.Stat_out());
        Debug.Log("리시브"+receive_ch.HP.Stat_out());
        //대미지 폰트 OFF
        //턴 넘어감
    }



    public override void Ch_anime_hook(Ch ch)
    {
     
    }

   
    

}



public class Fight_Struct{//구조체

   public Ch Execute_ch;//공격하는 놈
   public Skill_Type sk;//스킬
   public Ch receive_ch;//공격 받는 놈
    public Fight_Struct(Ch Execute_ch, Skill_Type sk, Ch receive_ch)
    {

        this.Execute_ch = Execute_ch;
        this.sk = sk;
        this.receive_ch = receive_ch;
    }


}

