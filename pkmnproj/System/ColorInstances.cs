using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInstances : MonoBehaviour
{
    public static ColorInstances i;

    public bool instanced = false;
    public CustomColorCollection collection;

    [System.Serializable]
    public class CustomColor
    {
        public string tag;
        public Color color;
    }

    public List<CustomColor> CCList;

    void Awake()    
        => i = this;

    void Start()
    {
        if (!instanced)
        {
            foreach(CustomColor cc in CCList)
                collection.CCDictionary.Add(cc.tag.ToUpper(), cc.color);

            instanced = true;
        }
    }

    public Color GetCollectionColor(string tag)
    {
        if (!collection.CCDictionary.ContainsKey(tag.ToUpper()))
            return Color.white;

        Color c = new Color();
        c = collection.CCDictionary[tag.ToUpper()];

        return c;
    }
}
