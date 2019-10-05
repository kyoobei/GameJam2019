using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : Controller {

    [SerializeField] CircleRotator circleRotator;
    [SerializeField] Animator circleRotatorAnimator;

    bool shouldStartRotating;
    bool isEaten;

    void Start()
    {
        ChangeStateToStop();
    }

    protected override void OnStartController()
    {
        circleRotator.gameObject.SetActive(true);

        if (!shouldStartRotating)
        {
            //ResetEatenCircle();
            shouldStartRotating = true;
            circleRotator.SetRotationStateToStart();
        }
        if(isEaten)
        {
            circleRotator.SetRotationStateToStop();
        }
        else if(!isEaten)
        {
            if(shouldStartRotating)
            {
                shouldStartRotating = false;
            }
        }
    }
    protected override void OnStopController()
    {
        if (shouldStartRotating)
        {
            shouldStartRotating = false;
            StopRotationOfCircle();
        }
        isEaten = false;
        circleRotator.gameObject.SetActive(false);
    }
    protected override void OnResetController()
    {
        isEaten = false;

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
    public void PlayEatenCirle()
    {
        circleRotatorAnimator.SetTrigger("isEaten");
        
    }
    public void EatCircle()
    {
        isEaten = true;
    }
    /*
    public void ResetEatenCircle()
    {
        circleRotatorAnimator.Play("animationOrangeIdle");
    }
    */
} 
