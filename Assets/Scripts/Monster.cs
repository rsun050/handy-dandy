using System.Collections;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
	[field: SerializeField] public NPCData data {get; private set; }
	[SerializeField] private Animation animator;

	[SerializeField] ItemData[] itemsOfInterest;
	[SerializeField] DialogueFragment[] correspondingDialogues;
	[SerializeField] AudioClip[] sfx;
	[SerializeField] DialogueFragment[] defaultDialogues;
	[SerializeField] AudioSource audioSource;

	private bool canKill = false;

	public void Start() {
		DialogueManager.Instance.dialogueEndE += Kill;
	}

	public void Interact() {
		canKill = false;
		for(int i = 0; i < itemsOfInterest.Length; i++) {
			if(InventoryManager.Instance.Has(itemsOfInterest[i], 1)) {
				DialogueManager.Instance.StartDialogue(correspondingDialogues[i]);
				audioSource.clip = sfx[i];
				canKill = true;
				return;
			}
		}

		int rng = Random.Range(0, defaultDialogues.Length);
		DialogueManager.Instance.StartDialogue(defaultDialogues[rng]);
		audioSource.Play();
	}

	public void Kill() {
		if(canKill) {
			// play animation
			audioSource.Play();
			StartCoroutine(DeathAnimationAndDestroy("sj001_hurt"));
			
		}
	}

	IEnumerator DeathAnimationAndDestroy(string clipName) {
		animator.Play(clipName);

		yield return null;

		while(animator.IsPlaying(clipName)) {
			yield return null;
		}

		// destroy this gameobject
		Destroy(this.gameObject);
	}
}