using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    public List<GameObject> enemyDeck;
    public bool isEnemyTurn;
    float firstCardPosX = -7.0f;
    float firstCardPosY = 3.5f;
    float firstCardPosZ = -1.0f;
    int currListOffset = 0;
    private BoardCtrl board;
    public Text enemyCnt;
    public Image turn;
    Vector3 deckPos;

    // Start is called before the first frame update
    void Awake()
    {
        enemyDeck = new List<GameObject>();
        board = FindObjectOfType<BoardCtrl>();
        isEnemyTurn = false;
        deckPos = new Vector3(10.0f, 10.0f, 2.0f);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (isChange)
    //        SetCardPos();
    //}
    public void AddCard(GameObject card)
    {
        enemyDeck.Add(card);
        //isChange = true;
        card.transform.position = deckPos;
        SetCardPos();
        SetCntText();
    }
    void SetCntText()
    {
        string tmp = "카드 수 : ";
        tmp += enemyDeck.Count.ToString();
        enemyCnt.text = tmp;
    }
    void SetCardPos()
    {
        for (int i = currListOffset; i < enemyDeck.Count; i++)
        {
            enemyDeck[i].transform.position = new Vector3(firstCardPosX + i * 1.5f, firstCardPosY, firstCardPosZ);
            enemyDeck[currListOffset * 10 + i].GetComponent<Card>().SetCardOwner(CARD_OWNER.ENEMY);
            enemyDeck[currListOffset * 10 + i].transform.parent = this.gameObject.transform;
            if (i == 9)
                break;
        }
    }
    public void SetEnemyTurn()
    {
        if (board.GetCurrTurn() == 1)
        {
            isEnemyTurn = true;
            turn.gameObject.SetActive(true);
        }
        else
        {
            isEnemyTurn = false;
            turn.gameObject.SetActive(false);
        }
    }
    public bool GetPlayerTurn()
    {
        return isEnemyTurn;
    }
    public void SendCardFromDeck(GameObject card)
    {
        if (!isEnemyTurn)
        {
            return;
        }
        int curNum = board.GetCurrNum();
        CARD_TYPE curType = board.GetCurrCardType();
        int curCardDam = board.GetCurrCardDam();

        int cardNum = card.GetComponent<Card>().GetCardNum();
        CARD_TYPE cardType = card.GetComponent<Card>().GetCardType();
        int cardDam = card.GetComponent<Card>().GetCardDamage();

        if (curNum == cardNum || curType == cardType || cardType == CARD_TYPE.JOKER || curType == CARD_TYPE.JOKER)
        {
            if (curCardDam <= cardDam)
            {
                enemyDeck.Remove(card);
                board.ReceiveCardFromPlayer(card);

                SetCardPos();
                SetCntText();
            }
        }
    }
}
