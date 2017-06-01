
using UnityEngine;


public class AppControl : MonoBehaviour
{
	public static AppControl Instance;

	float ax = 0;
	float ay = 0;

	private float rotateSpeed = 10;

	private float minSpeed = 10;
	private float maxSpeed = 100;


	void Awake()
	{
		Instance = this;
	}


	void Update()
	{
		//transform.Rotate(new Vector3(0, angelY, 0));
	}


	void LateUpdate()
	{
		ax += Input.acceleration.x * 30;
		ay += -Input.acceleration.y * 30;

		if (rotateSpeed > minSpeed)
			rotateSpeed -= 20 * Time.deltaTime;
		else if (rotateSpeed < -minSpeed)
			rotateSpeed += 20 * Time.deltaTime;
		else
			rotateSpeed = rotateSpeed > 0 ? minSpeed : -minSpeed;
		transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
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


}

