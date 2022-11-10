using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextSystem : MonoBehaviour
{
    public static TextSystem instance;

    public Text txt;
    // private Animator anim;

    [SerializeField]
    private float typeSpeed = 50f;
    [SerializeField]
    private int lineMax = 1;

    bool ExecuteCoroutine; //, UIanim, ExecuteInvoke;
    int currentLine;
    float upTimer;

    private List<string> texttotype = new List<string>();

    void Start()
    {
        instance = this;
        // anim = GetComponent<Animator>();
        ExecuteCoroutine = false;
        // ExecuteInvoke = false;
    }

    // void Update()
    // {
    //     if (UIanim)
    //     {
    //         UIanim = false; 
    //         if (!ExecuteInvoke)
    //         {
    //             ExecuteInvoke = true;
    //             Invoke("ResetAnim", upTimer + 6f);
    //         }
    //     }
    // }

    public void StartDialogue(string dt)
    {
        // anim.SetTrigger("goUp");
        // UIanim = true;

        if (!ExecuteCoroutine)
            StartCoroutine(DialogueCoroutine(dt));
    }

    // void ResetAnim()
    // {
    //     ExecuteInvoke = false;
    //     upTimer = 0;
    //     anim.SetTrigger("goDown"); 
    // }

    IEnumerator DialogueCoroutine(string DialogueToType)
    {
        // ExecuteCoroutine = true;
        if (!texttotype.Contains(DialogueToType))
            texttotype.Add(DialogueToType);

        while(texttotype.Count > 0)
        {
            string newDialogue = texttotype[0];
            float t = 0;
            int charIndex = 0;
            string oldDialogue = "";
            upTimer += newDialogue.Length* 0.1f;

            if (currentLine >= lineMax) 
                txt.text = "";
            oldDialogue = txt.text;

            while(charIndex < newDialogue.Length)
            {
                t += Time.deltaTime * typeSpeed;
                charIndex = Mathf.FloorToInt(t);
                charIndex = Mathf.Clamp(charIndex, 0, newDialogue.Length);

                txt.text = newDialogue.Substring(0, charIndex);

                yield return null;

            } 

            txt.text = oldDialogue + newDialogue;
            currentLine++;
            ExecuteCoroutine = false;
            texttotype.Remove(newDialogue);

            yield return new WaitForSeconds(1.2f);
        }
    }
}

// 123456789H123456789H123456789H123456789H123456789H123456789H123456789H123456789H123456789H123456789H