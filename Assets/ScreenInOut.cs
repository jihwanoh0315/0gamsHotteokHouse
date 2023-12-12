using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for Image

public class ScreenInOut : MonoBehaviour
{
    public Image EntireFader;
    // Diagonal 700
    public Image DiagUp;
    public Image DiagDown;

    // Vertical
    public Image VertLeft;
    public Image VertRight;

    // Horizontal
    public Image HorizUp;
    public Image HorizDown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    HorizCutIn();
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    HorizCutOut();
        //}
    }

    public TweenerCore<Color, Color, ColorOptions> EntireFadeIn(float time_ = 0.5f)
    {
        return EntireFader.DOFade(0, time_);
    }
    public TweenerCore<Color, Color, ColorOptions> EntireFadeOut(float time_ = 0.5f)
    {
        return EntireFader.DOFade(1, time_);
    }
    // CutIO
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutLeftIn(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(-2000.0f, 0.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_);
    }
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutLeftOut(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(-2000.0f, 0.0f, 30.0f), time_);
    }
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutRightIn(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(2000.0f, 0.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_);
    }
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutRightOut(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(2000.0f, 0.0f, 30.0f), time_);
    }
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutDownIn(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(0.0f, -1100.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_);
    }
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutDownOut(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, -1100.0f, 30.0f), time_);
    }
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutUpIn(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(0.0f, 1100.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_);
    }
    public TweenerCore<Vector3, Vector3, VectorOptions> EntireCutUpOut(float time_ = 0.5f)
    {
        EntireFader.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);
        return EntireFader.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 1100.0f, 30.0f), time_);
    }




    // Diag
    public Sequence DiagonalCutIn(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();

        DiagUp.gameObject.transform.localPosition = new Vector3(700.0f, 700.0f, 30.0f);
        DiagDown.gameObject.transform.localPosition = new Vector3(-700.0f, -700.0f, 30.0f);

        mySequence.Insert(0.0f, DiagUp.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_,true));
        mySequence.Insert(0.0f, DiagDown.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_,true));

        return mySequence;
    }


    public Sequence DiagonalCutOut(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();
        //DiagUp.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);
        //DiagDown.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);

        mySequence.Insert(0.0f, DiagUp.GetComponent<RectTransform>().DOLocalMove(new Vector3(800.0f, 800.0f, 30.0f), time_, true));
        mySequence.Insert(0.0f, DiagDown.GetComponent<RectTransform>().DOLocalMove(new Vector3(-800.0f, -800.0f, 30.0f), time_, true));
        return mySequence;
    }

    // Horiz
    public Sequence HorizCutIn(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();
        HorizUp.gameObject.transform.localPosition = new Vector3(0.0f, 600.0f, 30.0f);
        HorizDown.gameObject.transform.localPosition = new Vector3(0.0f, -600.0f, 30.0f);

        mySequence.Insert(0.0f, HorizUp.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 450.0f, 30.0f), time_));
        mySequence.Insert(0.0f, HorizDown.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, -450.0f, 30.0f), time_));
        
        return mySequence;
    }
    public Sequence HorizCutOut(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();
        //HorizUp.gameObject.transform.localPosition = new Vector3(0.0f, 450.0f, 30.0f);
        //HorizDown.gameObject.transform.localPosition = new Vector3(0.0f, -450.0f, 30.0f);

        mySequence.Insert(0.0f, HorizUp.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 600.0f, 30.0f), time_));
        mySequence.Insert(0.0f, HorizDown.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, -600.0f, 30.0f), time_));
        
        return mySequence;
    }

    public Sequence HorizOpen(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();
        HorizUp.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);
        HorizDown.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);

        mySequence.Insert(0.0f, HorizUp.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 450.0f, 30.0f), time_));
        mySequence.Insert(0.0f, HorizDown.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, -450.0f, 30.0f), time_));

        return mySequence;
    }

    public Sequence HorizClose(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();
        HorizUp.gameObject.transform.localPosition = new Vector3(0.0f, 450.0f, 30.0f);
        HorizDown.gameObject.transform.localPosition = new Vector3(0.0f, -450.0f, 30.0f);

        mySequence.Insert(0.0f, HorizUp.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0, 30.0f), time_));
        mySequence.Insert(0.0f, HorizDown.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0, 30.0f), time_));

        return mySequence;
    }

    // Vert
    public Sequence VertCutOut(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();
        VertLeft.gameObject.transform.localPosition = new Vector3(-1100.0f, 0.0f, 30.0f);
        VertRight.gameObject.transform.localPosition = new Vector3(1100.0f, 0.0f, 30.0f);

        mySequence.Insert(0.0f, VertLeft.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_));
        mySequence.Insert(0.0f, VertRight.GetComponent<RectTransform>().DOLocalMove(new Vector3(0.0f, 0.0f, 30.0f), time_));
        
        return mySequence;
    }
    public Sequence VertCutIn(float time_ = 1.0f)
    {
        Sequence mySequence = DOTween.Sequence();
        //VertLeft.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);
        //VertRight.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 30.0f);

        mySequence.Insert(0.0f, VertLeft.GetComponent<RectTransform>().DOLocalMove(new Vector3(-1100.0f, 0.0f, 30.0f), time_));
        mySequence.Insert(0.0f, VertRight.GetComponent<RectTransform>().DOLocalMove(new Vector3(1100.0f, 0.0f, 30.0f), time_));
        
        return mySequence;
    }

}
