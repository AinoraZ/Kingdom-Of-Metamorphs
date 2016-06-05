using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour {

	public GameObject menuObj;
	public GameObject[] OtherMenus;

	public void Toggle () {
		for (int i = 0; i < OtherMenus.Length; i++) {
			OtherMenus[i].SetActive(false);
		}
		menuObj.SetActive ( !menuObj.activeSelf );
		if (menuObj.activeSelf) {
			gameObject.GetComponent<Image>().color = new Color(0, 255, 0);
		}
		else
			gameObject.GetComponent<Image>().color = new Color(255, 0, 0);
	}

}
