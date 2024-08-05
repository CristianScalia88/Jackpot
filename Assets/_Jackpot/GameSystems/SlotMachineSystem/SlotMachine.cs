using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Jackpot.GameSystems.SlotMachineSystem {
	public class SlotMachine : MonoBehaviour {
		[SerializeField] private List<InfiniteReel> reels;
		[SerializeField] private float delayBetweenReels = .15f;
		[SerializeField] private float spinTimeMin = 2f;
		[SerializeField] private float spinTimeMax = 4f;
		[SerializeField] private Button spinButton;

		private void Start() {
			spinButton.onClick.AddListener(Spin);
		}

		private void Spin() {
			StartCoroutine(SpinCoroutine());
		}

		private IEnumerator SpinCoroutine() {
			spinButton.interactable = false;
			foreach (var reel in reels) {
				reel.Spin();
				yield return new WaitForSeconds(delayBetweenReels);
			}
			yield return new WaitForSeconds(Random.Range(spinTimeMin, spinTimeMax));
			foreach (var reel in reels) {
				reel.Stop();
				yield return new WaitForSeconds(delayBetweenReels);
			}

			yield return CollectScoreSequence();
			spinButton.interactable = true;
		}

		private IEnumerator CollectScoreSequence() {
			yield return new WaitForSeconds(1);
		}
	}
}