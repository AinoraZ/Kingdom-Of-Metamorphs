using UnityEngine;
using System.Collections;

public class CameraZom : MonoBehaviour {

	public Camera cam;
	public int zoomValue;

	public int minSize;
	public int maxSize;

	public void Zoom () {
		if(cam.orthographicSize + zoomValue >= minSize && cam.orthographicSize + zoomValue <= maxSize) cam.orthographicSize += zoomValue;
	}
}
