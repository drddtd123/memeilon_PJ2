using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class GameM_Turn
{//프록시 패턴
    //턴 관리하는 친구
    private Turn Turn_set;//턴
    private bool GameFlag_END_LEFT = false;
    //왼쪽 진형의 행동이 종료 되었는가? False면 종료된거 TRUE면 진행중
    private bool GameFlag_END_Right = false;
    //우측 진형의 행동이 종료 되었는가? False면 종료된거 TRUE면 진행중

    //1 안 [포켓몬] - >  속전속결 가능 
    // 1턴에 동시 결정(상대방과 내가)이 가능하고 Speed 값에 따라 서로의 순서를 정한다 
    // 서로의 행동이 정해지면 턴이 진행




    public void Awake_Turn()
    {//스테이지 정보 읽고 시작 턴수 읽기
        Turn_set = new Turn(1);
    }

    public void Turn_UP(int Turn_int)
    {
        Turn_set.Turn_UP(Turn_int);
    }

    public void Turn_Down(int Turn_int)
    {
        Turn_set.Turn_Down(Turn_int);
    }

    public int Check_Turn()//턴 리턴
    {
        return Turn_set.back_Turn();
    }

    public void Game_Set()
    {
        GameFlag_END_LEFT = true;
        GameFlag_END_Right = true;

    }


    public void END_OF_The_Turn(bool Flag/*flase면 왼 , TRUE면 오른쪽*/)
    {//턴 종료 메소드
        if (Flag)
        {
            GameFlag_END_Right = false;
        }
        else { GameFlag_END_LEFT = false; }
    }



    public bool Start_Check_Turn(bool Flag/*flase면 왼 , TRUE면 오른쪽*/)
    {//각 진영의 상태를 가져옴
     // 서버 추가시 여기에서 통신 관련을 추가해줘야함
        if (Flag)
        {
            return GameFlag_END_Right;
        }
        else { return GameFlag_END_LEFT; }
    }
    

    public virtual int Start_Corutine_Turn_Proceed()
    {// 양쪽 진형이 행동이 완료된것을 확인하고 턴을 진행하는 메소드
        int Turn_count = 0;

        if(Start_Check_Turn(true))
        {
            Turn_count++;
        }
        if(Start_Check_Turn(false))
        {
            Turn_count++;
        }

        return Turn_count;

    }



    public List<Ch> Corutine_Proceed_Turn(Ch[] Player , Ch[] Enemy)
    {//턴 
        //Speed 순서대로 행동을 재분배 

        Ch Temp_Speed;
        List<Ch> Proceed_List = new();


        for (int i = 0; i < Enemy.Length; i++)
        {
            if (Enemy[i] != null)
            {
                Proceed_List.Add(Enemy[i]);
            }
        }
        for (int i = 0; i < Player.Length; i++)
        {
            if (Player[i] != null)
            {
                Proceed_List.Add(Player[i]);
            }
        }


        for (int i = Proceed_List.Count - 1; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                if (Proceed_List[j].Speed.Stat_out() > Proceed_List[j + 1].Speed.Stat_out())
                {
                    Temp_Speed = Proceed_List[j];
                    Proceed_List[j] = Proceed_List[j + 1];
                    Proceed_List[j + 1] = Temp_Speed;
                }

            }



        }








        //for(int i = 0; i < Enemy.Length; i++)
        //{
        //    if (Enemy[i] != null)
        //    {
        //        Proceed_List.Add(new (Enemy[i].Ch_Flag,Enemy[i].Speed.Stat_out()));
        //    }
        //}
        //for (int i = 0; i < Player.Length; i++)
        //{
        //    if (Player[i] != null)
        //    {
        //        Proceed_List.Add(new(Player[i].Ch_Flag, Player[i].Speed.Stat_out()));
        //    }
        //}


        //for(int i = Proceed_List.Count-1; i > 0; i--)
        //{
        //    for(int j = 0;j < i; j++)
        //    {
        //        if (Proceed_List[j].Speed > Proceed_List[j+1].Speed)
        //        {
        //            Temp_Speed = Proceed_List[j];
        //            Proceed_List[j] = Proceed_List[j + 1];
        //            Proceed_List[j + 1] = Temp_Speed;
        //        }

        //    }



        //}



        return Proceed_List;









    }


    public int[] Speed_List_CH_isDisplay(List<Ch> Proceed_List, field_ch[] Field_List)
    {
        int[] Ch_List = new int[3];
        int count = 0;
        
        for(int i = 0; i < Proceed_List.Count; i++)
        {

            if (Proceed_List[i] != null)
            {
                for (int k = 0; k < Field_List.Length; k++)
                {
                    if (Field_List[k] != null)
                    {
                        if (Proceed_List[i].Ch_names == Field_List[k].Ch_names)
                        {
                            Ch_List[count] = k;
                            count++;
                            break;

                        }


                    }
                }

            }

         
           


        }
       


        return Ch_List;
    }


    ~GameM_Turn()
    {
        Turn_set = null;


    }
}
