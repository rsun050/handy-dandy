using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Fragment", menuName = "ScriptableObjects/Dialogue Fragment", order = 1)]
public class DialogueFragment : Fragment
{
	public List<string> dialogue;
	public List<string> playerOptions; // can be null (ie: dialogue ends)
}