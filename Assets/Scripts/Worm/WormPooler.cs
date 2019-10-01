using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPooler : ClonePooler
{
    const float WORM_POSITION_X = 0.5f;
    const float WORM_POSITION_Y = 0.15f;
    [SerializeField] List<Worm> listOfReleasedWorms = new List<Worm>();

    public delegate void FireWormEvent();
    public event FireWormEvent FireWorm;

    void Update()
    {
        if (listOfReleasedWorms.Count <= 0)
            return;

        foreach(Worm wormObj in listOfReleasedWorms)
        {
            if(wormObj.gameObject.activeInHierarchy)
            {
                if(IsCurrentWormOutOfBounds(wormObj.gameObject))
                {
                    //deactivate the worm manually
                    wormObj.gameObject.SetActive(false);
                }
            }
            else if(!wormObj.gameObject.activeInHierarchy)
            {
                //return to clone
                ReturnClone(wormObj.gameObject);
                //remove from current list of worm
                listOfReleasedWorms.Remove(wormObj);
            }
        }
    }
    bool IsCurrentWormOutOfBounds(GameObject objToCompare)
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(objToCompare.transform.position);
        if(pos.y < 0f)
        {
            return true;
        }
        return false;
    }
    Vector3 GetWormPositionBaseOnCamera()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3
            (
                WORM_POSITION_X, 
                WORM_POSITION_Y
            ));
    }
    /// <summary>
    /// Call this to release a worm
    /// </summary>
    public void SpawnWorm()
    {
        GameObject objCloned = AcquireClone();
        Worm objWorm = objCloned.GetComponent<Worm>();
        //removes the object clone from the parent area

        objCloned.transform.position = new Vector3
            (
                GetWormPositionBaseOnCamera().x,
                GetWormPositionBaseOnCamera().y,
                0f
            );

        objCloned.transform.rotation = Quaternion.identity;
        //initialize worm from the script
        objWorm.InitializeWorm();
        //add this worm to this pooler
        if (!listOfReleasedWorms.Contains(objWorm))
        {
            listOfReleasedWorms.Add(objWorm);
        }
    }
    /// <summary>
    /// Worm eats the center or the fruit if successful
    /// </summary>
    public void MoveWormsTowardsCenter()
    {
        foreach(Worm wormObj in listOfReleasedWorms)
        {
            wormObj.EatCenter();    
        }
    }
    /// <summary>
    /// Deactivate All Worm
    /// </summary>
    public void ReturnAllWorm()
    {
        foreach(Worm wormObj in listOfReleasedWorms)
        {
            wormObj.gameObject.SetActive(false);
            ReturnClone(wormObj.gameObject);
        }
    }

}
