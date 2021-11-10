using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Balle : MonoBehaviour
{
    private Vector3 currentSpeed;
    
    private List<GameObject> totalCubeList;
    private List<GameObject> totalCylinderList;
    
    //DELEGATE TESTING
    public delegate void OnPinCollisionDelegate();
    public static event OnPinCollisionDelegate onPinCollisionDelegate;

    public void OnPinCollision()
    {
        onPinCollisionDelegate();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = new Vector3(0f,0f,-20f);
        transform.position += currentSpeed * Time.deltaTime;

        GameObject[] cubeList = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] pinList = GameObject.FindGameObjectsWithTag("Pin");
        
        totalCubeList = new List<GameObject>();
        totalCubeList.AddRange(cubeList);
        totalCubeList.AddRange(pinList);
        
        GameObject[] cylinderList = GameObject.FindGameObjectsWithTag("Cylinder");
        
        totalCylinderList = new List<GameObject>();
        totalCylinderList.AddRange(cylinderList);
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
                if (cube.tag.Equals("Pin"))
                {
                    OnPinCollision();
                }
                if (surfaceCollision == 1) Rebond(Vector3.Normalize(cube.transform.right));
                if (surfaceCollision == 2) Rebond(Vector3.Normalize(cube.transform.up));
                if (surfaceCollision == 3) Rebond(Vector3.Normalize(cube.transform.forward));
            }
        }
        
        foreach (var cylinder in totalCylinderList)
        {
            Vector3 normal;
            bool surfaceCollision = CylinderDetectCollision(transform.position, cylinder, out normal);
            if (surfaceCollision) // Collision ?
            {
                Rebond(Vector3.Normalize(normal));
            }
        }

        PrintScore();
    }
    
    int CubeDetectCollision(Vector3 currentPos, GameObject cube)
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
        futurPos += currentSpeed * Time.deltaTime;
        Vector3 diffPosFutur = new Vector3(currentPos.x - futurPos.x, currentPos.y - futurPos.y, currentPos.z - futurPos.z) / 100;
        for (int i = 0; i < 100; i++)
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
        futurPos += currentSpeed * Time.deltaTime;
        Vector3 diffPosFutur = new Vector3(currentPos.x - futurPos.x, currentPos.y - futurPos.y, currentPos.z - futurPos.z) / 100;
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

    void Rebond(Vector3 normal)
    {
        Vector3 perpendicular = Vector3.Project(currentSpeed, normal);
        currentSpeed -= 2 * perpendicular;
        transform.position += currentSpeed * Time.deltaTime;
    }
    
    void CalculGravityVector()
    {
        Vector3 gravityVector = new Vector3(0.0f,-9.81f,0.0f);
        currentSpeed += gravityVector * Time.deltaTime;
        transform.position += currentSpeed * Time.deltaTime;
    }
    
    void PrintScore()
    {
        
    }
}
