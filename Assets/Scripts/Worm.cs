using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{

    [SerializeField]
    private Vector2 throwForce;

    public bool isActive = true;
    public Rigidbody2D rb;
    public BoxCollider2D wormCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wormCollider = GetComponent<BoxCollider2D>();
        wormCollider.enabled = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            //throw the knife
            //wormCollider.enabled = true;
            rb.AddForce(throwForce, ForceMode2D.Impulse);
            rb.gravityScale = 1;

            //GameController.Instance.GameUI.DecrementDisplayedWormCount();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive)
            return;
        isActive = false;

        if (collision.collider.tag == "CircleHit")
        {

            //GetComponent<ParticleSystem>().Play();
            //stop the knife
            rb.velocity = new Vector2(0, 0);
            //this will automatically inherit rotation of the new parent (log)
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.SetParent(collision.collider.transform);

            //move the collider away from the blade which is stuck in the log
            wormCollider.offset = new Vector2(wormCollider.offset.x, -0.4f);
            wormCollider.size = new Vector2(wormCollider.size.x, 4f);

            //Spawn another knife
            //GameController.Instance.OnSuccessfullyKnifeHit();

            WormPooler.Instance.SpawnWorm();

        }
        else if (collision.collider.tag == "Worm")
        {
            
            //start rapidly moving downwards
            rb.velocity = new Vector2(rb.velocity.x, -2);
            // TODO: Game over
            //GameController.Instance.StartGameOverSequence(false);
            wormCollider.enabled = false;
            WormPooler.Instance.SpawnWorm();
        }
        
    }
}
