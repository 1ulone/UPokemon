using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumberGenerator : MonoBehaviour
{
    public static RandomNumberGenerator instances;

    void Awake()
        => instances = this;

    public (bool b, string txt) CheckAccuracy(float acc, StatusAilments status)
    {
        bool res = false;
        string restxt = "";

        switch (status)
        {
            case StatusAilments.Paralyzed :
            {
                float r = Random.Range(1, 101);

                if (r <= 25)
                { 
                    res = false; 
                    restxt = "is paralyzed"; 
                    return (res, restxt);
                } else if (r > 25) { return (true, ""); }
            } break;
            
            case StatusAilments.Frozen :
            {
                res = false;
                restxt = "is frozen solid";
                return (res, restxt);
            }

            case StatusAilments.Sleep : 
            {
                res = false;
                restxt = "is fast asleep";
                return (res, restxt);
            }
            
            case StatusAilments.Confused :
            {
                float r = Random.Range(1, 101);

                if (r <= 50)
                {
                    res = false;
                    restxt = "";
                    return (res, restxt);
                } else if (r > 25) { return (true, ""); }
                
            } break;

            default : 
            {
                float r = Random.Range(1, 101);

                if (r > acc)
                { 
                    res = false; 
                    restxt = "missed"; 
                    return (res, restxt);
                } else if (r <= acc) { return (true, ""); }
            } break;
        }

        return (res, restxt);
    }

    public bool onShinyEncounter()
    {
        bool res = false;
        float r = Random.Range(1, 4096);

        if (r == 1)
            { res = true; }

        return res;
    }

    public int CheckGetStatus(float chances)
    {
        int res = 0;
        float r = Random.Range(1, 100);

        if (r <= chances)
            { res = Random.Range(1, 5); }

        return res;
    }
}
