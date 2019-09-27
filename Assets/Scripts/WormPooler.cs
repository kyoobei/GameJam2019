using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPooler : MonoBehaviour
{
    public static WormPooler Instance { get; private set; }
    public int numOfWorms;
    public List<Worm> listOfWorms;
    public ClonePooler clonePooler;
    public Vector2 wormSpawnPosition;
    public Vector2 poolPosition;

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        clonePooler = GetComponent<ClonePooler>();
        for (int i = 0; i < numOfWorms; i++)
        {
            listOfWorms.Add(Instantiate(clonePooler.originalObject, poolPosition, Quaternion.identity).GetComponent<Worm>());
            listOfWorms[i].gameObject.SetActive(false);
            //listOfWorms[i].wormCollider.enabled = false;
        }

        clonePooler.numberOfCopy = 5;
        Debug.Log(clonePooler.numberOfCopy);
        for(int i = 0; i < clonePooler.numberOfCopy; i++)
        {
            clonePooler.QueueClone(listOfWorms[i].gameObject);
        }

        SpawnWorm();
	}

    private void Update()
    {
        //check if any worms are below the screen then return them to the queue
        foreach(Worm _worm in listOfWorms)
        {
            if(_worm.isActiveAndEnabled)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(_worm.transform.position);
                if(pos.y < 0.0f) // object is below camera view
                {
                    Debug.Log("HELLO THERE");
                    clonePooler.ReturnClone(_worm.gameObject);
                    _worm.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SpawnWorm()
    {
        GameObject obj = clonePooler.AcquireClone();
        Worm newWorm = obj.GetComponent<Worm>();
        newWorm.transform.position = wormSpawnPosition;
        newWorm.wormCollider.enabled = true;
        newWorm.transform.rotation = Quaternion.identity;
        newWorm.rb.velocity = new Vector2(0f, 0f);
        newWorm.rb.gravityScale = 0f;
        newWorm.isActive = true;
        if(!listOfWorms.Contains(newWorm))
        {
            listOfWorms.Add(newWorm);
            //Debug.Log("THIS IS A NEWLY CREATED WORM");
        }
    }

}
