using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDoughController : MonoBehaviour
{
    [SerializeField] DoughManager doughManager;
    float currTime = 0.0f;
    float destroyTime;

    // Start is called before the first frame update
    private void Start()
    {
        doughManager = FindObjectOfType<DoughManager>();
        destroyTime = 5.0f;
    }
    void Awake()
    {
        currTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(GetComponentInParent<SpriteRenderer>().color.a <= 0.1f
            || destroyTime < currTime)
        {
            doughManager.canSpawn = true;
            gameObject.SetActive(false);
        }
    }
}
