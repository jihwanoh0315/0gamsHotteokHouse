using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotteok : MonoBehaviour
{
    enum HotteokType
    {
        Base = 0,
        GreenTea,
        SweetPotato
    }

    [SerializeField] HotteokSpriteManager hotteokSpriteManager;
    [SerializeField] HotteokBoardStatus hotteokBoardStatus;
    [SerializeField] GameObject fryEffect;

    [SerializeField] float cookingTime = 0f; // 초기 익은 정도
    [SerializeField] float maxCookingTime = 100f; // 최대 익은 정도
    [SerializeField] float shakeIntensity = 0.2f; // 초기 흔들림 강도

    public float lessMaxPoint = 30.0f;
    public float wellMaxPoint = 70.0f;

    public float lessCookedTime = 5.0f; // 덜 익은 상태까지의 시간
    public float wellCookedTime = 10.0f; // 잘 익은 상태까지의 시간
    public float transitionTime = 3f; // 다음 구간으로 넘어가기 전에 흔들림 시작할 시간

    float pointPerLess = 0.0f;
    float pointPerWell = 0.0f;

    float forwardFace = 0.0f;
    float backFace = 0.0f;

    [SerializeField] float burningMaxTime = 5.0f;
    float burningTimer = 0.0f;

    public bool isBurned = false;
    public int toast = 0; // 0 raw 1 welldone
    public int status = 0; // 0 1 ~ 5
    HotteokType type = HotteokType.Base;
    public int cost;

    // Start is called before the first frame update
    void Start()
    {
        hotteokSpriteManager = FindAnyObjectByType<HotteokSpriteManager>();
        hotteokBoardStatus = FindAnyObjectByType<HotteokBoardStatus>();

        pointPerLess = lessMaxPoint / lessCookedTime;
        pointPerWell = wellMaxPoint / wellCookedTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBurned)
            return;

        // 판의 온도에 따라 호떡이 타는 로직 추가
        if (hotteokBoardStatus.oilTemprature > 0.9f
            || burningTimer > burningMaxTime) // 판이 너무 뜨거우면
        {
            // 빵이 즉시 타버리도록 설정
            SetBurned();
        }
        else if(hotteokBoardStatus.oilTemprature > 0.7f)
        {
            if(toast == 0)
            {
                burningTimer += Time.deltaTime;
            }
            else
            {
                burningTimer += Time.deltaTime * 2.0f;
            }
        }


        // 호떡 굽기
        if (toast == 0)//raw
        {
            cookingTime += pointPerLess * Time.deltaTime;
            if (cookingTime > lessMaxPoint)
            {
                SetWellDone();
            }
        }
        else if (toast == 1) // welldone
        {
            cookingTime += pointPerWell * Time.deltaTime;
            if (cookingTime > wellMaxPoint)
            {
                SetBurned();
            }
        }

    }

    void SetWellDone()
    {
        toast = 1;
        // set welldone texture

    }

    void SetBurned()
    {
        isBurned = true;
        // set burning texture
    }

    void isClicked()
    {
        if(status == 2 || status == 3)
        {
            backFace = cookingTime;
        }
        else
        {
            forwardFace = cookingTime;
        }
    }

    void isFlipped()
    {
        if (status == 2 || status == 3)
        {
            cookingTime = backFace;
            // Set texture to 4
        }
        else if(status == 4)
        {
            cookingTime = forwardFace;
            // Set texture to 3
        }
    }

    void isPressed()
    {
        status = 3;
        // play animation
        // Change texture to 3
    }

}
