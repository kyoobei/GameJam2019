using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Animator startButtonAnimator;
    public Animator HeaderTitleAnimator;
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
