using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

    public class Balle : MonoBehaviour
{
    
    //singleton to access ball
    public static Balle balleInstance { get; set; } = new Balle();
    
    private Vector3 _currentSpeed;
    
    public List<GameObject> totalCubeList;
    private List<GameObject> _totalCylinderList;
    
    private Text _countText;
    private int _score;

    private float _speedMax;
    private float _speedMin;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = new Vector3(0f,0f,-20f);
        transform.position += _currentSpeed * Time.deltaTime;

        GameObject[] cubeNotScore = GameObject.FindGameObjectsWithTag("CubeNotScore");
        GameObject[] cubeList = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] pinList = GameObject.FindGameObjectsWithTag("Pin");
        GameObject[] tremplinList = GameObject.FindGameObjectsWithTag("Tremplin");
        GameObject lanceur = GameObject.FindGameObjectWithTag("Lanceur");
        
        totalCubeList = new List<GameObject>();
        totalCubeList.AddRange(cubeNotScore);
        totalCubeList.AddRange(cubeList);
        totalCubeList.AddRange(pinList);
        totalCubeList.AddRange(tremplinList);
        totalCubeList.Add(lanceur);
        
        GameObject[] cylinderList = GameObject.FindGameObjectsWithTag("Cylinder");
        
        _totalCylinderList = new List<GameObject>();
        _totalCylinderList.AddRange(cylinderList);
        
        _score = 0;
        _countText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        PrintScore();

        _speedMax = 20;
        _speedMin = 7;
    }

    // Update is called once per frame
    void Update()
    {
        CalculGravityVector();

        foreach (var cube in totalCubeList)
        {
            int surfaceCollision = CubeDetectCollision(transform.position, cube);
            if (surfaceCollision != 0) // Collision ?
            {
                // Update score
                if (!cube.tag.Equals("CubeNotScore") && !cube.tag.Equals("Pin")) _score += 5;

                // Rebond
                if (surfaceCollision == 1) Rebond(Vector3.Normalize(cube.transform.right), cube);
                if (surfaceCollision == 2) Rebond(Vector3.Normalize(cube.transform.up), cube);
                if (surfaceCollision == 3) Rebond(Vector3.Normalize(cube.transform.forward), cube);
            }
        }

        foreach (var cylinder in _totalCylinderList)
        {
            Vector3 normal;
            bool surfaceCollision = CylinderDetectCollision(transform.position, cylinder, out normal);
            if (surfaceCollision) // Collision ?
            {
                _score += 20;
                Rebond(Vector3.Normalize(normal), cylinder);
            }
        }

        PrintScore();
    }

    public int CubeDetectCollision(Vector3 currentPos, GameObject cube)
    {
        // initialisation value
        float rayonBalle = transform.localScale.x / 2;
        Vector3 cubeLocalScale = cube.transform.localScale;
        float distanceBeforeTouchX = rayonBalle + cubeLocalScale.x / 2;
        float distanceBeforeTouchY = rayonBalle + cubeLocalScale.y / 2;
        float distanceBeforeTouchZ = rayonBalle + cubeLocalScale.z / 2;
        Vector3 cubePos = cube.transform.position;
        
        // verification collision
        Vector3 futurPos = currentPos;
        futurPos += _currentSpeed * Time.deltaTime;
        Vector3 diffPosFutur = new Vector3(currentPos.x - futurPos.x, currentPos.y - futurPos.y, currentPos.z - futurPos.z) / 200;
        for (int i = 0; i < 200; i++)
        {
            // simulate move
            futurPos += diffPosFutur;
            
            // calcul scalar product
            Vector3 diffPos = new Vector3(futurPos.x - cubePos.x, futurPos.y - cubePos.y, futurPos.z - cubePos.z);
            float scalarProductX = Vector3.Dot(diffPos, Vector3.Normalize(cube.transform.right));
            float scalarProductY = Vector3.Dot(diffPos, Vector3.Normalize(cube.transform.up));
            float scalarProductZ = Vector3.Dot(diffPos, Vector3.Normalize(cube.transform.forward));

            // collision ?
            if (scalarProductY < distanceBeforeTouchY && scalarProductX < distanceBeforeTouchX && scalarProductZ < distanceBeforeTouchZ &&
                scalarProductY > -distanceBeforeTouchY && scalarProductX > -distanceBeforeTouchX && scalarProductZ > -distanceBeforeTouchZ)
            {
                // on what surface ?
                if (scalarProductX < distanceBeforeTouchX && scalarProductX > distanceBeforeTouchX - 0.1f || scalarProductX > -distanceBeforeTouchX && scalarProductX < -distanceBeforeTouchX + 0.1f) return 1;
                if (scalarProductY < distanceBeforeTouchY && scalarProductY > distanceBeforeTouchY - 0.1f || scalarProductY > -distanceBeforeTouchY && scalarProductY < -distanceBeforeTouchY + 0.1f) return 2;
                if (scalarProductZ < distanceBeforeTouchZ && scalarProductZ > distanceBeforeTouchZ - 0.1f || scalarProductZ > -distanceBeforeTouchZ && scalarProductZ < -distanceBeforeTouchZ + 0.1f) return 3;
            }
        }
        return 0;
    }
    
    bool CylinderDetectCollision(Vector3 currentPos, GameObject cylinder, out Vector3 normal)
    {
        // initialisation value
        float distanceBeforeTouch = transform.localScale.x / 2 + cylinder.transform.localScale.x / 2;

        // verification collision
        Vector3 futurPos = currentPos;
        futurPos += _currentSpeed * Time.deltaTime;
        Vector3 diffPosFutur = new Vector3(currentPos.x - futurPos.x, currentPos.y - futurPos.y, currentPos.z - futurPos.z) / 200;
        for (int i = 0; i < 100; i++)
        {
            // simulate move
            futurPos += diffPosFutur;
            
            // calcul projection
            Vector3 diffPos = futurPos - cylinder.transform.position;
            Vector3 diffOnCylinder = Vector3.Project(diffPos, cylinder.transform.up);
            normal = diffOnCylinder - diffPos;

            // collision ?
            if (normal.magnitude < distanceBeforeTouch) return true;
        }
        normal = Vector3.zero;
        return false;
    }

    public void Rebond(Vector3 normal, GameObject obj)
    {
        if (obj.tag.Equals("Lanceur"))
        {
            _currentSpeed = new Vector3(0f,0f,-20f);
            transform.position += _currentSpeed * Time.deltaTime;
            _score += 495;
        }
        else
        {
            float addMovement = 0.95f;
            if (obj.tag.Equals("Tremplin")) addMovement = 1f;
            else if (obj.tag.Equals("Pin") || obj.tag.Equals("Cylinder")) addMovement = 1.2f;

            if (obj.tag.Equals("Pin"))
            {
                float _width = obj.transform.localScale.z / 2f;

                transform.position -= new Vector3(0, 0, _width);

            }
            Vector3 perpendicular = Vector3.Project(_currentSpeed, normal);
            Vector3 parallel = _currentSpeed - perpendicular;
        
            if (_currentSpeed.magnitude * addMovement < _speedMax)
            {
                _currentSpeed = parallel - addMovement * perpendicular;
            }
            else if (_currentSpeed.magnitude < _speedMin && obj.tag.Equals("Pin"))
            {
                _currentSpeed = parallel - 1.5f * perpendicular;
            }
            else
            {
                _currentSpeed = parallel - perpendicular;
            }
            transform.position += _currentSpeed * Time.deltaTime;
        }
    }
    
    void CalculGravityVector()
    {
        Vector3 gravityVector = new Vector3(0.0f,-9.81f,0.0f);
        _currentSpeed += gravityVector * Time.deltaTime;
        transform.position += _currentSpeed * Time.deltaTime;
    }
    
    void PrintScore()
    {
        _countText.text = "Score: " + _score.ToString();
    }
}