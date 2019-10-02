using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : Controller {

    [SerializeField] CircleRotator circleRotator;

    void Start()
    {
        ChangeStateToStop();
    }

    protected override void OnStartController()
    {
        circleRotator.gameObject.SetActive(true);

        if (circleRotator.gameObject.activeInHierarchy)
            circleRotator.SetRotationStateToStart();
    }
    protected override void OnStopController()
    {
        if(circleRotator.gameObject.activeInHierarchy)
            circleRotator.SetRotationStateToStop();

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
    public void DeactivateCircle()
    {
        circleRotator.gameObject.SetActive(false);
    }
} 
