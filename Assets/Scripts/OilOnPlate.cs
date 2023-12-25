using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilOnPlate : MonoBehaviour
{
    GamePlayManager gamePlayManager;
    SpriteRenderer spriteRenderer;
    Animator animator;

    bool doDestroy = false;
    // Start is called before the first frame update
    void Start()
    {
        gamePlayManager = FindAnyObjectByType<GamePlayManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        doDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ִϸ��̼� ���� ������ ��������
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ������ �ִϸ��̼� Ŭ���� ���� �ִϸ��̼ǰ� ��ġ�ϰ�, ������ �ƿ� �����ߴ��� Ȯ��
        if (stateInfo.normalizedTime >= 1.0f && doDestroy == false)
        {
            doDestroy = true;
            spriteRenderer.DOFade(0.0f, 2.0f);
            Destroy(gameObject, 3.0f);
            --gamePlayManager.oilCount;
        }
    }
}
