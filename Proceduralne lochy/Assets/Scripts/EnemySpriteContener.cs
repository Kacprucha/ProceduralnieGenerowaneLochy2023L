using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteContener : MonoBehaviour
{
    [SerializeField]
    private Sprite vampire1, vampire2, skeleton1, skeleton2, skeleton3;

    [SerializeField]
    public LayerMask PlayerLayer;

    public Sprite GetVampireSprite ()
    {
        Sprite result;

        int randomMinus = Random.Range (0, 2) * 2 - 1;

        if (randomMinus > 0)
        {
            result = vampire1;
        }
        else
        {
            result = vampire2;
        }

        return result;
    }

    public Sprite GetSkeletonSprite ()
    {
        Sprite result;

        int random = Random.Range (0, 2);

        switch (random)
        {
            case 0:
                result = skeleton1;

                break;

            case 1:
                result = skeleton2;

                break;

            case 2:
                result = skeleton3;

                break;

            default:
                result = skeleton1;

                break;
        }

        return result;
    }
}
