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

	public void Interact() {
		canKill = false;
		for(int i = 0; i < itemsOfInterest.Length; i++) {
			if(InventoryManager.Instance.Has(itemsOfInterest[i], 1)) {
				DialogueManager.Instance.StartDialogue(correspondingDialogues[i]);
				DialogueManager.Instance.optionChosenE += Kill;
				audioSource.clip = sfx[i];
				canKill = true;
				return;
			}
		}

		int rng = Random.Range(0, defaultDialogues.Length);
		DialogueManager.Instance.StartDialogue(defaultDialogues[rng]);
		audioSource.Play();
	}

	private void Kill(Choice choice) {
		if(canKill && choice == Choice.Affirmative) {
			// play animation
			audioSource.Play();
			StartCoroutine(DeathAnimationAndDestroy("sj001_hurt"));
		}

		DialogueManager.Instance.optionChosenE -= Kill;
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