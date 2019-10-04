using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : Controller {

    [SerializeField] CircleRotator circleRotator;

    bool shouldStartRotating;

    void Start()
    {
        ChangeStateToStop();
    }

    protected override void OnStartController()
    {
        circleRotator.gameObject.SetActive(true);

        if (!shouldStartRotating)
        {
            shouldStartRotating = true;
            circleRotator.SetRotationStateToStart();
        }
    }
    protected override void OnStopController()
    {
        if (shouldStartRotating)
        {
            shouldStartRotating = false;
            StopRotationOfCircle();
        }

        circleRotator.gameObject.SetActive(false);
    }
    protected override void OnResetController()
    {

        int randRotation = (Random.Range(0, 10) % 2);
        int randSpeedModifier = (Random.Range(0, 10) % 2);

        circleRotator.IsRotationRandom = (randRotation == 0) ? true : false;
        circleRotator.HasRandomSpeedModifier = (randSpeedModifier == 0) ? true : false;
        circleRotator.InitializeRotation();

        ChangeStateToStart();
    }
    public void StopRotationOfCircle()
    {
        
        circleRotator.SetRotationStateToStop();
    }
    public void DeactivateCircle()
    {
        circleRotator.gameObject.SetActive(false);
    }
} 
