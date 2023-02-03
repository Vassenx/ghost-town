using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public bool show = false;
    public float shift;
    private bool hide;
    private float fadeTime = 0;
    private float slideTime = 0;
    private float fadeTotalTime = 1.5f;
    private float slideTotalTime = 3f;
    private SpriteRenderer ghostFillMat;
    private SpriteRenderer ghostLineMat;
    public GameObject pass;

    private List<float> fillHide = new List<float> { 0f, -3f };
    private List<float> lineHide = new List<float> { 0f, -10f };

    private List<float> fill = new List<float> { 0f, -5f };
    private List<float> line = new List<float> { 0f, -10f };

    private List<float> fillShow = new List<float> { 0.85f, 50f };
    private List<float> lineShow = new List<float> { 1.5f, 5f };

    void Start()
    {
        ghostFillMat = GetComponent<SpriteRenderer>();
        ghostLineMat = transform.GetChild(0).GetComponent<SpriteRenderer>();

        fillShow[0] = ghostFillMat.material.GetFloat("_Fade");
        fillShow[1] = ghostFillMat.material.GetFloat("_Slide");

        lineShow[0] = ghostLineMat.material.GetFloat("_Fade");
        lineShow[1] = ghostLineMat.material.GetFloat("_Slide");

        fill[0] = fillHide[0];
        fill[1] = fillHide[1];
        line[0] = lineHide[0];
        line[1] = lineHide[1];

        SetGhostFill(fill);
        SetGhostLine(line);

        hide = true;
    }

    void Update()
    {
        if (show)
        {
            if (hide)
            {
                hide = !hide;
            }

            if (fadeTime<= fadeTotalTime)
            {
                if (fadeTime * shift <= fadeTotalTime)
                {
                    fill[0] = Mathf.Lerp(fillHide[0], fillShow[0], (fadeTime * shift) / fadeTotalTime);
                }

                line[0] = Mathf.Lerp(lineHide[0], lineShow[0], fadeTime / fadeTotalTime);

                fadeTime += Time.deltaTime;
            }



            if (slideTime <= slideTotalTime)
            {
                if (slideTime * shift <= slideTotalTime)
                {
                    fill[1] = Mathf.Lerp(fillHide[1], fillShow[1], (slideTime * shift) / slideTotalTime);
                }

                line[1] = Mathf.Lerp(lineHide[1], lineShow[1], slideTime / slideTotalTime);

                slideTime += Time.deltaTime;
            }

            SetGhostFill(fill);
            SetGhostLine(line);
        }
        else
        {
            if (!hide)
            {
                Debug.Log("hide");

                fill[0] = fillHide[0];
                fill[1] = fillHide[1];
                line[0] = lineHide[0];
                line[1] = lineHide[1];

                SetGhostFill(fill);
                SetGhostLine(line);

                fadeTime = 0;
                slideTime = 0;

                hide = true;
            }
        }
    }

    public void showGhost()
    {
        show = true;
        if (pass != null)
        {
            pass.GetComponent<FadeController>().showGhost();
        }
    }

    void SetGhostFill(List<float> fill)
    {
        //Debug.Log("Set Fill to" + fill[0] + fill[1]);

        ghostFillMat.material.SetFloat("_Fade", fill[0]);
        ghostFillMat.material.SetFloat("_Slide", fill[1]);
    }
    void SetGhostLine(List<float> fill)
    {
        //Debug.Log("Set Line to" + fill[0] + fill[1]);
        ghostLineMat.material.SetFloat("_Fade", fill[0]);
        ghostLineMat.material.SetFloat("_Slide", fill[1]);
    }
}
