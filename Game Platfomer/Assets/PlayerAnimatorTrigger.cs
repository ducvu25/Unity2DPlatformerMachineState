using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorTrigger : MonoBehaviour
{
    Player player => GetComponentInParent<Player>();

    void AnimationTrigger() => player.AnimationTrigger();

}
