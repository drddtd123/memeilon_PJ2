using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BackGround_move : MonoBehaviour
{   
    public  Renderer r;
    public float move_speed = 0f;
    Material M;
    public Vector2 move_position = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(r!= null)//렌더가 비어 있지않으면
        {
            move_position.x += Time.deltaTime* move_speed;
            r.material.mainTextureOffset = move_position;
        }


    }
}
