using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Pin : MonoBehaviour
{
    //used to differentiate right and left pin
    [SerializeField] public bool isRight;

    private float _rotaSpeed;

    private Vector3 _angleCurr;

    private float _targetAnglePos, _targetAngleNeg;

    private float _restAnglePos, _restAngleNeg;

    public GameObject balleInstance;

    // Start is called before the first frame update
    void Start()
    {
        _rotaSpeed = 15f;
        _angleCurr = new Vector3(0, 0, 0);

        _targetAngleNeg = -25;
        _targetAnglePos = 25;
        _restAngleNeg = -30;
        _restAnglePos = 30;

        balleInstance = GameObject.FindGameObjectWithTag("Balle");
    }

    //bool BallDetectCollision(Vector3 currentPos, GameObject balle)
    //{
    //    // initialisation value
    //    float distanceBeforeTouchX = 4f;
    //        //GetComponentInChildren<Transform>().localScale.x / 2 + balle.transform.localScale.x / 2;
    //    float distanceBeforeTouchZ = 3.5f;
    //        //GetComponentInChildren<Transform>().localScale.z / 2 + balle.transform.localScale.z / 2;
//
    //        // verification collision
    //    Vector3 pos = transform.position;
//
    //    // calcul projection
    //    float diffPosX = pos.x - balle.transform.position.x;
    //    float diffPosZ = pos.z - balle.transform.position.z;
    //    
    //    //
    //    //Vector3 diffOnBall = Vector3.Project(diffPos, balle.transform.up);
    //    //normal = diffOnBall - diffPos;
//
    //    // collision ?
    //    //if (normal.magnitude < distanceBeforeTouch) return true;
    //    //normal = Vector3.zero;
    //    if (diffPosX <= distanceBeforeTouchX && diffPosZ <= distanceBeforeTouchZ)
    //    {
    //        canCollide = false;
    //        return true;
    //    }
//
    //    return false;
    //}

    // Update is called once per frame
    void Update()
    {
        if (!isRight)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                _angleCurr = new Vector3(
                    Mathf.LerpAngle(_angleCurr.x, _angleCurr.x, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.y, _targetAngleNeg, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.z, _angleCurr.z, _rotaSpeed * Time.deltaTime));
                GetComponent<RectTransform>().eulerAngles = _angleCurr;
            }
            else
            {
                _angleCurr = new Vector3(
                    Mathf.LerpAngle(_angleCurr.x, 0, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.y, _restAnglePos, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.z, 0, _rotaSpeed * Time.deltaTime));
                GetComponent<RectTransform>().eulerAngles = _angleCurr;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                _angleCurr = new Vector3(
                    Mathf.LerpAngle(_angleCurr.x, _angleCurr.x, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.y, _targetAnglePos, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.z, _angleCurr.z, _rotaSpeed * Time.deltaTime));
                GetComponent<RectTransform>().eulerAngles = _angleCurr;
            }
            else
            {
                _angleCurr = new Vector3(
                    Mathf.LerpAngle(_angleCurr.x, 0, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.y, _restAngleNeg, _rotaSpeed * Time.deltaTime),
                    Mathf.LerpAngle(_angleCurr.z, 0, _rotaSpeed * Time.deltaTime));
                GetComponent<RectTransform>().eulerAngles = _angleCurr;
            }
        }

        //dev only : reset game
        if (Input.GetKey(KeyCode.M))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}