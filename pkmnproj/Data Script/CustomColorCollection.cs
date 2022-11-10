using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Color Collection", menuName = "Collection/New Color Collection")]
public class CustomColorCollection : ScriptableObject
{
    public Dictionary<string, Color> CCDictionary = new Dictionary<string, Color>();
}
