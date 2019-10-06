using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scrollController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
	ScrollRect scroll;


	// Use this for initialization
	void Start () {
		scroll = GetComponent<ScrollRect>();
		scroll.verticalNormalizedPosition = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDrag (PointerEventData eventData) {
	}

	public void OnEndDrag (PointerEventData eventData) {
	}

	public void OnBeginDrag (PointerEventData eventData) {
	}
}
