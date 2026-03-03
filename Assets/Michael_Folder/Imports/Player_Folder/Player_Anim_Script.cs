using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Anim_Script : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;

    public void Enable_Walk_Animation()
    {
        Reset_Every_Animation();
        myAnimator.SetBool("Can_Walk", true);
    }

    public void Enable_Wave_Animation()
    {
        Reset_Every_Animation();
        myAnimator.SetBool("Can_Wave", true);
    }

    public void Enable_Idle_Animation()
    {
        Reset_Every_Animation();
        myAnimator.SetBool("Can_Idle", true);
    }

    public void Reset_Every_Animation()
    {
        myAnimator.SetBool("Can_Walk", false);
        myAnimator.SetBool("Can_Wave", false);
        myAnimator.SetBool("Can_Idle", false);
    }
}
