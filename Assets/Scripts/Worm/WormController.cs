using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : Controller {
    public static WormController Instance;

    [SerializeField] WormPooler wormPooler;

    public int SetNumberOfWorm
    {
        set
        {
            numberOfWorm = value;
        }
    }

    int numberOfWorm;
    bool hasDeployedWorm;

    #region EVENTS AND DELEGATES
    public delegate void ReleaseAWormEvent();
    public delegate void EatCenterEvent();
    public delegate void NextLevelEvent();
    public delegate void NoNextLevelEvent();

    public event EatCenterEvent EatCenter;
    public event ReleaseAWormEvent ReleaseAWorm;
    public event NextLevelEvent NextLevel;
    public event NoNextLevelEvent NoNextLevel;
    #endregion

    void Start()
    {
        //to make it singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if(Instance != null)
        {
            Destroy(this);
        }
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
            FireWorm();
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
        
    }
    void DeployAWorm()
    {
        wormPooler.SpawnWorm();
    }
    public void OnWormHitSuccessfully()
    {
        numberOfWorm -= 1;
        if (numberOfWorm > 0)
        {
            hasDeployedWorm = false;
        }
        else if(numberOfWorm <= 0)
        {
            CompletedTheLevel();
        }
    }
    public void OnWormHitFailed()
    {
        // failed
        ChangeStateToStop();
        if (NoNextLevel != null)
        {
            wormPooler.ReturnAllWorm();
            NoNextLevel();
        }
    }
    void CompletedTheLevel()
    {
        if (EatCenter != null)
            EatCenter();

        StartCoroutine("WaitForNextLevel");
    }
    void FireWorm()
    {
        if (ReleaseAWorm != null)
            ReleaseAWorm();
    }
    IEnumerator WaitForNextLevel()
    {
        yield return new WaitForSeconds(5f);
        if(NextLevel != null)
        {
            NextLevel();
        }
    }
}
