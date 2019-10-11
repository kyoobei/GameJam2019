using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotator : MonoBehaviour {

    const float DEFAULT_CIRCLE_SPEED = 200f;

    float currentSpeedRotation;
    float currentTimerDuration;
    Quaternion currentTargetRotation;           //the targetRotation that shoul
    bool isRotating;

    public bool IsRotationRandom
    {
        get; set;
    }
    public bool HasRandomSpeedModifier
    {
        set
        {
            if (value.Equals(true))
            {
                speedModifier = Random.Range(1f, 2f);
            }
            else
            {
                speedModifier = 1f;
            }
        }
    }
    float speedModifier;
    enum RotationState
    {
        START,
        STOP
    };

    RotationState rotationState;
    
    void Start()
    {
        currentTargetRotation = Quaternion.identity;
    }
    void Update()
    {
        if(rotationState == RotationState.START)
            RotateObject();
    }
    void RotateObject()
    {
        if(!isRotating)
        {
            if(IsNearTargetRotation())
            {
                SetCurrentRotationSpeed();
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
    void SetCurrentRotationSpeed()
    {
        if (IsRotationRandom)
        {
            int getRandomNumber = Random.Range(1, 7);
            int getModulo = getRandomNumber % 2;
            if (getModulo.Equals(0))
            {
                currentSpeedRotation = DEFAULT_CIRCLE_SPEED * speedModifier;
            }
            else
            {
                currentSpeedRotation = (-1f * DEFAULT_CIRCLE_SPEED) * speedModifier;
            }
        }
        else
        {
            currentSpeedRotation = DEFAULT_CIRCLE_SPEED * speedModifier;
        }

        currentTimerDuration = Random.Range(2f, 10f);
        ResetTargetObjectRotation();
        isRotating = true;
    }
    void ResetTargetObjectRotation()
    {
        float modifiedRotationZ = 0f;

        if(transform.rotation.z > 0)
        {
            //if positive;
            modifiedRotationZ = transform.rotation.z + 10f;
        }
        else if(transform.rotation.z <= 0)
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
    public void InitializeRotation()
    {
        //reset values;
        currentTargetRotation = Quaternion.identity;
        currentTimerDuration = 0;
        transform.rotation = Quaternion.identity;
        isRotating = false;
    }
    public void SetRotationStateToStart()
    {
        rotationState = RotationState.START;
    }
    public void SetRotationStateToStop()
    {
        rotationState = RotationState.STOP;
    }
    
}
