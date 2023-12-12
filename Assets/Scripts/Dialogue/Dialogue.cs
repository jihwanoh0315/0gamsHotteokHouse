using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("대사 치는 캐릭터 이름")]
    public string name;

    [Tooltip("대사 치는 캐릭터 이름")]
    public int ID;

    [Tooltip("대사 내용")]
    public string[] contexts;

    [Tooltip("대사 캐릭터 위치")]
    public bool isLeft;

    [Tooltip("대사 초상화 표정")]
    //////////////////////////////////
    /// 0. 일반
    ///  1. 슬픔
    ///  2. 웃음
    ///  3.놀람
    ///  4. 화남
    public int portrait;

    [Tooltip("딜레이")]
    public float delay;

    [Tooltip("대사 속도")]
    public float speechSpeed;
}

[System.Serializable]
public class DialogueEvent
{
    // 이벤트 이름(구분용)
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;
}
