
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

		if (Input.touchCount == 1)
		{
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
		else if (Input.touchCount == 2)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 touchDeltaPosition0 = Input.GetTouch(0).deltaPosition;
				Vector2 touchDeltaPosition1 = Input.GetTouch(1).deltaPosition;

				if ((touchDeltaPosition0.x * touchDeltaPosition1.x <= 0) && (touchDeltaPosition0.y * touchDeltaPosition1.y <= 0))
				{
					float simble = Input.GetTouch(0).position.x > Input.GetTouch(1).position.x ? touchDeltaPosition1.x - touchDeltaPosition0.x : touchDeltaPosition0.x - touchDeltaPosition1.x;
					simble = simble > 0 ? 1 : -1;
					float len = simble * (touchDeltaPosition0 - touchDeltaPosition1).magnitude * 0.05f;
					AppControl.Instance.OnTouchScale(len);
				}

			}  
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

