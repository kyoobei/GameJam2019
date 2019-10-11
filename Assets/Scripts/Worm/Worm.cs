using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    [SerializeField] Vector2 throwForce;
    [SerializeField] SpriteRenderer spriteRenderer;

    Rigidbody2D rb;
    BoxCollider2D wormCollider;
    GameObject targetObject;

    [SerializeField] bool isActive;
    public bool isEatingCenter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wormCollider = GetComponent<BoxCollider2D>();
        wormCollider.enabled = false;
    }

    /// <summary>
    /// Call this function to initialize worm
    /// </summary>
    public void InitializeWorm()
    {
        StopCoroutine("RotateSnake");
        transform.rotation = Quaternion.identity;
        //reset rigidbody components
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.sharedMaterial = null;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

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
        rb.bodyType = RigidbodyType2D.Dynamic;
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
                //StopCoroutine("RotateSnake");
                isEatingCenter = false;
            }
        }
    }
    IEnumerator RotateSnake()
    {
        while(true)
        {
            transform.Rotate
            (
                new Vector3
                (
                    0f,
                    0f,
                    400f * Time.deltaTime
                )
            );
            yield return null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive)
            return;
        isActive = false;

        if (collision.collider.tag == "CircleHit")
        {
            WormController.Instance.ReleaseAWorm -= FireWorm;
            //stop the knife
            rb.velocity = new Vector2(0, 0);
            //this will automatically inherit rotation of the new parent (log)
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.SetParent(collision.collider.transform);

            targetObject = collision.gameObject;

            WormController.Instance.OnWormHitSuccessfully();
        }
        else if (collision.collider.tag == "Worm")
        {
            WormController.Instance.ReleaseAWorm -= FireWorm;

            wormCollider.enabled = false;
            rb.velocity = Vector2.zero;
            StartCoroutine("RotateSnake");

            WormController.Instance.OnWormHitFailed();

            rb.constraints = RigidbodyConstraints2D.None;
        }
        
    }
}
