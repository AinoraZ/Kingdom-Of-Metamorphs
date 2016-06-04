using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public float orthographicSizeMin = 2f;      //Zoom min
	public float orthographicSizeMax = 30f;     //Zoom max

	void Update () {
		Zoom();
		CameraMoveUpdate();
	}

	void Zoom() {
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
			Camera.main.orthographicSize++;
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
			Camera.main.orthographicSize--;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);     //Camera zoom min and max
	}

	bool buttonPress = false;
	Vector3 mousePosition;                      //Previous mouse position
	public float divideBy = 10f;    //Divide the movement by this amount

	void CameraMoveUpdate() {
		if (Input.GetMouseButtonDown(1))    //Activate when mouse button is clicked
			CameraMove();
		if (buttonPress)                    //While it's down
			CameraMove();
		if (Input.GetMouseButtonUp(1))  //Right button is not pressed anymore
			buttonPress = false;
	}

	void CameraMove(){
		if (!buttonPress){
			mousePosition = Input.mousePosition;
			buttonPress = true;     //Right Button is pressed right now
		}
		else{
			GameObject.FindWithTag("MainCamera").transform.position = new Vector3(transform.position.x + ((mousePosition.x - Input.mousePosition.x) / divideBy), (transform.position.y + (mousePosition.y - Input.mousePosition.y) / divideBy), -6.79f);
			mousePosition = Input.mousePosition;
		}
	}
}
