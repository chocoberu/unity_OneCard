using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    public List<GameObject> enemyDeck;
    public bool isEnemyTurn;
    public bool leftDown;
    public bool rightDown;
    float firstCardPosX = -7.0f;
    float firstCardPosY = 3.5f;
    float firstCardPosZ = -1.0f;
    public int currListOffset = 0;
    int maxListOffset = 0;
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
        if ((enemyDeck.Count - 1) / 10 > maxListOffset)
        {
            maxListOffset = (enemyDeck.Count - 1) / 10;
        }
        //isChange = true;
        card.transform.position = deckPos;
        card.GetComponent<Card>().SetCardOwner(CARD_OWNER.ENEMY);
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
        if (enemyDeck.Count <= 0)
            return;
        if (currListOffset > 0 && rightDown)
        {
            for (int i = 0; i < 10; i++)
            {
                //Debug.Log((currListOffset - 1) * 10 + i);
                enemyDeck[(currListOffset - 1) * 10 + i].transform.position = deckPos;
            }
        }
        if (currListOffset < maxListOffset && leftDown)
        {
            for (int i = 0; i < 10; i++)
            {
                //Debug.Log((currListOffset - 1) * 10 + i);
                enemyDeck[(currListOffset + 1) * 10 + i].transform.position = deckPos;
                if ((currListOffset + 1) * 10 + i == enemyDeck.Count - 1)
                    break;
            }
        }
        for (int i = 0; i < 10; i++)
        {
            enemyDeck[currListOffset * 10 + i].transform.position = new Vector3(firstCardPosX + i * 1.5f, firstCardPosY, firstCardPosZ);
            //playerDeck[currListOffset * 10 + i].GetComponent<Card>().SetCardOwner(CARD_OWNER.PLAYER);
            //playerDeck[currListOffset * 10 + i].transform.parent = this.gameObject.transform;
            if (i == 9)
                break;
            if (currListOffset * 10 + i == enemyDeck.Count - 1)
                break;
        }
        leftDown = rightDown = false;
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
                maxListOffset = (enemyDeck.Count - 1) / 10;
                if (maxListOffset < currListOffset)
                    currListOffset = maxListOffset;
                SetCardPos();
                SetCntText();
            }
        }
    }
    public void SetCurrentListOffset(int offset)
    {
        currListOffset = offset;
        SetCardPos();
    }
    public int GetCurrentListOffset()
    {
        return currListOffset;
    }
    public int GetMaxListOffset()
    {
        return maxListOffset;
    }
}
