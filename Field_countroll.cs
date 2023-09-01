using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Field_countroll : MonoBehaviour
{
    GameM_chs game_m;


    // Start is called before the first frame update
    void Start()
    {
        game_m = GameM_chs.SetSpawn();
        File_Readerr.Back_List_Ch();
        Party_setting();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("SampleScene");
        }


    }




    public void Party_setting()//파티 세팅
    {
        if(game_m==null)
        {
            game_m = GameM_chs.SetSpawn();
        }
        game_m.Player[0] = PartySet("P");
        game_m.Enemy[0] = PartySet("E");
        //스킬 넣어주기
        game_m.Player[0].Skill_names = game_m.SKill_IN_CH(game_m.Player[0]);
        game_m.Enemy[0].Skill_names = game_m.SKill_IN_CH(game_m.Enemy[0]);
        Debug.Log("스킬 카운트" + game_m.Player[0].Skill_names[0].Skill_names);
        Debug.Log("스킬 카운트" + game_m.Enemy[0].Skill_names.Count);
    }


    public static Ch PartySet(string key)//임시
    {
        Ch a = null;
        if(key=="P")
        {
            a=  new Ch(1, File_Readerr.Ch_Lists[0]["Ch_List"].ToString(), int.Parse(File_Readerr.Ch_Lists[0]["HP"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[0]["POW"].ToString()), int.Parse(File_Readerr.Ch_Lists[0]["Speed"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[0]["Ultara_instinct"].ToString()), int.Parse(File_Readerr.Ch_Lists[0]["Melee"].ToString()),
                int.Parse(File_Readerr.Ch_Lists[0]["Ranger"].ToString()), int.Parse(File_Readerr.Ch_Lists[0]["Physics"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[0]["Magic"].ToString()), int.Parse((File_Readerr.Ch_Lists[0]["abnormal_status"].ToString())),
                 int.Parse((File_Readerr.Ch_Lists[0]["Defence"].ToString())), int.Parse((File_Readerr.Ch_Lists[0]["Mana_cost"].ToString()))
                 ,int.Parse(File_Readerr.Ch_Lists[0]["Mana_regeneration"].ToString()),
                 int.Parse(File_Readerr.Ch_Lists[0]["Mana_regeneration_P_Maxed"].ToString()), 
                 int.Parse(File_Readerr.Ch_Lists[0]["mana_Maxed"].ToString()),1,0,0);
                 
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
                 int.Parse(File_Readerr.Ch_Lists[1]["mana_Maxed"].ToString()),1,0,0);
        }
        return a;
    }



    


    


}
