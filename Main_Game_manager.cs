using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_Game_manager : MonoBehaviour
{//전투시 돌아가는 스크립트
 //전투 필드 스크립트

    private BackGround_Manager backGround_Manager = null;//대미지 관련 계산 하는놈
    private GameM_chs Game_ch = null;//게임 내 캐릭터 데이터 [파티 , 적] < - 필드씬 고칠시 다시만져줘야함 0824
                                     //public Fight Fight_;//싸움 방식  <- 스테이지 별로 다른 방식의 전투가 이어져야한다면 Awake시 스테이지 읽고 맞는 FIGHT 를 연결해주게 구성
    public field_ch[] Player_F = new field_ch[3];//필드에 나와있어야하는 캐릭터들
    public field_ch[] Enemy_F = new field_ch[3];// "
    [SerializeField] private GameObject[] Field_Player_ch_Spawn_Point;//필드[전투]의 오브젝트 스폰 장소[좌쪽]
    [SerializeField] private GameObject[] Field_Enemy_ch_Spawn_Point;//필드[전투]의 오브젝트 스폰 장소 [우측]
    public TextMeshProUGUI Health_Text;//체력 [좌측 진형의 선택된 캐릭터의 체력]
    public TextMeshProUGUI Mana_Cost_Text;//마나 [좌측 진형의 선택된 캐릭터의 마나]
    private GameM_Turn Game_Turn;//게임턴
    [SerializeField] private Fight_Struct[] Game_Proceed_Chs = new Fight_Struct[10];
    [SerializeField] private int Flag_Count = 0;
    [SerializeField] private List<Ch> ch_Proceed_Game;//캐릭터들의 행동순서
                                                      //Game_Proceed_Chs <- 필드[아웃 필드] 에서 전투 필드 로 옮겨갈떄  바꿔줘야함 캐릭터를 수를 받아오고
                                                      //그 수에 따라서 행동을 정할수있는 애들이 정해져있으니 그렇게 한다
                                                      //
    private int[] ch_Proceed_Game_List_isDisplay = new int[3];
    public Cinematic_Camera cinematic_Cameras;//카메라
    private int Flag_int_count = 0;//몇번쨰 캐릭을 플레이중인지 확인하는 친구

    //황금 4시간
    bool A = false;
    public void Awake()//필드 씬 -> 게임 씬 
    {
        Party_setting();
        Flag_Count = 0;
        backGround_Manager = BackGround_Manager.Start_manager();
        backGround_Manager.List_value_Read();
        Game_ch = GameM_chs.SetSpawn();
        GameSet();
        Game_Turn.Awake_Turn();
    }


    public void GameSet()//짜진 파티를 토대로 캐릭터를 필드에 불러옴
    {
        Spawn_objs(Field_Player_ch_Spawn_Point, Game_ch.Player_Party, Player_F);//플레이어 캐릭터 생성
        Spawn_objs(Field_Enemy_ch_Spawn_Point, Game_ch.Enemy_Party, Enemy_F);//적 캐릭터 생성
        Game_Turn = new GameM_Turn();
        ch_Proceed_Game_List_isDisplay = new int[3];
    }


    public virtual void Spawn_objs(GameObject[] Spawn_Place, Ch[] CH_List, field_ch[] F_CH)//캐릭터 스폰  [전투시 처음만 사용] 
    {
        Flag_Count = 0;
        for (int i = 0; i < Spawn_Place.Length; i++)
        {
            if (CH_List[i] != null)
            {
                F_CH[i] = new field_ch(Instantiate(Prefab_Manager(CH_List[i].Ch_names), Spawn_Place[i].transform.position, Quaternion.identity, Spawn_Place[i].transform), CH_List[i].Ch_names);
                F_CH[i].CH_obj.name = CH_List[i].Ch_names;
                CH_List[i].Ch_Flag = Flag_Count;
                Flag_Count++;//캐릭터 구분용 데이터
            }
            else
            {
                F_CH[i] = null;
            }
        }


    }




    public GameObject Prefab_Manager(string Ch_name)//캐릭터 매니저 -> 만약에 캐릭터 이미지가 없으면 더미를 소환
    {
        GameObject Prefab = null;

        Prefab = Resources.Load<GameObject>("Ch/" + Ch_name);
        if (Prefab == null)
        {
            Debug.Log("비어있음");
            Prefab = Resources.Load<GameObject>("Dummy/CH_Dummy");
        }

        return Prefab;
    }







    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && A == false)
        {
          
            Game_Setting_Next_Turn();

        }

    }

    public void Party_setting()//파티 세팅
    {
        if (Game_ch == null)
        {
            Game_ch = GameM_chs.SetSpawn();
        }
        Game_ch.Player_Party[0] = PartySet("P");
        Game_ch.Enemy_Party[0] = PartySet("E");
        //스킬 넣어주기
        Game_ch.Player_Party[0].Skill_names = Game_ch.SKill_IN_CH(Game_ch.Player_Party[0]);
        Game_ch.Enemy_Party[0].Skill_names = Game_ch.SKill_IN_CH(Game_ch.Enemy_Party[0]);
        Debug.Log("스킬 카운트" + Game_ch.Player_Party[0].Skill_names[0].Skill_names);
        Debug.Log("스킬 카운트" + Game_ch.Enemy_Party[0].Skill_names.Count);
    }


    public static Ch PartySet(string key)//임시
    {
        File_Readerr.Back_List_Ch();
        Ch a = null;
        if (key == "P")
        {
            a = new Ch(1, File_Readerr.Ch_Lists[0]["Ch_List"].ToString(), int.Parse(File_Readerr.Ch_Lists[0]["HP"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[0]["POW"].ToString()), int.Parse(File_Readerr.Ch_Lists[0]["Speed"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[0]["Ultara_instinct"].ToString()), int.Parse(File_Readerr.Ch_Lists[0]["Melee"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[0]["Ranger"].ToString()), int.Parse(File_Readerr.Ch_Lists[0]["Physics"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[0]["Magic"].ToString()), int.Parse((File_Readerr.Ch_Lists[0]["abnormal_status"].ToString())),
                 int.Parse((File_Readerr.Ch_Lists[0]["Defence"].ToString())), int.Parse((File_Readerr.Ch_Lists[0]["Mana_cost"].ToString()))
                 , int.Parse(File_Readerr.Ch_Lists[0]["Mana_regeneration"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[0]["Mana_regeneration_P_Maxed"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[0]["mana_Maxed"].ToString()), 1, 0, 0);

        }
        else// E
        {
            a = new Ch(1, File_Readerr.Ch_Lists[1]["Ch_List"].ToString(), int.Parse(File_Readerr.Ch_Lists[1]["HP"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[1]["POW"].ToString()), int.Parse(File_Readerr.Ch_Lists[1]["Speed"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[1]["Ultara_instinct"].ToString()), int.Parse(File_Readerr.Ch_Lists[1]["Melee"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[1]["Ranger"].ToString()), int.Parse(File_Readerr.Ch_Lists[1]["Physics"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[1]["Magic"].ToString()), int.Parse((File_Readerr.Ch_Lists[1]["abnormal_status"].ToString())),
                 int.Parse((File_Readerr.Ch_Lists[1]["Defence"].ToString())), int.Parse((File_Readerr.Ch_Lists[1]["Mana_cost"].ToString()))
                 , int.Parse(File_Readerr.Ch_Lists[1]["Mana_regeneration"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[1]["Mana_regeneration_P_Maxed"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[1]["mana_Maxed"].ToString()), 1, 0, 0);
        }


        return a;
    }




    private void Game_proceed_Set()//행동 순서 가져오는 메소드
    {
        if(ch_Proceed_Game != null)
        {
            ch_Proceed_Game.Clear();
        }
        ch_Proceed_Game = Game_Turn.Corutine_Proceed_Turn(Game_ch.Player_Party, Game_ch.Enemy_Party);
        if(ch_Proceed_Game != null)
        {
            Debug.Log("데이터[캐릭터 우선도]가 삽입 되었습니다.");
            for (int i = 0; i < ch_Proceed_Game.Count; i++)
            {
              Debug.Log("캐릭터 이름:"+ch_Proceed_Game[i].Ch_names +",캐릭터 스피드:"+ ch_Proceed_Game[i].Speed.Stat_out());
            }
        }
        else
        {
            Debug.Log("데이터[캐릭터 우선도]가 존재하지않습니다.");
        }
      
    }

    private void Game_Setting_Next_Turn()
    {//턴 셋팅
        //턴 종료시 보너스 있는 애들 것들
        Game_Turn.Turn_UP(1);
        Debug.Log("현재 턴수 : "+Game_Turn.Check_Turn());//UI 랑 연결 해주면 됨 
        Game_Turn.Game_Set();//양쪽 진영 Bool 초기화
        //턴 시작시 보너스 있는 애들 것들
        Game_proceed_Set();//캐릭터 순서 재분배 
                           //AI 패턴 돌리는거 후
        Flag_int_count = 0;//선택한 캐릭터 카운트 초기화
        ch_Proceed_Game_List_isDisplay = Game_Turn.Speed_List_CH_isDisplay(ch_Proceed_Game, Player_F); //플레이어측 캐릭터중 제일 빠르게 행동하는 얘한테 UI 이동 및 발동
        Game_Camera_Set_UI_isDisplay_Player();//UI 및 캐릭터 행동 정하기
    }
    private void Game_Setting_Next_Turn(int Turn)
    {//턴 셋팅
        Game_Turn.Turn_UP(Turn);
        Debug.Log("현재 턴수 : " + Game_Turn.Check_Turn());//UI 랑 연결 해주면 됨 
        Game_Turn.Game_Set();//양쪽 진영 Bool 초기화
        Flag_int_count = 0;//선택한 캐릭터 카운트 초기화
        //턴 시작시 보너스 있는 애들 것들
        Game_proceed_Set();//캐릭터 순서 재분배 
        //AI 패턴 돌리는거 후
        //플레이어측 캐릭터중 제일 빠르게 행동하는 얘한테 UI 이동 및 발동
    }




    private void Game_Camera_Set_UI_isDisplay_Player()//카메라 맞추고 시스템 UI 까지 띄우기
    {
    

        for(int i=0;i< ch_Proceed_Game_List_isDisplay.Length; i++)
        {
            if(Player_F[ch_Proceed_Game_List_isDisplay[i]] != null)
            {
                bool Ch_dead = Game_ch.Check_CH_is_dead(Player_F[ch_Proceed_Game_List_isDisplay[i]].Ch_names, Game_ch.Player_Party);
                if (Ch_dead)//캐릭터가 죽은 상태면
                {
                 
                }else if (Ch_dead == false)
                {
                    bool Can_Play = Game_ch.Check_CH_iscan_Proceed_Game(Player_F[ch_Proceed_Game_List_isDisplay[i]].Ch_names, Game_ch.Player_Party);
                    if (Can_Play)
                    {
                        Flag_int_count = i;
                        //해당 캐릭터가 행동할수있으면
                        break;
                    }

                }


            }
        }
        cinematic_Cameras.Camera_set_GameOBJ(Player_F[ch_Proceed_Game_List_isDisplay[Flag_int_count]].CH_obj);
        //UI ON
        


    }









    private void Turn_Start_Proceed()//
        // 양쪽 진형이 서로 행동 종료가 됐는지 확인후 코루틴으로 행동 진행 메소드
    {
        int Check_count = Game_Turn.Start_Corutine_Turn_Proceed();
        //bool Game_END_Flag = false;// false 면 게임이 안 끝난 상태 TRUE면 끝난 상태


        if(Check_count >= 2)// 양쪽 진형다 행동을 종료한  상태면
        {
           StartCoroutine(GameStart());// 게임 시작! 불형 리턴 Game_END_Flag
        }
        {
            //if(Game_END_Flag == false)
            //{
            //    Game_proceed_Set();
            //    //
            //}
            //if(Game_END_Flag)//게임이 끝났으면..
            //{
            //    //전투가 끝났으니 보상 창 + 필드씬으로 보내고 현재 전투에서 일어난 스크립트 소멸자 발동시켜줄수있게 연계
            //}
        }

    }




    public void Game_End_Flag_Check()//게임이 끝났는지 안끝났는지 확인
    {// UI쪽으로 시스템 만들어줘야함.
        bool IsPlayer_End  = Check_to_CHList_Field_ALL_Dead(Player_F, Game_ch.Player_Party); //어느 한쪽 진영[필드에 존재하는 캐릭 전부 다운시] 한쪽 진영이 패배하게 만드는 시스템
        bool IsEnemy_End = Check_to_CHList_Field_ALL_Dead(Enemy_F, Game_ch.Enemy_Party);

        bool Draw = IsPlayer_End == false && IsEnemy_End == false;

        if (Draw)//둘다 다운일떄
        {
            //멀티시 비겼다 나오게 
            //모험시 패배

        }else // 비긴 상태가 아님 
        {
            if(IsPlayer_End == false)//플레이어의 캐릭터가 전부 누운 상태(필드)
            {
                //게임 패배 - > 현재 위치에서 제일 가까운 세이브포인트(마을)로 이동
                Debug.Log("GameLose");
            }
            else if(IsEnemy_End == false)
            {
                Debug.Log("GameWin"); //게임 승리 - > 게임 결과 창으로 -> 필드
            }
            else //두 진영 다 살아 있는 상태
            {
                //전투 속행
                Game_Setting_Next_Turn();
            }

        }

    }

    public bool Check_to_CHList_Field_ALL_Dead(field_ch[] field_Ches , Ch[] CH_List )//필드에 나와있는 캐릭들이 전부 쓰러졌는가
    {
        bool Check_ALive = true; // 살아있나 true 죽어있나 false
        int Dead_Count = 0;
        for(int i =0;i< field_Ches.Length; i++)
        {
            for(int j=0; j < CH_List.Length; j++)
            {
                if (field_Ches[i].Ch_names.Equals(CH_List[j].Ch_names))
                {
                    bool isDead = CH_List[j].State_Flag.Get_Stat_names().Equals(5);
                    if (isDead)//캐릭터 사망 상태 체크
                    {
                        Dead_Count++;
                        break;
                    }
                }
            }
            

        }
        bool Field_ch_ALL_Dead = Dead_Count>=3;
        if (Field_ch_ALL_Dead)//필드에 있는 애들이 전부 다운이면
        {
            Check_ALive = false;
        }


        return Check_ALive;
    }








    private IEnumerator GameStart()// 좀 더 생각 필요 09 05 2023
    {
        //카메라 리셋 (필드 전체 볼수있게)

        //이곳을 코루틴으로 마개조 해야함 
        //FoR문 {}
        //누군가의 행동
        //딜레이 타임 <업데이트 제어권 넘김> - > 대미지 END가 뜨면  대미지 받는 대상에게 대미지 폰트 띄움 [다른곳에서 업데이트 돌아감]
        //해당 캐릭터 스크립트에게 실제로 대미지 들어감 
        //애니메이션 딜레이 타임

        //완전히 다시 생각 베이스가 잘못됨
        for (int i = 0; i < ch_Proceed_Game.Count; i++)
        {
            if (ch_Proceed_Game != null)
            {
                if (ch_Proceed_Game[i].State_Flag.Get_Stat_names() >= 0 && ch_Proceed_Game[i].State_Flag.Get_Stat_names() < 4)//행동이 가능한가?
                {//해당 캐릭이 행동 가능 상태의 범위 안에 있으면
                    int a = Check_Proceed_CH(ch_Proceed_Game[i]);
                    if (a != -1)
                    {
                        float Time =  Game_Proceed_Chs[a].Fight_back().delay;

                        StartCoroutine(Game_Proceed_Chs[a].Fight_back().Coroutine_Fight_Tamplate(Game_Proceed_Chs[a].Check_Excute_CH()
                            , Game_Proceed_Chs[a].Check_Skill_CH(),
                            Game_Proceed_Chs[a].Check_recive_CH()));
                        yield return new WaitForSeconds(Time+2f);//공격 이펙트 시간 + 애니메이션 시간 으로 설정
                        //전투가 끝난 상태
                      


                    }
                }


            }

        }
        Game_End_Flag_Check();//게임이 끝났는지 확인


    }

    public int Check_Proceed_CH(Ch CH_names)
    {
        int Check_count = -1;
        for(int i =0; i < Game_Proceed_Chs.Length; i++)
        {
            if (Game_Proceed_Chs[i].Check_Excute_CH().Ch_Flag.Equals(CH_names.Ch_Flag))
            {
                if(Game_Proceed_Chs[i].Check_Skill_CH() != null)//발동 되는 스킬이 있다면
                {
                    return i;// 발동 가능
                }
            }

        }
        return Check_count;//발동 불가

    }













}




