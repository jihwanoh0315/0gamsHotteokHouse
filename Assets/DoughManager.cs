using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughManager : MonoBehaviour
{
    public float timeLimit = 60.0f;
    //[SerializeField] GameObject go_hand;
    [SerializeField] DoughHand handInfo;
    [SerializeField] GameObject smallDough;
    [SerializeField] float spawnTime;

    [SerializeField] GameObject largeDough;
    [SerializeField] Color smallDoughColor;
    float currTime;
    public bool canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit -= Time.deltaTime;
        SpawnSmallDough();
        if (timeLimit < 0 )
        {
            Debug.Log(handInfo.CollideCount);
        }

        if(handInfo.CollideCount > 1200)
        {

        }
    }

    void SpawnSmallDough()
    {
        if(canSpawn)
        {
            currTime += Time.deltaTime;

            if (currTime > spawnTime)
            {
                smallDough.transform.localPosition = new Vector3( Random.Range(-1.8f, 1.8f), Random.Range(-1.8f, 1.8f), -1.0f);
                smallDough.GetComponent<SpriteRenderer>().color = Color.red;
                canSpawn = false;
                currTime = 0.0f;

                foreach (var sj in smallDough.GetComponentsInChildren<SpringJoint2D>())
                {
                    sj.autoConfigureConnectedAnchor = true;
                    sj.autoConfigureConnectedAnchor = false;
                }
                smallDough.SetActive(true);
            }
        }

    }
}
