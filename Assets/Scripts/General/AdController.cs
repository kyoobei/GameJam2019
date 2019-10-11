using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdController : MonoBehaviour
{
    public static AdController Instance;
    //testing devices
    //can be acquired by downloading "Device ID" on Google Play
    const string ID_DEVICE_SAMSUNG = "421512F7AAB0A833";

    const string ANDROID_ADMOB_ID = "ca-app-pub-6451912366364729~1725912382";

    const string ADMOB_BANNER_ID = "ca-app-pub-6451912366364729/5026863148";
    const string ADMOB_REWARDED_ID = "ca-app-pub-6451912366364729/3866319551";
    const string ADMOB_INTERSTITIAL_ID = "ca-app-pub-6451912366364729/4632606314";

    //ID's to use for testAds
    const string TEST_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    const string TEST_REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";
    const string TEST_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";

    [SerializeField] bool isInTestingMode;

    string adUnitID;

    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    public delegate void FinishedInterstitialAdEvent();
    public delegate void FinishedRewardedAdEvent();

    public event FinishedInterstitialAdEvent FinishedInterstitialAd;
    public event FinishedRewardedAdEvent FinishedRewardedAd;

    void Awake()
    {
        //to make classes static
        Instance = this;
    }
    void Start()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        adUnitID = ANDROID_ADMOB_ID;
#endif
        MobileAds.Initialize(adUnitID);

        ShowBannerAd();
    }

    public void ShowBannerAd()
    {
        AdSize adSize = new AdSize(250, 50);

        if (isInTestingMode)
        {
            //use tester id
            bannerView = new BannerView(TEST_BANNER_ID, adSize, AdPosition.Bottom);
            //add events
            RemoveEventsToBannerAd();
            AddEventsToBannerAd();
            AdRequest bannerRequest = new AdRequest.Builder().AddTestDevice(ID_DEVICE_SAMSUNG).Build();
            bannerView.LoadAd(bannerRequest);
        }
        else
        {
            //use admob provided ID to load actual ads
            bannerView = new BannerView(ADMOB_BANNER_ID, adSize, AdPosition.Bottom);
            //add events
            RemoveEventsToBannerAd();
            AddEventsToBannerAd();
            AdRequest bannerRequest = new AdRequest.Builder().Build();
            bannerView.LoadAd(bannerRequest);
        }
        
    }
    /// <summary>
    /// call this function to request first for an interstitalAd
    /// </summary>
    public void RequestInterstitialAd()
    {
        
        if(isInTestingMode)
        {
            interstitialAd = new InterstitialAd(TEST_INTERSTITIAL_ID);

            RemoveEventsToInterstialAd();
            AddEventsToInterstialAd();

            AdRequest interstitialRequest = new AdRequest.Builder().AddTestDevice(ID_DEVICE_SAMSUNG).Build();
            interstitialAd.LoadAd(interstitialRequest);
        }
        else
        {
            interstitialAd = new InterstitialAd(ADMOB_INTERSTITIAL_ID);

            RemoveEventsToInterstialAd();
            AddEventsToInterstialAd();

            AdRequest interstitialRequest = new AdRequest.Builder().Build();
            interstitialAd.LoadAd(interstitialRequest);
        }
    }
    /// <summary>
    /// call this function to request for a rewarded ad
    /// </summary>
    public void RequestRewardedAd()
    {

        if (isInTestingMode)
        {
            rewardedAd = new RewardedAd(TEST_REWARDED_ID);

            RemoveEventsToRewardedAd();
            AddEventsToRewardedAd();

            AdRequest rewardedRequest = new AdRequest.Builder().AddTestDevice(ID_DEVICE_SAMSUNG).Build();
            rewardedAd.LoadAd(rewardedRequest);
        }
        else
        {
            rewardedAd = new RewardedAd(ADMOB_REWARDED_ID);

            RemoveEventsToRewardedAd();
            AddEventsToRewardedAd();

            AdRequest rewardedRequest = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(rewardedRequest);
        }
    }
    public bool IsInterstitialReady()
    {
        return interstitialAd.IsLoaded();
    }
    public bool IsRewardedReady()
    {
        return rewardedAd.IsLoaded();
    }
    /// <summary>
    /// call this function to show interstital ad
    /// </summary>
    public void ShowInterstitalAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            //in case no ad has been acquired
            FinishedShowingInterstitialAd();
        }
    }
    /// <summary>
    /// call this function to show rewarded ad
    /// </summary>
    public void ShowRewardedAd()
    {
        if(rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            //in case no ad has been acquired
            FinishedShowingRewardedAd();
        }
    }

    #region DESTROY ADS 
    public void DestroyInterstitialAd()
    {
        RemoveEventsToInterstialAd();
        interstitialAd.Destroy();
    }
    public void DestroyBannerAd()
    {
        //to free up memory from the call
        RemoveEventsToBannerAd();
        bannerView.Destroy();
    }
    #endregion

    #region ADD EVENTS
    void AddEventsToInterstialAd()
    {
        interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        interstitialAd.OnAdOpening += HandleOnAdOpened;
        interstitialAd.OnAdClosed += HandleOnAdClosed;
        interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }
    void AddEventsToBannerAd()
    {
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        bannerView.OnAdOpening += HandleOnAdOpened;
        bannerView.OnAdClosed += HandleOnAdClosed;
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }
    void AddEventsToRewardedAd()
    {
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }
    #endregion

    #region REMOVE EVENTS
    void RemoveEventsToInterstialAd()
    {
        interstitialAd.OnAdLoaded -= HandleOnAdLoaded;
        interstitialAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        interstitialAd.OnAdOpening -= HandleOnAdOpened;
        interstitialAd.OnAdClosed -= HandleOnAdClosed;
        interstitialAd.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
    }
    void RemoveEventsToBannerAd()
    {
        bannerView.OnAdLoaded -= HandleOnAdLoaded;
        bannerView.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        bannerView.OnAdOpening -= HandleOnAdOpened;
        bannerView.OnAdClosed -= HandleOnAdClosed;
        bannerView.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
    }
    void RemoveEventsToRewardedAd()
    {
        rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
        rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
    }
    #endregion

    #region EVENT METHODS FOR NOT REWARDED ADS
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //this will be called when event is executed when an ad has finished loading.
        if(sender.Equals(bannerView))
        {
           
        }
        if(sender.Equals(interstitialAd))
        {
           
        }
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //this will be called when event is invoked when an ad fails to load.
        if (sender.Equals(bannerView))
        {
            // add for banner
            ShowBannerAd();
        }
        if(sender.Equals(interstitialAd))
        {
            RequestInterstitialAd();
        }
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //this will be called when user taps on an ad.
        if (sender.Equals(bannerView))
        {
            // add for banner
        }
        if(sender.Equals(interstitialAd))
        {

        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //this will be called When a user returns to the app after viewing an ad's destination URL
        if (sender.Equals(bannerView))
        {
            // add for banner
        }
        if(sender.Equals(interstitialAd))
        {
            //if user pressed close/skip button
            FinishedShowingInterstitialAd();
        }
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        //this will be called when a user click opens another app 
        if (sender.Equals(bannerView))
        {
           // add for banner
        }
        if(sender.Equals(interstitialAd))
        {

        }
    }
    #endregion
    
    #region EVENT METHODS FOR REWARDED ADS
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        RequestRewardedAd();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        FinishedShowingRewardedAd();
        RequestRewardedAd();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        FinishedShowingRewardedAd();
    }
    #endregion

    void FinishedShowingInterstitialAd()
    {
        if (FinishedInterstitialAd != null)
        {
            RemoveEventsToInterstialAd();
            FinishedInterstitialAd();
        }
    }
    void FinishedShowingRewardedAd()
    {
        if(FinishedRewardedAd != null)
        {
            RemoveEventsToRewardedAd();
            FinishedRewardedAd();
        }
    }
}
