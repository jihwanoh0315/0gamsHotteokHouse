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
     * role: ���� �Ĵ¹���
     *          ȣ��
     *          0. ����ȣ��
     *          1. ����ȣ��
     *          ���̵�
     *          2. ����
     *          3. ������
     *          4. �
     * ****************************************/
    public bool[] activeFoods;

    /*****************************************
 * name: activeFoods
 * role: ��������
 *          ȣ��
 *          0. �Ϲ�
 *          1.����ȣ��
 *          2. ����ȣ��
 * ****************************************/
    public float[] doughScore;
    /*****************************************
     * name: currDate
     * role: 
     *          ��¥ �������� ǥ�ÿ�
     * ****************************************/
    public int currDate;
    /*****************************************
     * name: currMission
     * role: 
     *          ī���� ������� �̼� � ������
     *          0. 	ȣ�� �⿬ -> Ʃ�丮��
     *          1. �������� ���� ī�� 1 ����
     *          2. ī��2 �������鼭 ��������
     *          3. ī��3(����) ���� �ǹ� ����
     *          4. ī��1 ������ؼ� ���Է� ������
     *          5. ī��4 -> 1���ǹ� ����
     *          6. ī�� 5-> ���̵��� ����
     *          7. ī�� 6-> ���̵��� ������
     *          8. ī�� 7 -> �˹� ����
     *          9. ī�� 8 -> ȫ������
     *          10. ī�� 9 -> ���̵��� �
     *          11. ī�� 10 -> 2����ȭ
     *          12. ī�� 11 -> ���׸����
     *          13. ī�´� �׿���� �౸ ���� - > ����          
     * ****************************************/
    public int currMission;

    public bool isWorking;
    /*****************************************
     * name: currBuilding
     * role: 
     *          0 = just begin
     *          1 = ���帶��
     *          2 = ���� �ǹ�
     *          3 = ����
     * ****************************************/
    public int currBuilding;
    /*****************************************
     * name: currMoney
     * role: 
     *          ���� �ݾ�
     * ****************************************/
    public int currMoney;
    /*****************************************
     * name: items
     * role: �����۰���
     *          ����
     *          0. ���� ��������
     *          1. ���� ��������
     *          ����
     *          2. ���� ġ���
     *          3. ���� ��������
     *          ���̵�
     *          ����)
     *          4.���� ������
     *          ������)
     *          5. ���� ��������
     *          6. ���� ����� �� ����
     *          �)
     *          7. ���� � ����
     *          8. ���� �� ����
     * ****************************************/
    public int[] items; // ���� �����۵� ����
    /*****************************************
     * name: levels
     * role: 
     *          0. ���׷���
     *          1. ��������
     *          2. ��������
     *          3.�������� ����
     *          4. �������� ����
     *          5. ���Ѽ��� ����
     *          6. ġ�� ����
     *          7. �����÷���
     *          8. �����
     * ****************************************/
    public int[] levels; // ���׷��̵� ����
    /*****************************************
     * name: openedBuilding
     * role: 
     *          0 = �� ����
     *          1 = ��ᰡ��
     *          2 = �ʰ���
     *          3 = ���׸����
     *          4 = ȫ������
     *          5 = �˹ٰ���
     * ****************************************/
    public int openedBuilding;
    /*****************************************
     * name: highestSale
     * role: 
     *          �ְ����
     * ****************************************/
    public int highestSale;

    /*****************************************
     * name: clear
     * role: 
     *          ����
     * ****************************************/
    public bool clear;
}
