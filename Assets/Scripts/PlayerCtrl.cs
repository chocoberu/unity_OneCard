using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public List<GameObject> playerDeck;
    public bool isPlayerTurn;
    public bool leftDown;
    public bool rightDown;
    float firstCardPosX = -7.0f;
    float firstCardPosY = -3.5f;
    float firstCardPosZ = -1.0f;
    public int currListOffset = 0;
    int maxListOffset = 0;
    private BoardCtrl board;
    Vector3 deckPos;
    public Text playerCnt;
    public Image turn;

    // Start is called before the first frame update
    void Awake()
    {
        playerDeck = new List<GameObject>();
        board = FindObjectOfType<BoardCtrl>();
        isPlayerTurn = false;
        deckPos = new Vector3(-10.0f, -10.0f, 2.0f);
        leftDown = rightDown = false;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (isChange)
    //    //    SetCardPos();
    //}
    public void AddCard(GameObject card)
    {
        playerDeck.Add(card);
        if((playerDeck.Count - 1) / 10 > maxListOffset)
        {
            maxListOffset = (playerDeck.Count - 1) / 10;
        }
        card.transform.position = deckPos;
        SetCardPos();
        SetCntText();
    }
    void SetCntText()
    {
        string tmp = "카드 수 : ";
        tmp += playerDeck.Count.ToString();
        playerCnt.text = tmp;
    }
    void SetCardPos()
    {
        if (playerDeck.Count <= 0)
            return;
        if (currListOffset > 0 && rightDown)
        {
            for (int i = 0; i < 10; i++)
            {
                //Debug.Log((currListOffset - 1) * 10 + i);
                playerDeck[(currListOffset - 1) * 10 + i].transform.position = deckPos;
            }
        }
        if(currListOffset < maxListOffset && leftDown)
        {
            for (int i = 0; i < 10; i++)
            {
                //Debug.Log((currListOffset - 1) * 10 + i);
                playerDeck[(currListOffset + 1) * 10 + i].transform.position = deckPos;
                if ((currListOffset + 1) * 10 + i == playerDeck.Count - 1)
                    break;
            }
        }
        for (int i = 0; i < 10; i++)
        {
            playerDeck[currListOffset * 10 + i].transform.position = new Vector3(firstCardPosX + i * 1.5f, firstCardPosY, firstCardPosZ);
            //playerDeck[currListOffset * 10 + i].GetComponent<Card>().SetCardOwner(CARD_OWNER.PLAYER);
            //playerDeck[currListOffset * 10 + i].transform.parent = this.gameObject.transform;
            if (i == 9)
                break;
            if (currListOffset * 10 + i == playerDeck.Count - 1)
                break;
        }
        leftDown = rightDown = false;
    }
    public void SetPlayerTurn()
    {
        if (board.GetCurrTurn() == 0)
        {
            isPlayerTurn = true;
            turn.gameObject.SetActive(true);
        }
        else
        {
            isPlayerTurn = false;
            turn.gameObject.SetActive(false);
        }
    }
    public bool GetPlayerTurn()
    {
        return isPlayerTurn;
    }
    public void SendCardFromDeck(GameObject card)
    {
        if (!isPlayerTurn)
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
                board.ReceiveCardFromPlayer(card);
                for (int i = 0; i < playerDeck.Count; i++)
                {
                    if (playerDeck[i] == card)
                    {
                        playerDeck.RemoveAt(i);
                        break;
                    }
                }
                SetCardPos();
                SetCntText();
            }
        }
        if(playerDeck.Count == 0)
        {
            Debug.Log("Player Win!");
            this.gameObject.SetActive(false);
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

