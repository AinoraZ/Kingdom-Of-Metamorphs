using UnityEngine;
using System.Collections;

public class ToggleMenu : MonoBehaviour {

	public GameObject menuObj;
	public GameObject[] OtherMenus;

	public void Toggle () {
		for (int i = 0; i < OtherMenus.Length; i++) {
			OtherMenus[i].SetActive(false);
		}
		menuObj.SetActive ( !menuObj.activeSelf );
	}

}
