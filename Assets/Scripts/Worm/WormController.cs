using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : Controller {
    public static WormController Instance;

    [SerializeField] WormPooler wormPooler;
    [SerializeField] WormUIPooler wormUIPooler;
    [SerializeField] AudioController audioController;

    public int SetNumberOfWorm
    {
        set
        {
            numberOfWorm = value;
            if(wormUIPooler != null)
            {
                wormUIPooler.ActivateWormUI(value);
            }
        }
    }

    int numberOfWorm;
    bool hasDeployedWorm;
    bool hasFired;

    #region EVENTS AND DELEGATES
    public delegate void ReleaseAWormEvent();
    public delegate void EatCenterEvent();
    public delegate void NextLevelEvent();
    public delegate void NoNextLevelEvent();
    public delegate void CompletedEatingEvent();

    public event EatCenterEvent EatCenter;
    public event ReleaseAWormEvent ReleaseAWorm;
    public event NextLevelEvent NextLevel;
    public event NoNextLevelEvent NoNextLevel;
    public event CompletedEatingEvent CompletedEating;
    #endregion

    void Start()
    {
        //to make it singleton
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }/*
        else if(Instance != null)
        {
            Destroy(this);
        }
        */
        //if wormpooler is not yet added
        if (wormPooler == null)
            wormPooler = transform.GetComponentInChildren<WormPooler>();

        ChangeStateToStop();
    }

    protected override void OnStartController()
    {
        if(!hasDeployedWorm)
        {
            DeployAWorm();
            hasDeployedWorm = true;
        }
        //if there is a press
        if(Input.GetMouseButtonDown(0))
        {
            if (!hasFired)
            {
                hasFired = true;
                audioController.PlayWormThrow();
                FireWorm();
            }
        }
    }
    protected override void OnResetController()
    {
        //if next level
        hasDeployedWorm = false;
        ChangeStateToStart();
    }
    protected override void OnStopController()
    {
        //if game over or something
        wormPooler.ReturnAllWorm();
        wormUIPooler.ReturnAllWormUI();
    }
    void DeployAWorm()
    {
        wormPooler.SpawnWorm();
        hasFired = false;
    }
    public void OnWormHitSuccessfully()
    {
        numberOfWorm -= 1;
        audioController.PlayWormHit();
        if (numberOfWorm > 0)
        {
            hasDeployedWorm = false;
        }
        else if(numberOfWorm <= 0)
        {
            CompletedTheLevel();
        }

        if (wormUIPooler != null)
        {
            wormUIPooler.DeactivateWormUIMain();
        }
    }
    public void OnWormHitFailed()
    {
        // failed
        //ChangeStateToStop();
        if (NoNextLevel != null)
        {
            wormUIPooler.ReturnAllWormUI();
            NoNextLevel();
        }
    }
    void CompletedTheLevel()
    {
        if (EatCenter != null)
            EatCenter();

        audioController.PlayWormSuccessful();
       
        StartCoroutine("WaitForNextLevel");
    }
    void FireWorm()
    {
        if (ReleaseAWorm != null)
            ReleaseAWorm();
    }
    IEnumerator WaitForNextLevel()
    {
        yield return new WaitForSeconds(1f);
        if(CompletedEating != null)
        {
            CompletedEating();
        }
        yield return new WaitForSeconds(2f);
        if(NextLevel != null)
        {
            wormUIPooler.ReturnAllWormUI();
            NextLevel();
            StopCoroutine("WaitForNextLevel");
        }
    }
}
