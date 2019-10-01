using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotator : MonoBehaviour {

    //dont add variables here since its not really for adding but viewing
    [SerializeField] float currentSpeedRotation;
    [SerializeField] float currentTimerDuration;
    Quaternion currentTargetRotation;
    bool isRotating;

    void Start()
    {
        currentTargetRotation = Quaternion.identity;
    }
    void Update()
    {
        RotateObject();
    }
    void RotateObject()
    {
        if(!isRotating)
        {
            if(IsNearTargetRotation())
            {
                currentSpeedRotation = 100f;
                currentTimerDuration = Random.Range(2f, 5f);
                ResetTargetObjectRotation();
                isRotating = true;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, 2f * Time.deltaTime);
            }
            
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
            else if(currentTimerDuration <= 0f)
            {

                isRotating = false;
            }
        }
    }
    void ResetTargetObjectRotation()
    {
        float modifiedRotationZ;
        if(transform.rotation.z > 0)
        {
            //if positive;
            modifiedRotationZ = transform.rotation.z + 10f;
        }
        else
        {
            //if negative
            modifiedRotationZ = transform.rotation.z - 10f;
        }
        currentTargetRotation = Quaternion.Euler(0, 0, modifiedRotationZ);
    }
    bool IsNearTargetRotation()
    {
        if (Quaternion.Angle(transform.rotation, currentTargetRotation) <= 0.05f)
            return true;
        return false;
    }
    
}
