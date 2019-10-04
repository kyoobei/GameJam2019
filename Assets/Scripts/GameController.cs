using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] CircleController circleController;
    [SerializeField] WormController wormController;

    public int GetNumberOfStages
    {
        get{ return numberOfStage; }
    }
    public int GetWormCountForStage
    {
        get { return numberOfWorm; }
    }

    [SerializeField] int currentStage;
    [SerializeField] int numberOfStage;
    [SerializeField] int numberOfWorm;
    bool isInBossStage;
    bool isGameInitialized;
    bool shouldStopGame;

    enum GameState
    {
        InMainMenu,
        InGame,
        InDefeatMenu
    };

    [SerializeField] GameState gameState;

    void Start()
    {
        currentStage = 0;
        gameState = GameState.InGame;

        wormController.NextLevel -= OnVictory;
        wormController.NoNextLevel -= OnLosing;

        wormController.NextLevel += OnVictory;
        wormController.NoNextLevel += OnLosing;
    }

    void Update()
    {
        UpdateGameState();
    }

    void UpdateGameState()
    {
        switch(gameState)
        {
            case GameState.InMainMenu:
                //load UI for main menu
                break;
            case GameState.InGame:
                //do game logic here
                InitializeGameValues();

                if(currentStage > numberOfStage)
                {
                    isInBossStage = true;
                }

                break;
            case GameState.InDefeatMenu:
                //load UI for defeat menu
                break;
        }
    }

    void InitializeGameValues()
    {
        if (!isGameInitialized)
        {
            isGameInitialized = true;
            numberOfStage = Random.Range(3, 5);
            PassValuesToController();
        }
    }
    void PassValuesToController()
    {
        numberOfWorm = Random.Range(3, 7);

        wormController.SetNumberOfWorm = numberOfWorm;

        wormController.ChangeStateToReset();
        circleController.ChangeStateToReset();

    }
    public void OnVictory()
    {
        if(isInBossStage)
        {
            currentStage = 0;
            isInBossStage = false;
        }
        else
        {
            currentStage++;
            
        }
        circleController.DeactivateCircle();
        PassValuesToController();
    }
    public void OnLosing()
    {
        Debug.Log("called");
        StartCoroutine("WaitForStoppingGame");
    }
    IEnumerator WaitForStoppingGame()
    {
        circleController.StopRotationOfCircle();

        yield return new WaitForSeconds(1f);

        gameState = GameState.InDefeatMenu;

        wormController.ChangeStateToStop();
        circleController.ChangeStateToStop();

        isGameInitialized = false;
        isInBossStage = false;
        numberOfStage = 0;
        numberOfWorm = 0;
    }
}
