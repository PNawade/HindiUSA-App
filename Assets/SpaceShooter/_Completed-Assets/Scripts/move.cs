using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;



public class move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	
	private Vector2 origin;
	private Vector2 dir;
	public Text txt;

	public void OnPointerDown (PointerEventData data)
	{
		origin = data.position;
		dir.x = 0;
		dir.y = 0;
	}

	public void OnPointerUp (PointerEventData data)
	{
		dir = data.position - origin;
		dir = dir.normalized;
		dir.x = 0;
		dir.y = 0;
	}

	public void OnDrag(PointerEventData data)
	{
		dir = data.position - origin;
		dir = dir.normalized;
		txt.text = dir.x + " : " + dir.y;
	}

	public Vector2 getDir ()  {
		return dir;
	}
}