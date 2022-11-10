using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTypeController : MonoBehaviour
{
    public static SpriteTypeController instances;

    [System.Serializable]
    public class movesOptionsUI
    {
        public Sprite sprite;
        public string tag;
    }
    
    public List<movesOptionsUI> optionsUIList;
    private Dictionary<string, Sprite> optionsDictionary = new Dictionary<string, Sprite>();

    void Awake()
    {
        instances = this;

        foreach(movesOptionsUI mo in optionsUIList)
            optionsDictionary.Add(mo.tag, mo.sprite);
    }

    public Sprite GetSprite(string type)
    {
        if (!optionsDictionary.ContainsKey(type))
            return optionsDictionary["EMPTY"];

        return optionsDictionary[type];
    }
}
