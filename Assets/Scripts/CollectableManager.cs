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
    
    [SerializeField] private RawImage StampsImage;
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetScore() == 0)
        {
            StampsImage.texture = ZeroStamps;
        }
        else if (GameManager.Instance.GetScore() == 1)
        {
            StampsImage.texture = OneStamps;
        }
        else if (GameManager.Instance.GetScore() == 2)
        {
            StampsImage.texture = TwoStamps;
        }
        else if (GameManager.Instance.GetScore() == 3)
        {
            StampsImage.texture = ThreeStamps;
        }
    }
}
