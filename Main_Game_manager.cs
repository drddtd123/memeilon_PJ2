using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_Game_manager : MonoBehaviour
{//전투시 돌아가는 스크립트


   private BackGround_Manager backGround_Manager =null;//대미지 관련 계산 하는놈
   private GameM_chs Game_ch= null;//게임 내 캐릭터 데이터 [파티 , 적] < - 필드씬 고칠시 다시만져줘야함 0824
   public Fight Fight_;//싸움 방식  <- 스테이지 별로 다른 방식의 전투가 이어져야한다면 Awake시 스테이지 읽고 맞는 FIGHT 를 연결해주게 구성
   public field_ch[] Player_F = new field_ch[3];//필드에 나와있어야하는 캐릭터들
   public field_ch[] Enemy_F = new field_ch[3];// "
   [SerializeField] private GameObject[] Field_Player_ch_Spawn_Point;
   [SerializeField] private GameObject[] Field_Enemy_ch_Spawn_Point;
    public TextMeshProUGUI Health_Text;//체력
    public TextMeshProUGUI Mana_Cost_Text;//마나


    //황금 4시간
    bool A = false;
    public void Awake()//필드 씬 -> 게임 씬 
    {
        backGround_Manager = BackGround_Manager.Start_manager();
        backGround_Manager.List_value_Read();
        Game_ch = GameM_chs.SetSpawn();
        GameSet();
        
    }


    public void GameSet()//짜진 파티를 토대로 캐릭터를 필드에 불러옴
    {
        Spawn_objs(Field_Player_ch_Spawn_Point, Game_ch.Player, Player_F);//플레이어 캐릭터 생성
        Spawn_objs(Field_Enemy_ch_Spawn_Point, Game_ch.Enemy, Enemy_F);//적 캐릭터 생성
    }


    public virtual void Spawn_objs(GameObject[] Spawn_Place, Ch[] CH_List, field_ch[] F_CH)//캐릭터 스폰  [전투시 처음만 사용] 
    {
        for(int i = 0; i < Spawn_Place.Length; i++)
        {
            if (CH_List[i]!= null)
            {
                F_CH[i] = new field_ch(Instantiate(Prefab_Manager(CH_List[i].Ch_names), Spawn_Place[i].transform.position, Quaternion.identity, Spawn_Place[i].transform), CH_List[i].Ch_names);
                F_CH[i].CH_obj.name = CH_List[i].Ch_names;
            }
            else
            {
                F_CH[i] = null;
            }
        }


    }




    public GameObject Prefab_Manager(string Ch_name)//캐릭터 매니저 -> 만약에 캐릭터 이미지가 없으면 더미를 소환
    {
        GameObject Prefab= null;

        Prefab = Resources.Load<GameObject>("Ch/"+ Ch_name);
        if(Prefab == null)
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
        if(Input.GetMouseButtonDown(0)&&A == false)
        {
            A = true;
            Debug.Log(Game_ch.Player[0].HP.Stat_out());
            Debug.Log(Game_ch.Enemy[0].HP.Stat_out());
           // StartCoroutine(Fight_.Fight_Tamplate(Game_ch.Player[0], new mana_Gain(100, "Melee", "Physics",1), Game_ch.Enemy[0]));
            Debug.Log("아군"+Game_ch.Player[0].HP.Stat_out());
            Debug.Log("적"+Game_ch.Enemy[0].HP.Stat_out());
        }





    }




}


public class field_ch// 필드 캐릭터용 구조체
{

    public GameObject CH_obj;
    public string Ch_names;


    public field_ch(GameObject cH_obj, string ch_names)
    {
        CH_obj = cH_obj;
        Ch_names = ch_names;
    }
}