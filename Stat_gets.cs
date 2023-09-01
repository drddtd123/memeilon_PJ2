using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_gets : MonoBehaviour
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
// 상태들 다 떄려박을 곳


public class State : State_CH
{// 현재 캐릭터의 상태를 담아두고 있는 클래스
    private int Stat_names;//현재 상태에 대한 이름
    //[상태 플래그]  0 = 아무것도 안한 상태, 1 = 공격  ,2= 행동 끝난 상태 ,3 = 아무것도 못하는 상태 , 4 = 파티 슬롯에 있는 상태 (살아있음) ,5 = 사망 상태(필드) ,6= 사망 상태(파티 슬롯)
    public int Get_Stat_names()
    {
        return Stat_names;
    }

    public void Set_Stat(int Stat_names)
    {
        this.Stat_names = Stat_names;
    }


}

