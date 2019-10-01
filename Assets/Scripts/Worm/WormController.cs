using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : Controller {

    public static WormController Instance;

    public int numberOfWorm;

    [SerializeField] WormPooler wormPooler;

    bool hasDeployedWorm;

    public delegate void ReleaseAWormEvent();
    public delegate void EatCenterEvent();

    public event EatCenterEvent EatCenter;
    public event ReleaseAWormEvent ReleaseAWorm;

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
    protected override void OnStopController()
    {
        
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

    }
    void CompletedTheLevel()
    {
        if (EatCenter != null)
            EatCenter();
    }
    void FireWorm()
    {
        if (ReleaseAWorm != null)
            ReleaseAWorm();
    }
}
