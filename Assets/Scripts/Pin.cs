using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    //used to spot differentiate right and left pin
    [SerializeField]
    public bool isRight;

    private float _rotaSpeed;

    private Vector3 _angleCurr;
    
    private float _targetAnglePos, _targetAngleNeg;

    private float _restAnglePos, _restAngleNeg;

    // Start is called before the first frame update
    void Start()
    {
        _rotaSpeed = 10f;
        _angleCurr = new Vector3(0, 0, 0);

        _targetAngleNeg = -25;
        _targetAnglePos = 25;
        _restAngleNeg = -30;
        _restAnglePos = 30;
    }

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
        } else
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
    }
}
