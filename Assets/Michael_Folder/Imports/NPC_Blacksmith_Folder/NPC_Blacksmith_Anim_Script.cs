using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Blacksmith_Anim_Script : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            Enable_Idle_Animation();
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Enable_Smith_Animation();
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Enable_Wave_Animation();
        }
    }

    public void Enable_Smith_Animation()
    {
        Reset_Every_Animation();
        myAnimator.SetBool("Can_Smith", true);
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
        myAnimator.SetBool("Can_Smith", false);
        myAnimator.SetBool("Can_Wave", false);
        myAnimator.SetBool("Can_Idle", false);
    }
}
