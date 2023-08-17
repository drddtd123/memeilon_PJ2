using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Game_manager : MonoBehaviour
{//게임 시작시 발동 되어야하는 매니저


   private BackGround_Manager backGround_Manager =null;
   private GameM_chs Game_ch= null;
   public Fight Fight_;
   public field_ch[] Player_F = new field_ch[3];
   public field_ch[] Enemy_F = new field_ch[3];
   public GameObject enemy;
   public GameObject player;
    bool A = false;
    public void Awake()
    {
        backGround_Manager = BackGround_Manager.Start_manager();
        backGround_Manager.List_value_Read();
        Game_ch = GameM_chs.SetSpawn();
        Game_ch.Set();
        Player_F[0] = new(player, Game_ch.Player[0].Ch_names);
        Enemy_F[0] = new(enemy, Game_ch.Enemy[0].Ch_names);
        Debug.Log(Player_F[0].Ch_names);
        Debug.Log(Enemy_F[0].Ch_names);
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
            StartCoroutine(Fight_.Fight_Tamplate(Game_ch.Player[0], new mana_Gain(100, "Melee", "Physics",1), Game_ch.Enemy[0]));
            Debug.Log(Game_ch.Player[0].HP.Stat_out());
            Debug.Log(Game_ch.Enemy[0].HP.Stat_out());
        }

    }




}


public class field_ch
{

    public  GameObject CH_obj;
    public string Ch_names;

    public field_ch(GameObject cH_obj, string ch_names)
    {
        CH_obj = cH_obj;
        Ch_names = ch_names;
    }
}