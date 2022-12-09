using UnityEngine;
using System.Collections;

public class DragableObject : MonoBehaviour {

	public bool isDraggable = false;
	Vector3 clickPos;
	Vector3 offset;	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		clickPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
		offset = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		offset = clickPos - offset;
		
	}

	void OnMouseDrag()
	{
		if (!isDraggable)
			return;
		
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		Vector3 newPos = curPosition - offset;
		transform.position = newPos;
	}

}
