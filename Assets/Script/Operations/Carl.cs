using UnityEngine;
using System.Collections;

public class Carl : MonoBehaviour
{
    TweenPosition[] jumps;
    bool isJumping, onLadder, isClimbing;
    Vector3 jumpHigh = new Vector3(0, 0.5f, 0);
    Animator animator;
    Rigidbody rigid;

    void Start()
    {
        jumps = GetComponents<TweenPosition>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        jumps[0].SetOnFinished(delegate()
        {
            jumps[1].from = transform.position;
            jumps[1].to = transform.position - jumpHigh;
            jumps[1].ResetToBeginning();
            jumps[1].PlayForward();
        });
        jumps[1].SetOnFinished(delegate()
        {
            isJumping = false;
            idle();
        });
        onLadder = isClimbing = false;
    }
    public void move(bool right)
    {
        if (!isClimbing)
        {
            Vector3 scout2 = transform.FindChild("scout2").position;
            if (Physics.Raycast(transform.position, right ? transform.right : -transform.right,
               0.3f, 1 << LayerMask.NameToLayer("Obstacle"))
                //以主角的位置向将要移动的方向发射线，看将要移动的方向是否被东西挡住了
                ||
                !Physics.Raycast(right ? scout2 :
                new Vector3(-scout2.x, scout2.y, scout2.z), Vector3.down,
                0.6f, 1 << LayerMask.NameToLayer("Obstacle"))
                //以右向探子向下发射线，看将要移动的方向是否有路
                )
            {//以脚下探子向“前”发射线，选取距离最远的射点（最前的），将主角移动到这一点
                Vector3 from = transform.FindChild("scout1").position;
                RaycastHit[] hits = Physics.RaycastAll(from, -transform.forward, 50, 1 << LayerMask.NameToLayer("Obstacle"));
                print(hits.Length);
                if (hits.Length != 0)
                {
                    Vector3 to = from;
                    for (int i = 0; i < hits.Length; ++i)
                        if (Vector3.Distance(from, to) < Vector3.Distance(from, hits[i].point))
                            to = hits[i].point;

                    float euler = Mathf.Abs(transform.eulerAngles.y) % 180;
                    if (euler > 45)
                        transform.position = new Vector3(to.x + (right ? 0.2f : -0.2f), transform.position.y, to.z);
                    else
                        transform.position = new Vector3(to.x, transform.position.y, to.z + (right ? 0.2f : -0.2f));
                }
            }

            //然后使主角移动
            // transform.rotation = Quaternion.Euler(0, velocity > 0 ? 0 : 180, 0);
            transform.Translate(Time.deltaTime * (right ? Vector3.right : Vector3.left));

            animator.Play("walk");
        }
    }
    public void jump(bool up)
    {
        if (onLadder)//如果在楼梯上就上楼梯
        {
            if (!isClimbing)
                StartCoroutine(climbLadder(up));
        }
        else//如果不在楼梯上就跳
        {
            if (!isJumping)
            {
                rigid.useGravity = false;
                isJumping = true;
                jumps[0].from = transform.position;
                jumps[0].to = transform.position + jumpHigh;
                jumps[0].ResetToBeginning();
                jumps[0].PlayForward();
                animator.Play("jump");
            }
        }

    }
    IEnumerator climbLadder(bool up)
    {
        isClimbing = true;
        while (onLadder)
        {
            transform.Translate((up ? Vector3.up : Vector3.down) * Time.deltaTime);
            yield return null;
        }
        isClimbing = false;
    }
    Transform ladder;
    IEnumerator jumpOffLadder()
    {
        rigid.useGravity = true;
        yield return 1;

        while (rigid.velocity.magnitude != 0)
        {//说明还在动
            yield return null;
        }
        rigid.useGravity = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ladder")
        {
            print("onLadder");
            onLadder = true;
            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "ladder")
        {
            onLadder = false;
            rigid.useGravity = true;
            if (isClimbing)
            {
                Transform top = other.transform.FindChild("top"),
                    bottom = other.transform.FindChild("bottom");
                Vector3 endPos = (top.position.y + bottom.position.y) / 2 > transform.position.y ?
                    bottom.position : top.position;
                transform.position = new Vector3(transform.position.x, endPos.y, endPos.z);
                print("success climb");
            }
            print("exit Ladder");
        }
    }
    public void idle()
    {
        //transform
        if (!isJumping)
            animator.Play("idle");
    }
    public LineRenderer line;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Vector3 from = transform.FindChild("scout1").position;
            RaycastHit[] hits = Physics.RaycastAll(from, -transform.forward, 50, 1 << LayerMask.NameToLayer("Obstacle"));
            print(hits.Length);
            if (hits.Length != 0)
            {
                Vector3 to = from;
                for (int i = 0; i < hits.Length; ++i)
                {
                    print(i + ": " + hits[i].point);
                    if (Vector3.Distance(from, to) < Vector3.Distance(from, hits[i].point))
                        to = hits[i].point;
                }
                line.SetPosition(0, from);
                line.SetPosition(1, to);
            }
        }
        if (rigid.velocity.magnitude == 0 && !isClimbing)
            idle();
    }
}
