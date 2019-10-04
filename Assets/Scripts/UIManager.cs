using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [Header("UI Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject inGameMenuPanel;
    [SerializeField] GameObject defeatMenuPanel;

    [Header("UI Animators")]
    [SerializeField] Animator startButtonAnimator;
    [SerializeField] Animator HeaderTitleAnimator;

    #region EVENTS AND DELEGATES
    public delegate void StartGameEvent();
    
    public event StartGameEvent StartGame;
    #endregion

    public void ActivateMainMenu(bool isActive)
    {
        mainMenuPanel.SetActive(isActive);
    }
    public void ActivateInGameMenu(bool isActive)
    {
        inGameMenuPanel.SetActive(isActive);
    }
    public void ActivateDefeatMenu(bool isActive)
    {
        defeatMenuPanel.SetActive(isActive);
    }

    public void StartButtonSlideOut()
    {
        startButtonAnimator.SetBool("isHidden", true);
        HeaderTitleAnimator.SetBool("isHidden", true);
    }

    public void StartButtonSlideIn()
    {
        HeaderTitleAnimator.SetBool("isHidden", false);
    }



}
