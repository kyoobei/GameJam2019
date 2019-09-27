using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePooler : MonoBehaviour {
    
    public int numberOfCopy;
    private Queue<GameObject> queuedClones = new Queue<GameObject>();
    public GameObject originalObject;
    [Tooltip("Possible parent of cloned objects")]
    public Transform possibleCloneParent;
    private void Start()
    {
        for (int i = 0; i < numberOfCopy; i++) {
            GameObject clone = Instantiate(originalObject) as GameObject;
            //clone.GetComponent<Clone>().originPooler = this;
            if (possibleCloneParent != null)
                clone.transform.parent = possibleCloneParent;
            queuedClones.Enqueue(clone);
            clone.SetActive(false);
        }
        numberOfCopy = queuedClones.Count;
    }
    
    public void QueueClone(GameObject clone)
    {
        queuedClones.Enqueue(clone);
        clone.SetActive(false);
    }

    public GameObject AcquireClone()
    {
        GameObject clone = null;
        if (queuedClones.Count > 0)
        {
            clone = queuedClones.Dequeue();
        }
        else
        {
            GameObject addClone = Instantiate(originalObject) as GameObject;
            //addClone.GetComponent<Clone>().originPooler = this;
            if (possibleCloneParent != null)
                addClone.transform.parent = possibleCloneParent;

            clone = addClone;
        }
        if (clone != null)
            clone.SetActive(true);
        numberOfCopy = queuedClones.Count;
        return clone;
    }
    public void ReturnClone(GameObject clonedObject)
    {
        queuedClones.Enqueue(clonedObject);
        numberOfCopy = queuedClones.Count;
    }
    
}
