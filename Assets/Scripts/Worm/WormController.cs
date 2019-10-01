using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : Controller {

    public static WormController Instance;

    [SerializeField] WormPooler wormPooler;

    bool hasDeployedWorm;

    public delegate void ReleaseAWormEvent();
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

    }
    protected override void OnStopController()
    {
        
    }
    void DeployAWorm()
    {
        wormPooler.SpawnWorm();
    }
    public void OnWormHit()
    {

    }
    public void FireWorm()
    {
        if (ReleaseAWorm != null)
            ReleaseAWorm();
    }
}
