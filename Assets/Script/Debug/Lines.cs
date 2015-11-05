using UnityEngine;
using System.Collections;

public class Lines : MonoBehaviour {
    LineRenderer[] lines;
    public Transform carl;
	void Start () {
        lines = new LineRenderer[10];
        GameObject line = transform.FindChild("line0").gameObject;
        lines[0] = line.GetComponent<LineRenderer>();

        for (int i = 1; i < 10; ++i)
        {
            GameObject newLine = GameObject.Instantiate(line) as GameObject;
            newLine.transform.parent = transform;
            newLine.name = "line" + i;

            lines[i] = newLine.GetComponent<LineRenderer>();
            lines[i].SetPosition(0, Vector3.zero);
            lines[i].SetPosition(1, Vector3.zero);
        }
	}
	
    public void setLine(int index,Vector3 v1,Vector3 v2){
        if (index > 9)
            return;
        lines[index].SetPosition(0, v1);
        lines[index].SetPosition(1, v2);
    }

	// Update is called once per frame
	void FixedUpdate () {
      //  lines[0].SetPosition(1, carl.right);
	}
}
