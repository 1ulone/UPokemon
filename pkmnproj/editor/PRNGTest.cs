using UnityEditor;
using UnityEngine;

public class PRNGTest : MonoBehaviour
{
    [MenuItem("Utilities/Generate PRNG")]
    public static void GeneratePRNG()
    {
        for (int i = 0; i < 20; i++)
            Debug.Log(PseudoRandomGeneration());
    }

    public static int PseudoRandomGeneration()
    {
        var seed = 1234;
        var digits = 4;

        var n = (seed * seed).ToString();
        while(n.Length < digits * 2)
        {
            n = "0" + n;
        }

        var start = System.Math.Floor((decimal)digits /2);
        var end = start + digits;

        seed = int.Parse(n.Substring((int)start, (int)end));

        return seed;
    }
}
