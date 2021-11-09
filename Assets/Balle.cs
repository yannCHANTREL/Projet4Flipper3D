using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Balle : MonoBehaviour
{
    private Vector3 currentSpeed;
    private GameObject[] cubeList;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = Vector3.zero;
        cubeList = GameObject.FindGameObjectsWithTag("Cube");
    }

    // Update is called once per frame
    void Update()
    {
        CalculGravityVector();
        foreach (var cube in cubeList)
        {
            int surfaceCollision = DetectCollision(transform.position, cube);
            if (surfaceCollision != 0) // Collision ?
            {
                Debug.Log("surfaceCollision: " + surfaceCollision);
                if (surfaceCollision == 1) Rebond(Vector3.Normalize(cube.transform.right));
                if (surfaceCollision == 2) Rebond(Vector3.Normalize(cube.transform.up));
                if (surfaceCollision == 3) Rebond(Vector3.Normalize(cube.transform.forward));
            }
        }
    }
    
    int DetectCollision(Vector3 currentPos, GameObject cube)
    {
        // calcul scalar product
        Vector3 cubePos = cube.transform.position;
        Vector3 diffPos = new Vector3(currentPos.x - cubePos.x, currentPos.y - cubePos.y, currentPos.z - cubePos.z);
        float scalarProductX = Vector3.Dot(diffPos, Vector3.Normalize(cube.transform.right));
        float scalarProductY = Vector3.Dot(diffPos, Vector3.Normalize(cube.transform.up));
        float scalarProductZ = Vector3.Dot(diffPos, Vector3.Normalize(cube.transform.forward));
        
        // verification collision
        float rayonBalle = transform.localScale.x / 2;
        Vector3 cubeLocalScale = cube.transform.localScale;
        float distanceBeforeTouchX = rayonBalle + cubeLocalScale.x / 2;
        float distanceBeforeTouchY = rayonBalle + cubeLocalScale.y / 2;
        float distanceBeforeTouchZ = rayonBalle + cubeLocalScale.z / 2;
        // collision ?
        if (scalarProductY < distanceBeforeTouchY && scalarProductX < distanceBeforeTouchX && scalarProductZ < distanceBeforeTouchZ &&
            scalarProductY > -distanceBeforeTouchY && scalarProductX > -distanceBeforeTouchX && scalarProductZ > -distanceBeforeTouchZ)
        {
            // on what surface ?
            if (scalarProductX < distanceBeforeTouchX && scalarProductX > distanceBeforeTouchX - 0.1f || scalarProductX > -distanceBeforeTouchX && scalarProductX < -distanceBeforeTouchX + 0.1f) return 1;
            if (scalarProductY < distanceBeforeTouchY && scalarProductY > distanceBeforeTouchY - 0.1f || scalarProductY > -distanceBeforeTouchY && scalarProductY < -distanceBeforeTouchY + 0.1f) return 2;
            if (scalarProductZ < distanceBeforeTouchZ && scalarProductZ > distanceBeforeTouchZ - 0.1f || scalarProductZ > -distanceBeforeTouchZ && scalarProductZ < -distanceBeforeTouchZ + 0.1f) return 3;
        }
        return 0;
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
}
