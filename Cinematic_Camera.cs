using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic_Camera : MonoBehaviour
{
    public Camera cam;
    public float CamSize_Y = 1080;
    public float CamSize_X = 1920;
    public float size = 5;
    private Vector3 Cam_Vector = new Vector3(0, 0, -16);
    bool CamMoveing_obj = false;
    float CamMoveing_Speed = 0.1f;
    float CamReaching_values = 0;
    bool Down = false;
    bool Up = false;
    bool CamTranslate_ison = false;
    Vector3 Traslate_Place = Vector3.zero;
    private OverLay_UI overLay_UI;
    // 0  0 -16
    // Start is called before the first frame update

    private void Awake()
    {
        overLay_UI = FindObjectOfType<OverLay_UI>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (CamMoveing_obj)
        {
            if(Cam_MovePlace_isDown()&& Down)
            {
                cam.orthographicSize -= CamMoveing_Speed;
               

            }

            if(Cam_MovePlace_isUp()&&Up)
            {
                cam.orthographicSize += CamMoveing_Speed;
            }
            Cam_TranslatePosition();
            Cam_OrthographicSize_isOver();

        }

        
    }

    private bool Cam_MovePlace_isDown()
    {
        return CamReaching_values <= cam.orthographicSize;
    }
    private bool Cam_MovePlace_isUp()
    {
        return CamReaching_values >= cam.orthographicSize;
    }



    private void Cam_OrthographicSize_isOver()
    {
        if(cam.orthographicSize <= CamReaching_values + 0.5 && cam.orthographicSize >= CamReaching_values - 0.5)
        {
            CamMoveing_obj = false;
            Down = false;
            Up = false;
            cam.orthographicSize = CamReaching_values;
        }

    }


    public void CamReaching_values_SET(float Value)
    {
        CamReaching_values = Value;
    }



    public void CameraPositionReset()
    {//카메라 위치 및 시점 리셋
       

    }


    private void Cam_TranslatePosition()
    {
        if (CamTranslate_ison)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, Traslate_Place+ Cam_Vector , 1);
            if (cam.transform.position == Traslate_Place + Cam_Vector)
            {
                CamTranslate_ison = false;
            }
        }

    }


    public void Camera_set_GameOBJ(GameObject Game_obj)
    {
        CamMoveing_obj = true;
        //cam.transform.position = Game_obj.transform.position+ Cam_Vector;
        Traslate_Place = Game_obj.transform.position;
         Down = true;
        CamReaching_values = 2.5f;
        CamTranslate_ison = true;
       
        //게임 오브젝트 받은거 기준으로 카메라 워킹
    }

}
