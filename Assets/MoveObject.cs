using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveObject : MonoBehaviour
{
    //public bool isLoop = false;
    public bool toLeft = false;
    public float delay = 0.0f;
    public float moveSpeed = 1.0f;
    public float currTime = 0.0f;
    public float leftLimit = -30.0f;
    public float rightLimit = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        if(moveSpeed == 0.0f )
        {
            moveSpeed = 1.0f;
        }

        if (toLeft)
            moveSpeed = -1.0f * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(currTime < delay)
        {
            currTime += Time.deltaTime;
            return;
        }
        transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);

        if (toLeft)
        {
            if (transform.position.x < leftLimit)
            {
                Vector3 currPos = transform.position;
                transform.position = new Vector3(rightLimit + 5.0f, currPos.y, currPos.z);
                currTime = 0.0f;
            }
        }
        else
        {
            if (transform.position.x > rightLimit)
            {
                Vector3 currPos = transform.position;
                transform.position = new Vector3(leftLimit - 5.0f, currPos.y, currPos.z);
                currTime = 0.0f;
            }
        }
    }
}
