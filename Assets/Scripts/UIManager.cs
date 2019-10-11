using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {
    [Header("UI Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject inGameMenuPanel;
    [SerializeField] GameObject defeatMenuPanel;

    [Header("UI Animators")]
    [SerializeField] Animator startButtonAnimator;
    [SerializeField] Animator HeaderTitleAnimator;
    [SerializeField] Animator DefeatButtonsAnimator;

    [Header("Others")]
    [SerializeField] Text stageCounterText;
    [SerializeField] Text highScoreCounterText;

    [Header("Buttons")]
    [SerializeField] Button restartButton;

    public int SetStageCounterText
    {
        set
        {
            stageCounterText.text = value.ToString();
        }
    }

    #region EVENTS AND DELEGATES
    public delegate void StartGameEvent();
    
    public event StartGameEvent StartGame;
    #endregion

    bool isStartSelected;

    void Start()
    {
        restartButton.onClick.AddListener(AdController.Instance.ShowRewardedAd);
    }

    void Update()
    {
        if(isStartSelected)
        {
            if(IsTransitionDone(HeaderTitleAnimator, "animationHeaderFadeOut"))
            {
                if (StartGame != null)
                {
                    StartGame();
                    isStartSelected = false;
                }
            }
        }
        //if ready restart button will be interactable; maybe change later
        //restartButton.interactable = AdController.Instance.IsInterstitialReady();

        highScoreCounterText.text = PlayerPrefs.GetString("PlayerScore"); 
    }

    bool IsTransitionDone(Animator animatorToCheckUp, string nameOfAnimation)
    {
        if (animatorToCheckUp.GetCurrentAnimatorStateInfo(0).IsName(nameOfAnimation) &&
            animatorToCheckUp.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            return true;

        return false;
    }

    public bool IsDefeatPanelExitDone()
    {
        if (DefeatButtonsAnimator.GetCurrentAnimatorStateInfo(0).IsName("animationDefeatPanelButtonsExit") &&
            DefeatButtonsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            return true;
        return false;
    }

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
        startButtonAnimator.SetTrigger("isHidden");
        HeaderTitleAnimator.SetTrigger("isHidden");
        isStartSelected = true;
    }

    //public void StartButtonSlideIn()
    //{
    //    HeaderTitleAnimator.SetBool("isHidden", false);
    //}

    public void BackToMainPressed()
    {
        DefeatButtonsAnimator.SetTrigger("BackToMainPressed");
    }

    public void RetryPressed()
    {
        DefeatButtonsAnimator.SetTrigger("RetryPressed");
    }


}
