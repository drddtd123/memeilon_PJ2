using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class OverLay_UI : MonoBehaviour
{
    //턴 UI,HP , MP 등등 관리할거
    //게임 아이디어
    //필드 이동시 벽 러닝 (월 러닝), 로프 액션 가능하게[테라리아 N수정 로프 같이]

    public TextMeshProUGUI TurnText_isDisplay_Every;
    public TextMeshProUGUI HPText_isDisplay_Player;
    public TextMeshProUGUI MPText_isDisplay_Player;
    public Image TurnImage_isDisplay_Every;
    public Image HPImage_isDisplay_Player;
    public Image MPImage_isDisplay_Player;
    Camera_UI camera_UI;//카메라에 붙어 있는 UI [플레이어 매뉴 담당]

    //public Image[] Player_ 

    private void Awake()
    {
        camera_UI = FindObjectOfType<Camera_UI>();
    }
    public void DeafualtUIvaluesChange()
    {
        TurnText_isDisplay_Every.text = "1";
        HPText_isDisplay_Player.text = "2";
        MPText_isDisplay_Player.text = "3";
    }


    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TurnUI_TextChange(int Turn)
    {
        TurnText_isDisplay_Every.text = "턴:"+Turn.ToString();
    }



    public void OFF_UI()
    {
        TurnImage_isDisplay_Every.gameObject.SetActive(false);
        HPImage_isDisplay_Player.gameObject.SetActive(false);
        MPImage_isDisplay_Player.gameObject.SetActive(false);
    }



    public void Show_PlayerGamePlayProgress_UI(Ch ProgressCH)
    {
        HPText_isDisplay_Player.text = ProgressCH.HP.Stat_out().ToString()+'/'+ ProgressCH.HP.Stat_out_maxed().ToString();
        MPText_isDisplay_Player.text = ProgressCH.Mana_cost.Stat_out().ToString() + '/' + ProgressCH.Mana_cost.Stat_out_maxed().ToString();
        show_ImageUI();
    }

    public void show_ImageUI()
    {//애니메이션 연출로 바꿀시 해당 UI가 화면 바깥으로 나가는 연출로 변경
        TurnImage_isDisplay_Every.gameObject.SetActive(true);
        HPImage_isDisplay_Player.gameObject.SetActive(true);
        MPImage_isDisplay_Player.gameObject.SetActive(true);
    }


}
