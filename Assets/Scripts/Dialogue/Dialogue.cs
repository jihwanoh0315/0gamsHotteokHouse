using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("��� ġ�� ĳ���� �̸�")]
    public string name;

    [Tooltip("��� ġ�� ĳ���� �̸�")]
    public int ID;

    [Tooltip("��� ����")]
    public string[] contexts;

    [Tooltip("��� ĳ���� ��ġ")]
    public bool isLeft;

    [Tooltip("��� �ʻ�ȭ ǥ��")]
    //////////////////////////////////
    /// 0. �Ϲ�
    ///  1. ����
    ///  2. ����
    ///  3.���
    ///  4. ȭ��
    public int portrait;

    [Tooltip("������")]
    public float delay;

    [Tooltip("��� �ӵ�")]
    public float speechSpeed;
}

[System.Serializable]
public class DialogueEvent
{
    // �̺�Ʈ �̸�(���п�)
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;
}
