using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameM_chs
{//게임 매니저 [캐릭터 관리용]싱글톤

    // 게임 시작시 적 정보 와 아군 정보를 가져와서 배틀 씬의 캐릭터 배치를 담당
    private static GameM_chs GameM_ = null;
    // Start is called before the first frame update

    public Ch[] Player = new Ch[5];//아군 파티

    public Ch[] Enemy = new Ch[9];//적군 파티 [MAX 9]




    public void Set()
    {
        for(int i = 0; i < Player.Length; i++)
        {
            Player[i] = null;

        }

        for (int i = 0; i < Enemy.Length; i++)
        {
            Enemy[i] = null;

        }
        Player[0] = new Ch("네일즈", 150, 50, 100, 50, 2, 0, 5, 0, 0, 20, 2, 10);
        Enemy[0] = new Ch("마일즈", 150, 50, 100, 50, 2, 0, 5, 0, 0, 20, 2, 10);
    
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
        }


        return GameM_;
    }



    public static void Off_Game()//
    {
        GameM_ = null;//비워주기
        GC.Collect();//가비지 돌리기
        GC.WaitForPendingFinalizers();
        //씬 변경
    }

    
    


}
