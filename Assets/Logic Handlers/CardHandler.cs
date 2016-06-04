using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour {

	public List<GameObject> cardFathers;
	int cardNum = 0;
	public List<Sprite> cardTextures;

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {

	}

	public CardInfo[] cards = {new CardInfo(), new CardInfo(), new CardInfo(), new CardInfo()};
	private GameObject clickedCard;

	public void CardClicked(GameObject card) {
		if (GetComponent<TurnHandler>().Move) {
			for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
				GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().ColorReset();
			}
			clickedCard = card;
			for (int x = 0; x < cards.Length; x++) {
				if (card == cards[x].father) {
					effectTemp = cards[x].cardEffectNum;
					Mark();
				}
			}
		}
	}

	bool WaitForClick = false;
	List<GameObject> possibleMove = new List<GameObject>();

	void Mark() {
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P1") {
					GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().PossibleMove();
					possibleMove.Add(GameObject.FindGameObjectsWithTag("Tile")[x]);
				}
			}
		}
		WaitForClick = true;
	}

	int effectTemp = 0;

	public void CardEffect(GameObject minion) {
		if (effectTemp == 0) {
			minion.GetComponent<MinionInfo>().atk = (int)(minion.GetComponent<MinionInfo>().atk * 1.5f);
		}
		else if (effectTemp == 1) {
			minion.GetComponent<MinionInfo>().def = (int)(minion.GetComponent<MinionInfo>().def * 1.5f);
		}
		else if (effectTemp == 2) {
			minion.GetComponent<MinionInfo>().atk += 3;
		}
		else if (effectTemp == 3) {
			minion.GetComponent<MinionInfo>().def += 3;
		}
		else {
			minion.GetComponent<MinionInfo>().atk += 1;
			minion.GetComponent<MinionInfo>().def += 2;
		}
	}

	public void MinionClicked(GameObject clicked) {
		if (WaitForClick) {
			if (Uti.PossibleMoveCheck(clicked, possibleMove)) {
				cardNum--;
				CardEffect(clicked.GetComponent<TileInfo>().minion);
				effectTemp = 0;
				clickedCard.GetComponent<BoxCollider2D>().enabled = false;
				clickedCard.GetComponent<Image>().enabled = false;
				clickedCard = null;
				WaitForClick = false;
				for (int x = 0; x < Uti.ListLength(possibleMove); x++) {
					possibleMove[x].GetComponent<TileInfo>().ColorReset();
				}

				possibleMove = new List<GameObject>();
			}
			else {
				effectTemp = 0;
				WaitForClick = false;
				for (int x = 0; x < Uti.ListLength(possibleMove); x++) {
					possibleMove[x].GetComponent<TileInfo>().ColorReset();
				}
				possibleMove = new List<GameObject>();
			}
		}
	}

	public void CardAdd() {
		if (cardNum <= 4) {
			int emptyNum = emptySlot();
			if (emptyNum != -1) {
				cards[emptyNum].father = cardFathers[emptyNum];
				cards[emptyNum].cardEffectNum = new System.Random().Next(5);
				cardFathers[emptyNum].GetComponent<BoxCollider2D>().enabled = true;
				cardFathers[emptyNum].GetComponent<Image>().sprite = cardTextures[cards[emptyNum].cardEffectNum];
				cardFathers[emptyNum].GetComponent<Image>().enabled = true;
				cardNum++;
			}
		}
	}

	private int emptySlot() {
		for (int x = 0; x < Uti.ListLength(cardFathers); x++) {
			if (cardFathers[x].GetComponent<BoxCollider2D>().enabled == false) {
				return x;
			}
		}
		return -1;
	}

	public void TurnEnd() {
		clickedCard = null;
		effectTemp = 0;
		WaitForClick = false;
		possibleMove = new List<GameObject>();
	}
}

public class CardInfo {
	public GameObject father;
	public int cardEffectNum;
}
