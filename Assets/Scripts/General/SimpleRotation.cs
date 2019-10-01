using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour {

    public float Speed;
    private WheelJoint2D wheelJoint;
    private JointMotor2D motor;

    private void Awake()
    {
        wheelJoint = GetComponent<WheelJoint2D>();
        motor = new JointMotor2D();
        StartCoroutine("PlayRotationPattern");
    }

    private IEnumerator PlayRotationPattern()
    {
        int rotationIndex = 0;
        //infinite coroutine loop
        while (true)
        {
            //working with physics, executing as if this was running in a FixedUpdate method
            yield return new WaitForFixedUpdate();

            motor.motorSpeed = Speed;
            //hard coded 10000, feel free to experiment with other torques if you wish
            motor.maxMotorTorque = 10000;
            //set the updated motor to be the motor "sitting" on the Wheel Joint 2D
            wheelJoint.motor = motor;
        }
    }
}
