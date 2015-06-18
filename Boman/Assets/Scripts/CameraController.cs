using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject follow;
    public float _mapWidth;
    public float _mapHeight;
    public float _cameraWidth;
    public float _cameraHeight;
    public float _panelWidth;

    public float _startX;
    public float _startY;

    // Use this for initialization
    void Start()
    {
        follow = GameObject.FindGameObjectWithTag("Player");
        MapGenerator mg = Camera.main.GetComponent<MapGenerator>();
        _mapWidth = mg._width;
        _mapHeight = mg._height;

        _cameraHeight = Camera.main.orthographicSize;
        _cameraWidth = _cameraHeight * Camera.main.aspect;
        _cameraHeight -= _panelWidth;
    }



    // Update is called once per frame
    void Update()
    {

        //	transform.position = Vector3.MoveTowards (new Vector3(transform.position.x, transform.position.y, zPoint), new Vector3(follow.transform.position.x, follow.transform.position.y, zPoint), Time.deltaTime);	

        //camera flickering (partial) fix @ http://forum.unity3d.com/threads/solved-2d-sprites-flicker-shake-on-camera-movement.270741/
        if (follow)
        {
            float camera_x = follow.transform.position.x;
            float camera_y = - follow.transform.position.y;

            if (camera_x - _startX - _cameraWidth < 0)
            {
                camera_x = _cameraWidth + _startX;
            }
            else if (camera_x - _startX + _cameraWidth - _mapWidth >0)
            {
                camera_x = _mapWidth - _cameraWidth + _startX;
            }


            if (camera_y - _startY - _cameraHeight < 0)
            {
                camera_y = _cameraHeight + _startY;
            }
            else if (camera_y - _startY + _cameraHeight - _mapHeight > 0)
            {
                camera_y = _mapHeight - _cameraHeight +_startY;
            }
            

            //float rounded_x = RoundToNearestPixel(player_x);
            //float rounded_y = RoundToNearestPixel(player_y);

            Vector3 new_pos = new Vector3(camera_x, -camera_y, -10f);
            transform.position = new_pos;
        }
    }

    public float pixelToUnits = 256f;

    public float RoundToNearestPixel(float unityUnits)
    {
        //float valueInPixels = unityUnits * pixelToUnits;
        //valueInPixels = Mathf.Round(valueInPixels);
        //float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        //return roundedUnityUnits;

        return unityUnits;
    }
}
