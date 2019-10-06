using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class uniqueRand  {
	private List<int> usedValues = new List<int>();
	private int min;
	private int max;

	public uniqueRand(int mn, int mx) {
		min = mn;
		max = mx;
		resetUniqueRandomInt ();
	}

	public void resetUniqueRandomInt() {
		usedValues.Clear();
	}
	public int nextRandom () {
		if (usedValues.Count == max - min + 1)
			usedValues.Clear ();

		int val = Random.Range (min, max);
		while (usedValues.Contains (val)) {
			val = val - 1;
			if (val < min)
				val = max;
		}
		usedValues.Add (val);
		return val;
	}
}
