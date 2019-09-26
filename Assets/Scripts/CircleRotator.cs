using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotator : MonoBehaviour {


    [SerializeField] float currentSpeedRotation;
    [SerializeField] float currentTimerDuration;
    Quaternion currentTargetRotation;
    bool isRotating;

    void Start()
    {
    
    }
    void Update()
    {
        RotateObject();
    }
    void RotateObject()
    {
        if(!isRotating)
        {
            currentSpeedRotation = Random.Range(-20f, 20f);
            currentTimerDuration = Random.Range(2f, 5f);
            isRotating = true;
        }
        else
        {
            if (currentTimerDuration > 0f)
            {
                transform.Rotate
                    (
                        0f, 0f, currentSpeedRotation * Time.deltaTime
                    );
                currentTimerDuration -= Time.deltaTime;
            }
            else
            {
               
            }
        }
        /*
        if(!isRotating)
        {
            //ResetTargetObjectRotation();
            isRotating = true;
        }
        else
        {
            /*
            if (IsNearTargetRotation())
            {
                isRotating = false;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, currentSpeedRotation * Time.deltaTime);
            }
            
        }
        */
    }
    void ResetTargetObjectRotation()
    {
        float randomRotation = Random.Range(0, 359f);
        currentTargetRotation = Quaternion.Euler(0, 0, randomRotation);
        currentSpeedRotation = 2f;//Random.Range(5f, 10f);
    }
    bool IsNearTargetRotation()
    {
        if (Quaternion.Angle(transform.rotation, currentTargetRotation) <= 0.05f)
            return true;
        return false;
    }
    
}
