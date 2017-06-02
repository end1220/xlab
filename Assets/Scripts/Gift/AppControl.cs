
using UnityEngine;


public class AppControl : MonoBehaviour
{
	public static AppControl Instance;

	//float ax = 0;
	//float ay = 0;

	private float rotateSpeed = 10;
	private const float decSpeed = 20;
	private const float minSpeed = 10;
	private const float maxSpeed = 100;

	private float cameraDistance;
	private Camera cam;
	private const float minDistance = 3;
	private const float maxDistance = 30;


	void Awake()
	{
		Instance = this;
		cam = GameObject.FindObjectOfType<Camera>();
		cameraDistance = (cam.transform.position - transform.position).magnitude;
	}


	void Update()
	{
		//transform.Rotate(new Vector3(0, angelY, 0));
	}


	void LateUpdate()
	{
		//ax += Input.acceleration.x * 30;
		//ay += -Input.acceleration.y * 30;

		if (rotateSpeed > minSpeed)
			rotateSpeed -= 20 * Time.deltaTime;
		else if (rotateSpeed < -minSpeed)
			rotateSpeed += 20 * Time.deltaTime;
		else
			rotateSpeed = rotateSpeed > 0 ? minSpeed : -minSpeed;
		transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));

		LerpCamera();
	}


	public void OnTouchBegan()
	{

	}

	public void OnTouchEnd()
	{
		
	}


	public void OnTouchMoved(float dx, float dy)
	{
		rotateSpeed = -dx / Time.deltaTime;
		if (rotateSpeed > maxSpeed)
			rotateSpeed = maxSpeed;
		else if (rotateSpeed < -maxSpeed)
			rotateSpeed = -maxSpeed;
		transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
	}


	public void OnTouchScale(float delta)
	{
		cameraDistance += delta;
		if (cameraDistance < minDistance)
			cameraDistance = minDistance;
		else if (cameraDistance > maxDistance)
			cameraDistance = maxDistance;
	}

	void LerpCamera()
	{
		Vector3 mPosition = cam.transform.rotation * new Vector3(0, 0, -cameraDistance) + transform.position;
		cam.transform.position = Vector3.Lerp(cam.transform.position, mPosition, Time.deltaTime * 5);
	}

}

