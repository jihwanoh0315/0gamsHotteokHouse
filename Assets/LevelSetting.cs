using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelSetting : MonoBehaviour
{
    static public LevelSetting instance;

    public GameData m_gameData;
    public bool eventHappened;

    private void Start()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [ContextMenu("To Json Data")]
    void SaveDataToJson()
    {
        string jsonData = JsonUtility.ToJson(m_gameData, true);
        string path = Path.Combine(Application.dataPath, "GameData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    void LoadDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "GameData.json");
        string jsonData = File.ReadAllText(path);
        m_gameData = JsonUtility.FromJson<GameData>(jsonData);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class GameData
{
    /*****************************************
     * name: activeFoods
     * role: 현재 파는물건
     *          호떡
     *          0. 녹차호떡
     *          1. 고구마호떡
     *          사이드
     *          2. 식혜
     *          3. 슬러시
     *          4. 어묵
     * ****************************************/
    public bool[] activeFoods;

    /*****************************************
 * name: activeFoods
 * role: 반죽점수
 *          호떡
 *          0. 일반
 *          1.녹차호떡
 *          2. 고구마호떡
 * ****************************************/
    public float[] doughScore;
    /*****************************************
     * name: currDate
     * role: 
     *          날짜 몇일인지 표시용
     * ****************************************/
    public int currDate;
    /*****************************************
     * name: currMission
     * role: 
     *          카오닝 멤버들의 미션 몇개 깼는지
     *          0. 	호우 출연 -> 튜토리얼
     *          1. 상점으로 가서 카멤 1 등장
     *          2. 카멤2 지나가면서 옷집설명
     *          3. 카멤3(별찌) 등장 피버 설명
     *          4. 카멤1 재등장해서 가게로 오세요
     *          5. 카멤4 -> 1차건물 업글
     *          6. 카멤 5-> 사이드디시 식혜
     *          7. 카멤 6-> 사이드디시 슬러쉬
     *          8. 카멤 7 -> 알바 고용소
     *          9. 카멤 8 -> 홍보가게
     *          10. 카멤 9 -> 사이드디시 어묵
     *          11. 카멤 10 -> 2차진화
     *          12. 카멤 11 -> 인테리어가게
     *          13. 카온님 닝요님의 축구 참여 - > 엔딩          
     * ****************************************/
    public int currMission;

    public bool isWorking;
    /*****************************************
     * name: currBuilding
     * role: 
     *          0 = just begin
     *          1 = 포장마차
     *          2 = 작은 건물
     *          3 = 빌딩
     * ****************************************/
    public int currBuilding;
    /*****************************************
     * name: currMoney
     * role: 
     *          보유 금액
     * ****************************************/
    public int currMoney;
    /*****************************************
     * name: items
     * role: 아이템갯수
     *          반죽
     *          0. 보유 녹차가루
     *          1. 보유 고구마가루
     *          토핑
     *          2. 보유 치즈갯수
     *          3. 보유 고구마갯수
     *          사이드
     *          식혜)
     *          4.보유 식혜병
     *          슬러시)
     *          5. 보유 얼음갯수
     *          6. 보유 음료수 병 갯수
     *          어묵)
     *          7. 보유 어묵 갯수
     *          8. 보유 떡 갯수
     * ****************************************/
    public int[] items; // 각각 아이템들 갯수
    /*****************************************
     * name: levels
     * role: 
     *          0. 반죽레벨
     *          1. 설탕레벨
     *          2. 식혜레벨
     *          3.녹차가루 레벨
     *          4. 고구마가루 레벨
     *          5. 씨앗설탕 레벨
     *          6. 치즈 레벨
     *          7. 슬러시레벨
     *          8. 어묵레벨
     * ****************************************/
    public int[] levels; // 업그레이드 레벨
    /*****************************************
     * name: openedBuilding
     * role: 
     *          0 = 안 열림
     *          1 = 재료가게
     *          2 = 옷가게
     *          3 = 인테리어가게
     *          4 = 홍보가게
     *          5 = 알바가게
     * ****************************************/
    public int openedBuilding;
    /*****************************************
     * name: highestSale
     * role: 
     *          최고매출
     * ****************************************/
    public int highestSale;

    /*****************************************
     * name: clear
     * role: 
     *          깼나
     * ****************************************/
    public bool clear;
}
