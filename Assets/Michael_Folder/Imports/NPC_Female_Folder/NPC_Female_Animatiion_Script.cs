using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Female_Animatiion_Script : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Enable_Idle_Animation();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Enable_Walk_Animation();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Enable_Wave_Animation();
        }
    }

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
