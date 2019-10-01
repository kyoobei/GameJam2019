using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    [SerializeField]
    Vector2 throwForce;

    Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    BoxCollider2D wormCollider;

    GameObject targetObject;
   

    bool isActive;
    public bool isEatingCenter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wormCollider = GetComponent<BoxCollider2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        wormCollider.enabled = false;
    }

    /// <summary>
    /// Call this function to initialize worm
    /// </summary>
    public void InitializeWorm()
    {
        //reset rigidbody components
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        //reset collider
        wormCollider.offset = Vector2.zero;
        wormCollider.enabled = true;

        targetObject = null;
        isEatingCenter = false;
        spriteRenderer.sortingOrder = 2;
        isActive = true;

        WormController.Instance.ReleaseAWorm -= FireWorm;
        WormController.Instance.EatCenter -= EatCenter;

        WormController.Instance.ReleaseAWorm += FireWorm;
        WormController.Instance.EatCenter += EatCenter;
    }
    /// <summary>
    /// Call this function to fire worm
    /// </summary>
    public void FireWorm()
    {
        rb.gravityScale = 1;
        rb.AddForce(throwForce, ForceMode2D.Impulse);
    }

    public void EatCenter()
    {
        isEatingCenter = true;
        
    }
    void Update()
    {
        if(isEatingCenter)
        {
            spriteRenderer.sortingOrder = 0;
            float distanceToCenter = Vector2.Distance(transform.position, targetObject.transform.position);
            if(distanceToCenter > 0.1f)
            {
                transform.position = Vector2.MoveTowards
                    (
                        transform.position,
                        targetObject.transform.position,
                        2f * Time.deltaTime
                    );
            }
            else if(distanceToCenter <= 0.5f)
            {
                gameObject.SetActive(false);
                isEatingCenter = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive)
            return;
        isActive = false;

        if (collision.collider.tag == "CircleHit")
        {
            //stop the knife
            rb.velocity = new Vector2(0, 0);
            //this will automatically inherit rotation of the new parent (log)
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.SetParent(collision.collider.transform);

            targetObject = collision.gameObject;

            //move the collider away from the blade which is stuck in the log
            //wormCollider.offset = new Vector2(wormCollider.offset.x, -0.4f);
            //wormCollider.size = new Vector2(wormCollider.size.x, 4f);

            //Spawn another knife
            //GameController.Instance.OnSuccessfullyKnifeHit();

            //WormPooler.Instance.SpawnWorm();
            WormController.Instance.OnWormHitSuccessfully();

        }
        else if (collision.collider.tag == "Worm")
        {
            
            //start rapidly moving downwards
            rb.velocity = new Vector2(rb.velocity.x, -2);
            // TODO: Game over
            //GameController.Instance.StartGameOverSequence(false);
            wormCollider.enabled = false;
            //WormPooler.Instance.SpawnWorm();
            //WormController.Instance.OnWormHit();
            WormController.Instance.OnWormHitFailed();
        }
        
    }
}
