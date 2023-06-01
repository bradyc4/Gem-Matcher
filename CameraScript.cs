using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Board board;
    public float cameraWidth;
    // camera dimensions are 9:16
    void Start()
    {
        Camera.main.orthographicSize = (float)board.height/2 * 1.2f;

        transform.position = new Vector3(    (float)board.width/2 - 0.5f,    Camera.main.orthographicSize - 1f,    transform.position.z    );
        //Vector3 pos = Camera.main.WorldToViewportPoint(new Vector3(board.width, board.height, board.transform.position.z));
        
        cameraWidth = Camera.main.aspect * Camera.main.orthographicSize * 2;
        //Debug.Log("camera orthographicSize = " + Camera.main.orthographicSize);
        if(board.width+1 >= cameraWidth){
            Camera.main.orthographicSize = (1/Camera.main.aspect) * (board.width+1) / 2;
        }
    }
}
