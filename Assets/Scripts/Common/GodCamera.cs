
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lite
{
	public class GodCamera : MonoBehaviour
	{
		public static GodCamera Instance { get; set; }

		// camera positon
		private float camPosX = 0;
		private float camPosZ = -20;
		private float camPosY = 20;
		private const float maxX = 32;
		private const float minX = -32;
		private const float maxY = 35;
		private const float minY = 5;
		private const float maxZ = 20;
		private const float minZ = -20;
		
		private float ZoomSpeed = 30;
		private float DragSpeed = 30;

		private float dampingY = 10;
		private float dampingXZ = 20;

		void Awake()
		{
			Instance = this;
		}

		void Start()
		{
			transform.position = new Vector3(camPosX, camPosY, camPosZ);
		}

		void Update()
		{
			ProcessMouse();

			ProcessKeyboard();
		}

		void LateUpdate()
		{
			try
			{
				AdjustCamera();
			}
			catch (UnityException ue)
			{
				Log.Error(ue.ToString());
			}
		}

		public void ZoomInOut(float scrollValue)
		{
			camPosY -= scrollValue * ZoomSpeed;
			camPosY = Mathf.Clamp(camPosY, minY, maxY);
		}

		float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360)
				angle += 360;
			if (angle > 360)
				angle -= 360;
			return Mathf.Clamp(angle, min, max);
		}

		private void AdjustCamera()
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, camPosY, transform.position.z), Time.deltaTime * dampingY);

			transform.position = Vector3.Lerp(transform.position, new Vector3(camPosX, transform.position.y, camPosZ), Time.deltaTime * dampingXZ);
		}

		bool isMiddleButtonDown = false;
		Vector2 mouseLastPosition;
		Vector2 mousePositionOffset;
		void ProcessMouse()
		{
			ZoomInOut(Input.GetAxis("Mouse ScrollWheel"));

			if (GUIUtility.hotControl == 0 && Input.GetMouseButtonUp(0))
			{
				OnClickScene(Input.mousePosition);
			}
			if (Input.GetMouseButtonDown(2))
			{
				if (!isMiddleButtonDown)
				{
					isMiddleButtonDown = true;
					mouseLastPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				}
			}
			if (Input.GetMouseButtonUp(2))
			{
				isMiddleButtonDown = false;
			}

			if (isMiddleButtonDown)
			{
				mousePositionOffset = new Vector2(Input.mousePosition.x - mouseLastPosition.x, Input.mousePosition.y - mouseLastPosition.y);
				mouseLastPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				camPosX -= mousePositionOffset.x / Screen.width * DragSpeed;
				camPosZ -= mousePositionOffset.y / Screen.height * DragSpeed;
				camPosX = Mathf.Clamp(camPosX, minX, maxX);
				camPosZ = Mathf.Clamp(camPosZ, minZ, maxZ);
			}

		}

		void ProcessKeyboard()
		{
			if (Input.GetKey(KeyCode.Space))
			{
			}
		}

		private void OnClickScene(Vector3 mousePosition)
		{
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			int layerMask =
				1 << LayerMask.NameToLayer(AppDefine.LayerTerrain)
				| 1 << LayerMask.NameToLayer(AppDefine.LayerBot);

			if (Physics.Raycast(ray, out hit, 50, layerMask))
			{
				Vector3 hitPoint = hit.point;
				int hitLayer = hit.collider.gameObject.layer;
				if (hitLayer == LayerMask.NameToLayer(AppDefine.LayerTerrain))
				{
					OnClickTerrain(hitPoint);
				}
				else if (hitLayer == LayerMask.NameToLayer(AppDefine.LayerBot))
				{
					
				}
			}
		}

		private GameObject clickTerrainEffect;
		private void OnClickTerrain(Vector3 position)
		{
			if (clickTerrainEffect == null)
			{
				var prefab = Resources.Load("Prefabs/Bot1");
				clickTerrainEffect = GameObject.Instantiate(prefab) as GameObject;
			}
			clickTerrainEffect.transform.position = position + new Vector3(0,-0.495f,0);
		}

		

	}

}