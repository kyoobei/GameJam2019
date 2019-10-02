using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPooler : ClonePooler
{
    const float WORM_POSITION_X = 0.5f;
    const float WORM_POSITION_Y = 0.15f;
    [SerializeField] List<Worm> listOfReleasedWorms = new List<Worm>();



    void Update()
    {
        if (listOfReleasedWorms.Count <= 0)
            return;

        CheckStatusOfReleasedWorms();
    }
    void CheckStatusOfReleasedWorms()
    {
        for (int i = 0; i < listOfReleasedWorms.Count; i++)
        {
            if (listOfReleasedWorms[i].gameObject.activeInHierarchy)
            {
                if (IsCurrentWormOutOfBounds(listOfReleasedWorms[i].gameObject))
                {
                    //deactivate the worm manually
                    listOfReleasedWorms[i].gameObject.SetActive(false);
                    return;
                }
            }
            else if (!listOfReleasedWorms[i].gameObject.activeInHierarchy)
            {
                //return to clone
                ReturnClone(listOfReleasedWorms[i].gameObject);
                //remove from current list of worm
                listOfReleasedWorms.Remove(listOfReleasedWorms[i]);

                return;
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
    /// Deactivate All Worm
    /// </summary>
    public void ReturnAllWorm()
    {
        for(int i = 0; i < listOfReleasedWorms.Count; i++)
        {
            ReturnClone(listOfReleasedWorms[i].gameObject);
        }
        listOfReleasedWorms.Clear();
    }

}
