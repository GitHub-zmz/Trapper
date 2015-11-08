using nityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform carl;

    TweenPosition move;
    void Start()
    {
        transform.position = carl.position;

        move = GetComponent<TweenPosition>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float euler = Mathf.Abs(transform.eulerAngles.y) % 180;
        float dis = 0;
        if (euler > 45)//是侧面
            dis = Vector2.Distance(new Vector2(transform.position.z,transform.position.y),new Vector2(carl.position.z,carl.position.y));
        else//是正面
            dis = Vector2.Distance(transform.position, carl.position);

        if (dis > 0.1f)
        {
            move.from = transform.position;
            if (euler > 45) //是侧面
                move.to = new Vector3(transform.position.x, carl.position.y, carl.position.z);
            else//是正面
                move.to = new Vector3(carl.position.x, carl.position.y , transform.position.z);
            move.ResetToBeginning();
            move.duration = Mathf.Abs(dis)*3f;
            move.PlayForward();
        }
    }
}
