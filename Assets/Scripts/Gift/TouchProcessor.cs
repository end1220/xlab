
using UnityEngine;
using Lite;


public class TouchProcessor : MonoBehaviour
{
	public static TouchProcessor Inst;
	private bool bMoveKey = false;


	private void Awake()
	{
		Inst = this;
	}

	private bool bTouchBegin = false;


	public void LateUpdate()
	{
		try
		{
			UpdateTouchScreen();
		}
		catch (System.Exception e)
		{
			Log.Error(e.ToString());
		}
	}
	
	private void UpdateTouchScreen()
	{
		if (Input.touchCount == 0)
			return;

		//当前触摸在UI上 ?
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
			return;

		if (Input.GetTouch(0).phase == TouchPhase.Began)
		{
			OnTouchBegan();
		}
		else if (Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			OnTouchEnd();
		}
		else if (Input.GetTouch(0).phase == TouchPhase.Canceled)
		{
			OnTouchCanceled();
		}
		else if (Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			OnTouchMoved(Input.GetTouch(0).deltaPosition);
		}
	}


	private void OnTouchBegan()
	{
		bTouchBegin = true;
		AppControl.Instance.OnTouchBegan();
	}

	private void OnTouchEnd()
	{
		bTouchBegin = false;
		AppControl.Instance.OnTouchEnd();
	}

	private void OnTouchCanceled()
	{
		bTouchBegin = false;
		AppControl.Instance.OnTouchEnd();
	}

	private void OnTouchMoved(Vector2 touchDeltaPosition)
	{
		if (bTouchBegin)
		{
			AppControl.Instance.OnTouchMoved(touchDeltaPosition.x, touchDeltaPosition.y);
		}
	}

}

