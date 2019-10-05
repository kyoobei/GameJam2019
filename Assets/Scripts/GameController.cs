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
    bool isGameInitialized;
    bool fromDefeatMenu = false;

    enum GameState
    {
        InMainMenu,
        InGame,
        InDefeatMenu
    };

    GameState gameState;

    void Start()
    {
        currentStage = 1;
        gameState = GameState.InMainMenu;

        uiManager.StartGame += SwitchStateToInGame;

        wormController.NextLevel -= OnVictory;
        wormController.NoNextLevel -= OnLosing;
        wormController.EatCenter -= circleController.StopRotationOfCircle;
        wormController.CompletedEating -= circleController.EatCircle;

        wormController.CompletedEating += circleController.PlayEatenCirle;
        wormController.NextLevel += OnVictory;
        wormController.NoNextLevel += OnLosing;
        wormController.EatCenter += circleController.EatCircle;

        PlayerPrefs.SetString("PlayerScore", currentStage.ToString());
    }

    void Update()
    {
        UpdateGameState();

        uiManager.SetStageCounterText = currentStage;
    }

    void UpdateGameState()
    {
        switch(gameState)
        {
            case GameState.InMainMenu:
                if (fromDefeatMenu)
                {
                    if (uiManager.IsDefeatPanelExitDone())
                        fromDefeatMenu = false;
                }
                else
                {
                    //load UI for main menu
                    uiManager.ActivateMainMenu(true);
                    uiManager.ActivateInGameMenu(false);
                    uiManager.ActivateDefeatMenu(false);

                    ResetValues();
                }
                break;
            case GameState.InGame:
                if (fromDefeatMenu)
                {
                    if (uiManager.IsDefeatPanelExitDone())
                        fromDefeatMenu = false;
                }
                else
                {
                    //do game logic here
                    uiManager.ActivateMainMenu(false);
                    uiManager.ActivateInGameMenu(true);
                    uiManager.ActivateDefeatMenu(false);

                    InitializeGameValues();

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

    public void SwitchStateToMainMenu()
    {
        gameState = GameState.InMainMenu;
        if(fromDefeatMenu)
            uiManager.BackToMainPressed(); 
    }
    public void SwitchStateToInGame()
    {
        gameState = GameState.InGame;
        if(fromDefeatMenu)
            uiManager.RetryPressed();
    }
    public void SwitchStateToDefeat()
    {
        gameState = GameState.InDefeatMenu;
        fromDefeatMenu = true;
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
        currentStage++;
        PlayerPrefs.SetString("PlayerScore", currentStage.ToString());
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
        //circleController.ResetEatenCircle();
        currentStage = 1;
        numberOfStage = 0;
        numberOfWorm = 0;
    }
}
