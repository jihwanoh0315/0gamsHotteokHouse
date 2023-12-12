using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDoughController : MonoBehaviour
{
    [SerializeField] DoughManager doughManager;
    // Start is called before the first frame update
    private void Start()
    {
        doughManager = FindObjectOfType<DoughManager>();
    }
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponentInParent<SpriteRenderer>().color.a <= 0.1f)
        {
            doughManager.canSpawn = true;
            gameObject.SetActive(false);
        }
    }
}
