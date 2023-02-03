using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class indicatorColor : MonoBehaviour
{
    public List<Color> reColor;

    public List<SpriteRenderer> spritesToColor;

    public void change(int color)
    {
        foreach(SpriteRenderer sprite in spritesToColor)
        {
            sprite.color = reColor[color];

            if (color == 0)
            {
                sprite.sortingLayerName = "Ghost";
            }
            else if (color==1)
            {
                sprite.sortingLayerName = "Ruin";
            }
        }
    }
}
