using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public struct PartsSprite {
    public Sprite[] sprite;
    //public Sprite arm_sprite, spoon_sprite, fork_sprite, legSeated_sprite, legStand_sprite, body_sprite, head_sprite, angry_sprite, openMouth_sprite;
}

public class CustomerTypeStats : CustomerStats {
    public PartsSprite[] spriteSheet;
}
