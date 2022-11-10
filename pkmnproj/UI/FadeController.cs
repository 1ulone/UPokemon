using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController instances;

    private Animator anim;

    void Start()
    {
        instances = this;

        anim = GetComponent<Animator>();
        this.gameObject.SetActive(false);
    }

    public void SetFade(string tag)
    {
        this.gameObject.SetActive(true);
        anim.Play(tag);
    }

    public void ResetFade()
    {
        this.gameObject.SetActive(false);
    }
}
