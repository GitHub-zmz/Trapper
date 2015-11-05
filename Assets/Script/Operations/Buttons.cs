using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

	public  UIEventListener[] buttons;
    public Carl carl;
	void Start () {
	    foreach(UIEventListener btn in buttons)
            btn.onClick = onClick;
	}

    void onClick(GameObject go)
    {
        switch (go.name)
        {
            

        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
