using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    const float WAIT_TIME_LOSING = 1f;

    [SerializeField] CircleController circleController;
    [SerializeField] WormController wormController;
    [SerializeField] UIManager uiManager;

    public int GetNumberOfStages
    {
        get{ return numberOfStage; }
    }
    public int GetWormCountForStage
    {
        get { return numberOfWorm; }
    }

    int numberOfStage;
    int numberOfWorm;

    int currentStage;
    bool isInBossStage;
    bool isGameInitialized;

    enum GameState
    {
        InMainMenu,
        InGame,
        InDefeatMenu
    };

    GameState gameState;

    void Start()
    {
        currentStage = 0;
        gameState = GameState.InMainMenu;

        uiManager.StartGame += SwitchStateToInGame;

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
                uiManager.ActivateMainMenu(true);
                uiManager.ActivateInGameMenu(false);
                uiManager.ActivateDefeatMenu(false);

                ResetValues();
                break;
            case GameState.InGame:
                //do game logic here
                uiManager.ActivateMainMenu(false);
                uiManager.ActivateInGameMenu(true);
                uiManager.ActivateDefeatMenu(false);

                InitializeGameValues();

                if(currentStage > numberOfStage)
                {
                    isInBossStage = true;
                }

                break;
            case GameState.InDefeatMenu:
                //load UI for defeat menu
                uiManager.ActivateMainMenu(false);
                uiManager.ActivateInGameMenu(false);
                uiManager.ActivateDefeatMenu(true);

                break;
        }
    }

    void SwitchStateToMainMenu()
    {
        gameState = GameState.InMainMenu;
    }
    void SwitchStateToInGame()
    {
        gameState = GameState.InGame;
    }
    void SwitchStateToDefeat()
    {
        gameState = GameState.InDefeatMenu;
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
        StartCoroutine("WaitForStoppingGame");
    }
    IEnumerator WaitForStoppingGame()
    {
        circleController.StopRotationOfCircle();

        yield return new WaitForSeconds(WAIT_TIME_LOSING);

        SwitchStateToDefeat();

        wormController.ChangeStateToStop();
        circleController.ChangeStateToStop();

        ResetValues();
    }
    void ResetValues()
    {
        isGameInitialized = false;
        isInBossStage = false;
        numberOfStage = 0;
        numberOfWorm = 0;
    }
}
