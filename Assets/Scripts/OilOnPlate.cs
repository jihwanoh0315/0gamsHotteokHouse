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
        // 현재 애니메이션 상태 정보를 가져오기
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 지정한 애니메이션 클립이 현재 애니메이션과 일치하고, 마지막 컷에 도달했는지 확인
        if (stateInfo.normalizedTime >= 1.0f && doDestroy == false)
        {
            doDestroy = true;
            spriteRenderer.DOFade(0.0f, 2.0f);
            Destroy(gameObject, 3.0f);
            --gamePlayManager.oilCount;
        }
    }
}
