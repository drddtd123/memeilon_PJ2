using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Camera_UI : MonoBehaviour
{
    //캐릭터 선택 매뉴 위주로 만들어갈거
    public enum FightManu { 
    NomalSkill
    ,SKill
    ,DeffenceMode
    ,Back
    }

    public enum SelectedActionManu
    {
        ATTack
       ,UseItem
       ,ChangeCh
       ,Escape
    }

    public enum Slide_Deffence
    {
        Deffence,//0
        not//1
        ,Dodge//2

    }



    private bool CantuseNomal_Attack = false;//기본 평타 금지
    private bool CantuseUltamate = false;//궁극기 사용불가
    private bool CantuseSkill = false;//스킬 사용불가
    private bool CantFight = false;//싸움불가
    private bool CantUseITEM = false;//아이템 사용불가
    private bool CantEscape = false;//탈출불가
    private bool CantChangeCH = false;//캐릭터 교체 불가
    private bool CantDeffence = false;//방어불가
    private bool CantDodge = false;//회피불가


    [Header("ThinkUIobj")]
    public GameObject ThinkUIobjs;
    [Header("FightUIobj")]
    public GameObject FightUIobjs;
    [Header("ThinkUIButtons")]
    public Button FightManuUI_BUTTON;
    public Button ItemManuUI_BUTTON;
    public Button chChangeManuUI_BUTTON;
    public Button RunManuUI_BUTTON;
    [Header("FightManuUIButtons")]
    public Button NomalAttack_BUTTON;
    public Button Skill_BUTTON;
    public Button DeffenceMode_BUTTON;
    public Button Return_thinkManuUI_Button;
    [Header("Color")]
    public Color HideColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public Color ShowColor = new Color(1f,1f, 1f, 1);

    private Ch SelectedCh_Now;//현재 선택된 캐릭터


    public void DeafualtUISET()
    {
        SelectedCh_Now = null;
        ThinkUIobjs.SetActive(false);
        FightUIobjs.SetActive(false);
    }

    public void Ch_ManuUIShowisDisplay(Ch SelectedCH)// 턴 시작 - > 캐릭터 자동 선택 -> UI 띄우기 여기로
    {
        SelectedCh_Now = SelectedCH;
        ThinkUIobjs.SetActive(true);
        //UI 애니메이션 효과가 있으면 발동
        if (CantFight != true)
        {
            FightManuUI_BUTTON.enabled = true;
            FightManuUI_BUTTON.image.color = ShowColor;
        } else if (CantFight == true)
        {
            FightManuUI_BUTTON.enabled = false;
            FightManuUI_BUTTON.image.color = HideColor;
        }

        if (CantUseITEM != true)
        {
            ItemManuUI_BUTTON.enabled = true;
            ItemManuUI_BUTTON.image.color = ShowColor;
        }
        else if (CantUseITEM == true)
        {
            ItemManuUI_BUTTON.enabled = false;
            ItemManuUI_BUTTON.image.color = HideColor;
        }


        if (CantEscape != true)
        {
            RunManuUI_BUTTON.enabled = true;
            RunManuUI_BUTTON.image.color = ShowColor;
        }
        else if (CantEscape == true)
        {
            RunManuUI_BUTTON.enabled = false;
            RunManuUI_BUTTON.image.color = HideColor;
        }

        if (CantChangeCH != true)
        {
            chChangeManuUI_BUTTON.enabled = true;
            chChangeManuUI_BUTTON.image.color = ShowColor;
        }
        else if (CantChangeCH == true)
        {
            chChangeManuUI_BUTTON.enabled = false;
            chChangeManuUI_BUTTON.image.color = HideColor;
        }

    }


    public void Manu_Selected(int slected_manu)
    {//매뉴 선택
        ThinkUIobjs.SetActive(false);

        switch (slected_manu)
        {
            case (int)SelectedActionManu.ATTack:
                FightManuSelected();

                break;
            case (int)SelectedActionManu.UseItem:

                break;
            case (int)SelectedActionManu.ChangeCh:

                break;
            case (int)SelectedActionManu.Escape:

                break;


            default:
                break;
                
        }


    }

    public void FightManu_Selected(int slected_manu)
    {//행동 매뉴 선택
        FightUIobjs.SetActive(false);

        switch (slected_manu)
        {
            case (int)FightManu.NomalSkill:

                break;
            case (int)FightManu.SKill:

                break;
            case (int)FightManu.DeffenceMode:

                break;
            case (int)FightManu.Back:

                break;


            default:
                
                break;

        }


    }


    public void FightManuSelected()
    {
        //공격 (행동 선택하기)  ->  행동 매뉴 보여주기
        FightUIobjs.SetActive(true);

        if (CantuseNomal_Attack == true)
        {
            NomalAttack_BUTTON.enabled = false;
            NomalAttack_BUTTON.image.color = HideColor;
        }
        else
        {
            NomalAttack_BUTTON.enabled = true;
            NomalAttack_BUTTON.image.color = ShowColor;
        }

        if (CantuseSkill == true)
        {
            Skill_BUTTON.enabled = false;
            Skill_BUTTON.image.color = HideColor;
        }
        else
        {
            Skill_BUTTON.enabled = true;
            Skill_BUTTON.image.color = ShowColor;
        }



    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Manu_ON()
    {


    }

}
