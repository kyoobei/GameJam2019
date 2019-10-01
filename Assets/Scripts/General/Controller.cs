using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    enum ControllerState
    {
        START,
        STOP
    }
    ControllerState controllerState;

    void Start()
    {
        controllerState = ControllerState.STOP;
    }
    void Update()
    {
        UpdateState();
    }
    void UpdateState()
    {
        switch(controllerState)
        {
            case ControllerState.START:
                OnStartController();
                break;
            case ControllerState.STOP:
                OnStopController();
                break;
        }
    }
    #region PROTECTED METHODS
    protected virtual void OnStartController()
    {
        //add code here
    }
    protected virtual void OnStopController()
    {
        //add code here
    }
    #endregion
    #region PUBLIC METHODS
    public virtual void ChangeStateToStart()
    {
        controllerState = ControllerState.START;
    }
    public virtual void ChangeStateToStop()
    {
        controllerState = ControllerState.STOP;
    }
    #endregion
}
