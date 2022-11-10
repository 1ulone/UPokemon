using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowTextParent : MonoBehaviour
{
    private Text txtPar, txt;

    void Start()
    {
        txtPar = this.transform.parent.GetComponent<Text>();
        txt = GetComponent<Text>();
    }

    void Update()
    {
        if (txtPar.text != "")
            { txt.text = txtPar.text; }
    }
}
