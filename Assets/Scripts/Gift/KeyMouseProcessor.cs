

using UnityEngine;
using Lite;


public class KeyMouseProcessor : MonoBehaviour
{
	private bool bMouseDown = false;
	private Vector3 mouseDownPosition;
	private Vector3 lastMousePosition;

	private bool bMoveKey = false;


#if UNITY_EDITOR
	void LateUpdate()
	{
		try
		{
			UpdateMouse();
		}
		catch (System.Exception e)
		{
			Log.Error(e.ToString());
		}
	}
#endif


	void UpdateMouse()
	{
		bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
		if (isOverUI)
			return;

		if (Input.GetMouseButtonDown(0))
		{
			if (!bMouseDown)
			{
				bMouseDown = true;
				OnMouseDown();
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			if (bMouseDown)
			{
				bMouseDown = false;
				OnMouseUp();
			}
		}

		// dectect drag event
		if (bMouseDown)
		{
			var down = lastMousePosition;
			var pos = Input.mousePosition;
			if (!Mathf.Approximately(down.x, pos.x) || !Mathf.Approximately(down.y, pos.y) || !Mathf.Approximately(down.z, pos.z))
			{
				OnMouseDrag();
			}
		}

		float scrollValue = Input.GetAxis("Mouse ScrollWheel");
		AppControl.Instance.OnTouchScale(scrollValue * 5);

	}


	private void OnMouseDown()
	{
		mouseDownPosition = Input.mousePosition;
		lastMousePosition = Input.mousePosition;
		AppControl.Instance.OnTouchBegan();
	}


	private void OnMouseUp()
	{
		AppControl.Instance.OnTouchEnd();
	}


	private void OnMouseDrag()
	{
		var curPos = Input.mousePosition;
		var d = curPos - lastMousePosition;
		lastMousePosition = Input.mousePosition;
		AppControl.Instance.OnTouchMoved(d.x, d.y);
	}


}

