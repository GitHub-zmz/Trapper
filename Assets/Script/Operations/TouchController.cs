using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour
{
    public Transform carlTran, faceMeTran;
    float screenCenterX;
    TweenRotation faceRot, carlRot;
    public static bool isTurning;
    Carl carl;
    void Start()
    {
        screenCenterX = Screen.width / 2;
        faceRot = faceMeTran.GetComponent<TweenRotation>();
        carlRot = carlTran.GetComponent<TweenRotation>();
        isTurning = false;
        faceRot.SetOnFinished(delegate()
        {
            isTurning = false;
        });
        carl = carlTran.GetComponent<Carl>();
    }

    Vector2 touchBeginPos;
    int touchPause;
    bool slide, touchStart;

    void OnMouseDown()
    {
        touchBeginPos = Input.mousePosition;
        touchPause = 0;
        touchStart = true;
    }
    void OnMouseOver()
    {
        if (touchStart)
        {
            if (touchPause++ < 1)
                return;
            Vector2 touchEndPos = Input.mousePosition;
            if (Vector2.Distance(touchBeginPos, touchEndPos) > 3)//3待标准化
            {//说明滑动了
                if (!slide)
                {
                    slide = true;
                    float offX = touchEndPos.x - touchBeginPos.x,
                        offY = touchEndPos.y - touchBeginPos.y;
                    if (Mathf.Abs(offX) > Mathf.Abs(offY) && !isTurning)
                    {//左右划
                        isTurning = true;
                        if (offX > 0)
                            turn(false);
                        else
                            turn(true);
                    }
                    else
                    {//上下划
                        if (offY > 0)
                            carl.jump(true);
                        else
                            carl.jump(false);
                    }
                }
            }
            else if(!slide)
            {
                if (touchPause++ < 6)
                    return;
                if (touchEndPos.x > screenCenterX)
                {
                    carl.move(true);
                }
                else
                {
                    carl.move(false);
                }
            }
        }
    }
    void OnMouseUp()
    {
        touchStart = false;
        slide = false;
    }
    void turn(bool left)
    {
        faceRot.from = faceMeTran.localRotation.eulerAngles;
        faceRot.to = new Vector3(faceRot.from.x, faceRot.from.y + (left ? -90 : 90), faceRot.from.z);
        faceRot.ResetToBeginning();
        faceRot.PlayForward();

        carlRot.from = carlTran.localRotation.eulerAngles;
        carlRot.to = new Vector3(carlRot.from.x, carlRot.from.y + (left ? -90 : 90), carlRot.from.z);
        carlRot.ResetToBeginning();
        carlRot.PlayForward();
    }
}
