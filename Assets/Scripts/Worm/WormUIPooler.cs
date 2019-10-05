using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WormUIPooler : ClonePooler {
    //values acquired from "prefab"
    const float DEFAULT_ANCHOR_MIN_X = 0.1574f;
    const float DEFAULT_ANCHOR_MIN_Y = 0.05f;
    const float DEFAULT_ANCHOR_MAX_X = 0.8425f;
    const float DEFAULT_ANCHOR_MAX_Y = 0.0898f;
    const float DEFAULT_PIVOT_X = 0.5f;
    const float DEFAULT_PIVOT_Y = 0.5f;

    List<GameObject> wormUIMainReleased = new List<GameObject>();
    List<GameObject> wormUIMainList = new List<GameObject>();

    int currentWormUI;

    public override void Start()
    {
        for(int i = 0; i < numberOfCopy; i++)
        {
            GameObject clonedWormUI = Instantiate(originalObject) as GameObject;
            if(possibleCloneParent != null)
            {
                clonedWormUI.transform.parent = possibleCloneParent;
            }
            else
            {
                possibleCloneParent = transform;
                clonedWormUI.transform.parent = possibleCloneParent;
            }
            //reposition everything
            RectTransform wormRect = clonedWormUI.GetComponent<RectTransform>();

            wormRect.transform.localPosition = Vector3.zero;
            wormRect.transform.localScale = Vector3.one;

            wormRect.offsetMax = new Vector2(-1f, -1f);
            wormRect.offsetMin = new Vector2(1f, 1f);

            //add to queue
            queuedClones.Enqueue(clonedWormUI);

            clonedWormUI.SetActive(false);
        }
        numberOfCopy = queuedClones.Count;
    }
    public void ActivateWormUI(int numberOfWorm)
    {
        Vector3 lastPositionOfObject = Vector3.zero;
        Vector2 lastAnchorMin = Vector2.zero;
        Vector2 lastAnchorMax = Vector2.zero;

        for (int i = 0; i < numberOfWorm; i++)
        {
            GameObject clone = AcquireClone();
            if (lastPositionOfObject.Equals(Vector3.zero))
            {
                lastPositionOfObject = clone.transform.localPosition;
                RectTransform wormRect = clone.GetComponent<RectTransform>();

                lastAnchorMin = wormRect.anchorMin;
                lastAnchorMax = wormRect.anchorMax;
            }
            else
            {   
                RectTransform wormRect = clone.GetComponent<RectTransform>();

                //reposition anchors and pivot
                wormRect.anchorMin = new Vector2
                    (
                        DEFAULT_ANCHOR_MIN_X,
                        lastAnchorMin.y + 0.05f
                    );

                wormRect.anchorMax = new Vector2
                    (
                        DEFAULT_ANCHOR_MAX_X,
                        lastAnchorMax.y + 0.05f
                    );
                wormRect.pivot = new Vector2
                    (
                        DEFAULT_PIVOT_X,
                        DEFAULT_PIVOT_Y
                    );

                lastAnchorMin = wormRect.anchorMin;
                lastAnchorMax = wormRect.anchorMax;

                //set offsets
                wormRect.offsetMax = new Vector2(-1f, -1f);
                wormRect.offsetMin = new Vector2(1f, 1f);
                
                //set last position
                lastPositionOfObject = clone.transform.localPosition;
            }
            //add to released wormUIforTracking
            wormUIMainReleased.Add(clone.gameObject);
            //add to list the child object of the cloned worm
            wormUIMainList.Add(clone.transform.GetChild(0).gameObject);

            clone.SetActive(true);
        }

        currentWormUI = numberOfWorm - 1;
    }
    public void DeactivateWormUIMain()
    {
        wormUIMainList[currentWormUI].SetActive(false);

        if(currentWormUI >= 0)
            --currentWormUI;
    }
    //public
    public void ReturnAllWormUI()
    {
        for(int i = 0; i < wormUIMainReleased.Count; i++)
        {
            RectTransform wormRect = wormUIMainReleased[i].GetComponent<RectTransform>();

            //reposition anchors and pivot
            wormRect.anchorMin = new Vector2
                (
                    DEFAULT_ANCHOR_MIN_X,
                    DEFAULT_ANCHOR_MIN_Y
                );

            wormRect.anchorMax = new Vector2
                (
                    DEFAULT_ANCHOR_MAX_X,
                    DEFAULT_ANCHOR_MAX_Y
                );
            wormRect.pivot = new Vector2
                (
                    DEFAULT_PIVOT_X,
                    DEFAULT_PIVOT_Y
                );

            //set offsets
            wormRect.offsetMax = new Vector2(-1f, -1f);
            wormRect.offsetMin = new Vector2(1f, 1f);

            ReturnClone(wormUIMainReleased[i].gameObject);
            wormUIMainReleased[i].SetActive(false);
            //turn on previous off
            wormUIMainList[i].SetActive(true);
        }
        currentWormUI = 0;
        //clear list
        wormUIMainReleased.Clear();
        wormUIMainList.Clear();
    }
}
