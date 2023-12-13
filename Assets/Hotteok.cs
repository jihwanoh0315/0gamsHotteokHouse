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

    [SerializeField] float cookingTime = 0f; // �ʱ� ���� ����
    [SerializeField] float maxCookingTime = 100f; // �ִ� ���� ����
    [SerializeField] float shakeIntensity = 0.2f; // �ʱ� ��鸲 ����

    public float lessMaxPoint = 30.0f;
    public float wellMaxPoint = 70.0f;

    public float lessCookedTime = 5.0f; // �� ���� ���±����� �ð�
    public float wellCookedTime = 10.0f; // �� ���� ���±����� �ð�
    public float transitionTime = 3f; // ���� �������� �Ѿ�� ���� ��鸲 ������ �ð�

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

        // ���� �µ��� ���� ȣ���� Ÿ�� ���� �߰�
        if (hotteokBoardStatus.oilTemprature > 0.9f
            || burningTimer > burningMaxTime) // ���� �ʹ� �߰ſ��
        {
            // ���� ��� Ÿ�������� ����
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


        // ȣ�� ����
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
