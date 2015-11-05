using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform carl;

    TweenPosition move;
    void Start()
    {
        move = GetComponent<TweenPosition>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if(Vector3.Distance(transform.po))
        move.from = transform.position;
        move.to = new Vector3(transform.position.x, carl.position.y + 1.22f, transform.position.z);
        move.ResetToBeginning();
        move.duration = Mathf.Abs(move.to.y - move.from.y);
        move.PlayForward();
    }
}
