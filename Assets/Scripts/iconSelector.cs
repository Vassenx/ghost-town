using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iconSelector : MonoBehaviour
{
    public List<Sprite> ghostIcons;
    public List<Sprite> ruinsIcons;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void change(int index, bool ghost)
    {
        if (ghost)
        {
            sprite.sprite = ghostIcons[index];
        }
        else
        {
            sprite.sprite = ruinsIcons[index];
        }

    }
}
