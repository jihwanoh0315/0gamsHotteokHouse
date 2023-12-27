using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Hotteok : MonoBehaviour
{
    enum HotteokType
    {
        Base = 0,
        GreenTea,
        SweetPotato
    }

    enum HotteokRoast
    {
        RAW = 0,
        WELL,
        BURN
    }

    class Face
    {
        public HotteokRoast roast = HotteokRoast.RAW;
        public float roastAmount = 0.0f;
    }


    [SerializeField] HotteokSpriteManager hotteokSpriteManager;
    [SerializeField] HotteokBoardStatus hotteokBoardStatus;
    [SerializeField] LevelSetting levelSetting;
    [SerializeField] GameObject fryEffect;
    [SerializeField] GameObject toolEffect;

    Dictionary<HotteokRoast, List<Sprite>> thisHotteokSprites = new();
    Dictionary<HotteokRoast, Animator> thisPressAnim = new();
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;
    Vector3 prevPos = Vector3.zero;

    public float lessMaxPoint = 30.0f;
    public float wellMaxPoint = 70.0f;

    public float lessCookedTime = 5.0f; // 덜 익은 상태까지의 시간
    public float wellCookedTime = 10.0f; // 잘 익은 상태까지의 시간
    public float transitionTime = 3f; // 다음 구간으로 넘어가기 전에 흔들림 시작할 시간

    float pointPerLess = 0.0f;
    float pointPerWell = 0.0f;

    float shakeTerm = 1.2f;
    float shakeTimer = 0.0f;

    [SerializeField] float burningMaxTime = 5.0f;
    float burningTimer = 0.0f;
    float plateBurningTimer = 0.0f;
    float holdTimer = 0.0f;

    public int currStatus = 0; // 0 1 ~ 5
    [SerializeField] HotteokType doughType = HotteokType.Base;
    [SerializeField] int currBackSide;// 0 = F, 1 = B
    [SerializeField] Face[] faces;
    [SerializeField] List<Sprite> toolsSprites;
    public int price = 0;
    public int finalStats = 2; //0 raw 1 well 2 great

    public bool isBurned = false;
    public bool isPlaced = false;
    public bool isHeld = false;
    public bool isPressing = false;
    public bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        hotteokSpriteManager = FindAnyObjectByType<HotteokSpriteManager>();
        hotteokBoardStatus = FindAnyObjectByType<HotteokBoardStatus>();
        levelSetting = FindAnyObjectByType<LevelSetting>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();


        pointPerLess = lessMaxPoint / lessCookedTime;
        pointPerWell = wellMaxPoint / wellCookedTime;

        faces = new Face[2];
        faces[0] = new Face();
        faces[1] = new Face();

        if (doughType == HotteokType.Base)
            price = 100;
        else if (doughType == HotteokType.GreenTea)
            price = 120;
        else
            price = 150;

        switch (doughType)
        {
            case HotteokType.Base:
                thisHotteokSprites.Add(HotteokRoast.RAW, hotteokSpriteManager.rawHotteok);
                thisHotteokSprites.Add(HotteokRoast.WELL, hotteokSpriteManager.wellHotteok);
                thisHotteokSprites.Add(HotteokRoast.BURN, hotteokSpriteManager.burnHotteok);
                thisPressAnim.Add(HotteokRoast.RAW, hotteokSpriteManager.pressAnim_Raw);
                thisPressAnim.Add(HotteokRoast.WELL, hotteokSpriteManager.pressAnim);
                thisPressAnim.Add(HotteokRoast.BURN, hotteokSpriteManager.pressAnim_Burn);
                break;
            case HotteokType.GreenTea:
                thisHotteokSprites.Add(HotteokRoast.RAW, hotteokSpriteManager.gr_rawHotteok);
                thisHotteokSprites.Add(HotteokRoast.WELL, hotteokSpriteManager.gr_wellHotteok);
                thisHotteokSprites.Add(HotteokRoast.BURN, hotteokSpriteManager.gr_burnHotteok);

                thisPressAnim.Add(HotteokRoast.RAW, hotteokSpriteManager.gr_pressAnim_Raw);
                thisPressAnim.Add(HotteokRoast.WELL, hotteokSpriteManager.gr_pressAnim);
                thisPressAnim.Add(HotteokRoast.BURN, hotteokSpriteManager.gr_pressAnim_Burn);
                break;
            case HotteokType.SweetPotato:
                thisHotteokSprites.Add(HotteokRoast.RAW, hotteokSpriteManager.sp_rawHotteok);
                thisHotteokSprites.Add(HotteokRoast.WELL, hotteokSpriteManager.sp_wellHotteok);
                thisHotteokSprites.Add(HotteokRoast.BURN, hotteokSpriteManager.sp_burnHotteok);
                thisPressAnim.Add(HotteokRoast.RAW, hotteokSpriteManager.sp_pressAnim_Raw);
                thisPressAnim.Add(HotteokRoast.WELL, hotteokSpriteManager.sp_pressAnim);
                thisPressAnim.Add(HotteokRoast.BURN, hotteokSpriteManager.sp_pressAnim_Burn);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            return;
        }
        if (isPressing)
        {
            // 현재 애니메이션 상태 정보를 가져오기
            AnimatorStateInfo stateInfo = toolEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

            // 지정한 애니메이션 클립이 현재 애니메이션과 일치하고, 마지막 컷에 도달했는지 확인
            if (stateInfo.normalizedTime >= 1.0f && currStatus == 0)
            {
                toolEffect.SetActive(false);
                isPressing = false;
                spriteRenderer.sprite = thisHotteokSprites[faces[0].roast][2];

                currStatus = 2;
                currBackSide = 1;
                ++levelSetting.leftPresser;
                toolEffect.GetComponent<Animator>().runtimeAnimatorController = null;
                fryEffect.GetComponent<Animator>().runtimeAnimatorController = hotteokSpriteManager.anim_oilBig.runtimeAnimatorController;
                levelSetting.handActive = true;
            }
            return;
        }
        if(isMoving)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - transform.localPosition;
            if (Vector2.Distance(transform.position, mousePos) > 0.1f)
            {
                // Rigidbody2D에 힘을 가함
                GetComponent<Rigidbody2D>().AddForce(dir.normalized * 2.0f);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            toolEffect.SetActive(true);
            levelSetting.handActive = false;
        }

        if (isHeld)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(mousePos.x, mousePos.y, this.transform.localPosition.z);
            holdTimer += Time.deltaTime;
            return;
        }
        
        if (isBurned)
        {
            return;
        }

        Face currBackFace = faces[currBackSide];


        // 판의 온도에 따라 호떡이 타는 로직 추가
        if (hotteokBoardStatus.oilTemprature > 0.9f
            || burningTimer > burningMaxTime) // 판이 너무 뜨거우면
        {
            //Debug.Log("SO HOT, SO BURN");

            // 빵이 즉시 타버리도록 설정
            SetBurned();
        }
        else if(hotteokBoardStatus.oilTemprature > 0.8f)
        {
            if(currBackFace.roast == HotteokRoast.RAW)
            {
                isPlateBurning();
            }
            else
            {
                isPlateBurning(2.0f);
            }
        }

        switch (currBackFace.roast)
        {
            case HotteokRoast.RAW:
                currBackFace.roastAmount += pointPerLess * Time.deltaTime * hotteokBoardStatus.toastAmount;
                if (currBackFace.roastAmount > lessMaxPoint)
                {
                    SetWellDone();
                }
                if(currBackFace.roastAmount > 50.0f)
                {
                    if (!isMoving)
                        transform.DOShakePosition(0.1f, new Vector3(0.15f, 0, 0), 3);
                }
                break;
            case HotteokRoast.WELL:

                if (currBackFace.roastAmount > 100.0f)
                {
                    currBackFace.roastAmount = 101.0f;
                    isBurning();

                }
                else
                {
                    currBackFace.roastAmount += pointPerWell * Time.deltaTime * hotteokBoardStatus.toastAmount;
                    if (currBackFace.roastAmount > 70.0f)
                    {
                        shakeTimer += Time.deltaTime;
                        float roastRatio = currBackFace.roastAmount / 100.0f;
                        if (shakeTimer > (shakeTerm - roastRatio) * 2.0f)
                        {
                            shakeTimer = 0;
                            if (!isMoving)
                                transform.DOShakePosition(0.05f, new Vector3(0.2f, 0, 0), 3);
                        }
                    }
                }

                break;
            default:
                break;
        }
        //PrintDebug();

        // Shake Object
    }
    void SetWellDone()
    {
        if(!isMoving)
            transform.DOShakePosition(0.1f, new Vector3(0.15f, 0, 0), 3);
        faces[currBackSide].roast = HotteokRoast.WELL;
        if (currStatus == 0)
        {
            spriteRenderer.sprite = thisHotteokSprites[HotteokRoast.WELL][0];
        }
    }

    void isBurning(float mul_ = 1.0f)
    {
        burningTimer += Time.deltaTime * mul_;

        shakeTimer += Time.deltaTime;
        if (shakeTimer > 0.6f)
        {
            shakeTimer = 0;
            if (!isMoving)
                transform.DOShakePosition(0.05f, new Vector3(0.1f, 0, 0), 3);
        }
    }

    void isPlateBurning(float mul_ = 1.0f)
    {
        plateBurningTimer += Time.deltaTime * mul_;

        shakeTimer += Time.deltaTime;
        if (shakeTimer > 0.6f)
        {
            shakeTimer = 0;
            if (!isMoving)
                transform.DOShakePosition(0.05f, new Vector3(0.1f, 0, 0), 3);
        }
    }


    void SetBurned()
    {
        isBurned = true;
        // set burning texture
        spriteRenderer.color = Color.gray;
        faces[currBackSide].roast = HotteokRoast.BURN;
        fryEffect.GetComponent<SpriteRenderer>().color = Color.black;
        if (currStatus == 0)
        {
            spriteRenderer.sprite = thisHotteokSprites[HotteokRoast.BURN][0];
        }
        ReleaseFace();
    }


    void FlipFace()
    {
        Face currBackFace = faces[currBackSide];

        burningTimer = 0.0f;

        currBackSide = 1 - currBackSide;
        switch (currStatus)
        {
            case 1:
                break;
            case 2:
            case 3:
            case 5:
                currStatus = 4;
                break;
            case 4:
                currStatus = 3;
                if(currBackFace.roast == HotteokRoast.WELL)
                {
                    if(currBackFace.roastAmount < 50.0f)
                    {
                        currStatus = 5;
                    }
                }
                break;
            default:
                break;
        }
        spriteRenderer.sprite = thisHotteokSprites[currBackFace.roast][currStatus];
    }

    public void PressFace()
    {
        if (!levelSetting.handActive)
            return;
        //Debug.Log("Press " + name);
        if(currStatus == 0 && !isBurned)
        {
            if (!isPressing)
            {
                Face currBackFace = faces[currBackSide];

                // play animation
                toolEffect.SetActive(true);
                toolEffect.GetComponent<Animator>().runtimeAnimatorController = thisPressAnim[currBackFace.roast].runtimeAnimatorController;
                if(currBackFace.roast == HotteokRoast.WELL)
                {
                    currBackFace.roastAmount = lessMaxPoint;
                }
                isPressing = true;
                --levelSetting.leftPresser;
                if (levelSetting.leftPresser == 0)
                    levelSetting.handActive = false;
            }
        }
        else
        {
            if (currStatus == 0)
                return;
            toolEffect.SetActive(true);
            toolEffect.GetComponent<SpriteRenderer>().sprite = toolsSprites[2];
            isMoving = true;
            //toolEffect.GetComponent<SpriteRenderer>().sprite = toolImages[0];
        }
    }
    public void ReleaseFace()
    {
        if (!isPressing)
        {
            toolEffect.SetActive(false);
            levelSetting.handActive = true;
            isMoving = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void HoldHotteok()
    {
        //Debug.Log(name + "  = Hold");
        if (!levelSetting.handActive)
            return;
        spriteRenderer.sortingLayerName = "OnTheHand";
        levelSetting.handActive = false;
        if (currStatus == 0 && !isBurned)
        {
            isMoving = true;
            toolEffect.GetComponent<SpriteRenderer>().sprite = toolsSprites[0];
        }
        else
        {
            isHeld = true;
            holdTimer = 0.0f;
            toolEffect.SetActive(true);
            fryEffect.SetActive(false);
            spriteRenderer.sortingOrder = 10;

            //save prevPos <- back here when can't place
            prevPos = transform.position;
            circleCollider.enabled = false;

            if(currStatus == 0)
                toolEffect.GetComponent<SpriteRenderer>().sprite = toolsSprites[0];
            else
                toolEffect.GetComponent<SpriteRenderer>().sprite = toolsSprites[1];
        }
    }

    public bool ReleaseHotteok()
    {
        //Debug.Log(name + "  = Release");

        toolEffect.SetActive(false);
        isHeld = false;
        isMoving = false;
        spriteRenderer.sortingLayerName = "HotteokBoard";

        if (currStatus == 0 && !isBurned)
        {
            toolEffect.SetActive(false);
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool goBack = true;
            if (mousePos.y < 1.0f)
            {
                if ((mousePos.x > -5.0f && mousePos.x < 5.0f))
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(
                    transform.position, circleCollider.radius, (1 << LayerMask.NameToLayer("Hotteok")));
                    if (colliders.Length == 0)
                    {
                        goBack = false;
                    }
                }
                else if (mousePos.x < -6.0f)
                {
                    Destroy(gameObject);
                    return true;
                }
            }

            if (goBack)
            {
                transform.position = prevPos;
            }
            circleCollider.enabled = true;

            fryEffect.SetActive(true);
            spriteRenderer.sortingOrder = 1;

            if(currStatus != 0 && holdTimer < 0.4f)
            {
                FlipFace();
            }
        }

        return false;
    }

    public void AddToRack()
    {
        //Debug.Log(name + "  = Added to Rack");
        isPlaced = true;

        levelSetting.handActive = true;
        toolEffect.SetActive(false);
        isHeld = false;
        isMoving = false;
        hotteokBoardStatus.hotteoks.Remove(this);

        Destroy(gameObject);
    }
    public bool CanRack()
    {
        if (currStatus == 0 || isBurned)
            return false;
        return true;
    }
    public int GetDoughType()
    {
        return ((int)doughType);
    }

    public int GetFrontIndex()
    {
        if (currStatus == 5)
        {
            return 1;
        }

        if (faces[0].roast == HotteokRoast.RAW)
        {
            return 0;
        }
        else
        {
            return 2;
        }
    }


    void PrintDebug()
    {

        string toPrint = "";

        if (isBurned)
        {
            toPrint += "Burned\n";
        }
        if (isHeld)
        {
            toPrint += "Hold\n";
        }

        toPrint += "앞면 : " + faces[0].roastAmount;
        if (faces[0].roast == HotteokRoast.RAW)
        {
            toPrint += " (RAW)";
        }
        else if (faces[0].roast == HotteokRoast.WELL)
        {
            toPrint += " (WELL)";
        }
        else
        {
            toPrint += " (BURN)";
        }

        toPrint += ", 뒷면 : " + faces[1].roastAmount;
        if (faces[1].roast == HotteokRoast.RAW)
        {
            toPrint += " (RAW)";
        }
        else if (faces[1].roast == HotteokRoast.WELL)
        {
            toPrint += " (WELL)";
        }
        else
        {
            toPrint += " (BURN)";
        }

        Debug.Log(toPrint);
    }
    public int GetPrice()
    {
        SetPrice();
        return price;
    }
    public int GetStats()
    {
        return finalStats;
    }
    void SetPrice()
    {
        float currPrice = 1.0f;
        
        // Price for Roast
        for(int i = 0; i < 2; i++)
        {
            float val = faces[i].roastAmount;
            
            if(val < 30.0f)
            {
                finalStats = 0;
                currPrice = 0.3f;
            }
            else if(val < 70.0f)
            {
                if(finalStats > 1)
                {
                    finalStats = 1;
                    currPrice = 0.7f;
                }
            }

        }

        // Upgrade
        float levelCombine = 1.0f;
        int[] levs = levelSetting.m_gameData.levels;
        
        switch (doughType)
        {
            case HotteokType.Base:
                levelCombine += levs[0];
                levelCombine += levs[1];
                break;
            case HotteokType.GreenTea:
                levelCombine += levs[1];
                levelCombine += levs[3];
                levelCombine += levs[5];
                break;
            case HotteokType.SweetPotato:
                levelCombine += levs[1];
                levelCombine += levs[4];
                levelCombine += levs[6];
                break;
            default:
                break;
        }

        // Dough 
        currPrice *= levelSetting.doughScore[(int)doughType];
        currPrice *= levelCombine;

        // Pet effect
        // if(pet == 1)
        //  currPrice *= 1.2f;

        price =  Mathf.RoundToInt(((float)price * currPrice));
    }

    public void IsFeverTimeNow()
    {
        if (isBurned)
        {
            Destroy(this);
        }

        for(int i = 0; i < 2; ++i)
        {
            faces[i].roast = HotteokRoast.WELL;
            faces[i].roastAmount = 70.0f;
        }

        if(currBackSide == 0)
        {
            spriteRenderer.sprite = thisHotteokSprites[HotteokRoast.WELL][4];
        }
        else
        {
            spriteRenderer.sprite = thisHotteokSprites[HotteokRoast.WELL][3];
        }
    }
}
