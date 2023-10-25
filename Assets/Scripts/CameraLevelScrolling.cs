using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CameraMoveType
{
    scroll,
    playerY,
    player
}

public class CameraLevelScrolling : MonoBehaviour
{

    //The speed that the camera moves across the screen
    public float cam_speed;
    //The boolean that says whether or not the camera should move
    public static CameraMoveType cam_move_type = CameraMoveType.playerY;

    public GameObject player;

    public float cam_smoothness;


    void Update()
    {
        if (cam_move_type == CameraMoveType.scroll)
        {
            gameObject.transform.Translate(Vector3.right * cam_speed * Time.deltaTime);
        }
        else if(cam_move_type == CameraMoveType.playerY)
        {
            Vector3 cam_pos = new Vector3(gameObject.transform.position.x +  cam_speed * Time.deltaTime,
                        Mathf.Lerp(gameObject.transform.position.y, player.transform.position.y, cam_smoothness * Time.deltaTime));
            //Vector3 cam_pos = player.transform.position;
            cam_pos.z = -10;
            gameObject.transform.position = cam_pos;
        }
        else if(cam_move_type == CameraMoveType.player)
        {
            Vector3 cam_pos = (Vector2.Lerp(gameObject.transform.position, player.transform.position, cam_smoothness*Time.deltaTime));
            //Vector3 cam_pos = player.transform.position;
            cam_pos.z = -10;
            gameObject.transform.position = cam_pos;
        }
    }
    
}

