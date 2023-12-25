using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughHand : MonoBehaviour
{
    public int CollideCount = 0;
    CircleCollider2D circleCollider;
    [SerializeField] GameObject doughBackground;
    // Start is called before the first frame update
    void Start()
    {
        CollideCount = 0;
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            ++CollideCount;
            circleCollider.radius -= 0.001f;
        }
        else if (collision.gameObject.layer == 12)
        {
            ++CollideCount;
            circleCollider.radius -= 0.001f;

            Color currCol = collision.gameObject.GetComponentInParent<SpriteRenderer>().color;
            collision.gameObject.GetComponentInParent<SpriteRenderer>().color 
                = new Color(currCol.r, currCol.g, currCol.b, currCol.a - 0.005f);
            currCol = collision.gameObject.GetComponentInParent<SpriteRenderer>().color;
        }
    }
}
