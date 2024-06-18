using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    
    [SerializeField] public Texture ZeroStamps;
    [SerializeField] public Texture OneStamps;
    [SerializeField] public Texture TwoStamps;
    [SerializeField] public Texture ThreeStamps;
    
    public Texture ReturnStampImage(int score)
    {
        if (score == 0)
        {
            return ZeroStamps;
        }
        else if (score == 1)
        {
            return OneStamps;
        }
        else if (score == 2)
        {
            return TwoStamps;
        }
        else if (score == 3)
        {
            return ThreeStamps;
        }
        return null;
    }
}
