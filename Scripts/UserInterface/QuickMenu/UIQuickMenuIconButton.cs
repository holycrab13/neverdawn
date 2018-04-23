using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQuickMenuIconButton : UIIconButton
{
    //public float[] angles;
    //public float radius;
    //public float size;

    public Text amount;
    //private float targetAngle;
    //private float angle;



    //// Update is called once per frame
    //void Update()
    //{

    //    if (angle != targetAngle)
    //    {
    //        angle = targetAngle;

    //        updateScaleAndPosition(angle);
    //        updateGlow(angle);
    //        updatePosition(angle);
    //    }
    //}



    //private void updateGlow(float angle)
    //{
    //    float scale = Mathf.Clamp01(1.0f - angle);

    //    color.a = scale;
    //    glow.color = color;
    //}

    //internal void SetTargetAngle(float angle)
    //{
    //    this.targetAngle -= angle;
    //}

    //internal void SetPosition(float angle)
    //{
    //    this.targetAngle = angle;
    //    this.angle = angle;

    //    updateScaleAndPosition(angle);
    //    updateGlow(angle);
    //}

    //private void updateScaleAndPosition(float angle)
    //{
    //    int start = (int)angle;
    //    int end = (int)angle + 1;
    //    float lerp = angle - start;

    //    float worldAngle = Mathf.Lerp(angles[start], angles[end], lerp);

    //    float scale = Mathf.Clamp(1f - 0.1f * angle, 0.5f, 1f);
    //    transform.localScale = new Vector3(scale, scale);

    //    float selectionOffset = Mathf.Clamp01(1.0f - angle) * 10.0f;

    //    transform.localPosition = Quaternion.Euler(0.0f, 0.0f, worldAngle) * (Vector3.up * (radius + size * scale + selectionOffset));
    //}

}
