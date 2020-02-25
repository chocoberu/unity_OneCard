using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCtrl : MonoBehaviour
{
    public List<GameObject> deck;
    public Transform deckTransform;
    public List<GameObject> usedCardList;

    public PlayerCtrl player;
    public EnemyCtrl enemy;
    Vector3 usedCardPos;
    Vector3 deckPos;

    public CARD_TYPE currType;
    public int currCardNum;
    public int currPlayerTurn; // 현재 턴이 누구 턴인가
    public int currCardDam;
    public int sumCardDam;
    public Text boardCnt;
    public Text currTypeText;
    public Text totalDamText;
    public GameObject ui7;
    int tempTurn;

    // Start is called before the first frame update
    void Start()
    {
        usedCardList = new List<GameObject>();
        usedCardPos = new Vector3(0, 0, 0);
        deckPos = new Vector3(5, 0, 0);
        ShuffleCard();
        InitDeck();
        currPlayerTurn = Random.Range(0, 100) % 2; // 0이면 플레이어 턴, 1이면 에너미 턴

        currCardDam = sumCardDam = 0;
        player.SetPlayerTurn();
        enemy.SetEnemyTurn();
    }

    void InitDeck()
    {
        // 첫 장을 뽑아서 usedCardList에 옮긴다
        usedCardList.Add(deck[deck.Count - 1]);
        Debug.Log(deck[deck.Count - 1].gameObject.name);
        deck.RemoveAt(deck.Count - 1);
        usedCardList[0].gameObject.transform.position = usedCardPos;
        usedCardList[0].GetComponent<Card>().SetCardOwner(CARD_OWNER.USED);
        currType = usedCardList[0].GetComponent<Card>().GetCardType();
        currCardNum = usedCardList[0].GetComponent<Card>().GetCardNum();
        currCardDam = 0;
        SetCurrentTypeText();
        SetTotalDamText();

        for (int i = 0; i < 5; i++)
        {
            if (player)
            {
                player.AddCard(deck[deck.Count - 1]);
                deck[deck.Count - 1].GetComponent<Card>().SetCardOwner(CARD_OWNER.PLAYER);
                deck[deck.Count - 1].transform.parent = player.gameObject.transform;
                //Debug.Log(deck[deck.Count - 1].gameObject.name);
                deck.RemoveAt(deck.Count - 1);
            }
            if(enemy)
            {
                enemy.AddCard(deck[deck.Count - 1]);
                deck[deck.Count - 1].GetComponent<Card>().SetCardOwner(CARD_OWNER.ENEMY);
                deck[deck.Count - 1].transform.parent = enemy.gameObject.transform;
                //Debug.Log(deck[deck.Count - 1].gameObject.name);
                deck.RemoveAt(deck.Count - 1);
            }
        }
        SetCntText();
    }
    void ShuffleCard()
    {
        for (int i = 0; i < deck.Count - 1; i++)
        {
            int random = Random.Range(i + 1, deck.Count);
            GameObject temp = deck[i];
            deck[i] = deck[random];
            deck[random] = temp;
        }
    }
    public int GetCurrNum()
    {
        return currCardNum;
    }
    public int GetCurrCardDam()
    {
        return currCardDam;
    }
    public CARD_TYPE GetCurrCardType()
    {
        return currType;
    }
    public int GetCurrTurn()
    {
        return currPlayerTurn;
    }
    void SetPlayerTurn(int num = 0)
    {
        if(num != 11 && num != 12 && num <= 13)
            currPlayerTurn = (currPlayerTurn + 1) % 2;
        else if(num == 14)
        {
            tempTurn = currPlayerTurn;
            currPlayerTurn = 3;
        }
        else if(num == 15)
        {
            currPlayerTurn = (tempTurn + 1) % 2;
        }

        player.SetPlayerTurn();
        enemy.SetEnemyTurn();
    }
    public void ReceiveCardFromPlayer(GameObject card)
    {
        usedCardList[usedCardList.Count - 1].transform.position = usedCardPos + new Vector3(0.0f,0.0f,1.0f);
        usedCardList.Add(card);
        card.GetComponent<Card>().SetCardOwner(CARD_OWNER.DECK);
        currCardNum = card.GetComponent<Card>().GetCardNum();
        currType = card.GetComponent<Card>().GetCardType();
        currCardDam = card.GetComponent<Card>().GetCardDamage();
        sumCardDam += currCardDam;
        card.transform.parent = this.transform;
        card.transform.position = usedCardPos;
        if(currCardNum == 7)
        {
            ui7.gameObject.SetActive(true);
            SetPlayerTurn(14);
            return;
        }

        SetCurrentTypeText();
        SetTotalDamText();

        //Debug.Log(player.playerDeck.Count);
        if(player.playerDeck.Count == 0)
        {
            Debug.Log("Player Win!");
            player.gameObject.SetActive(false);
        }
        if(enemy.enemyDeck.Count == 0)
        {
            Debug.Log("Enemy Win!");
            enemy.gameObject.SetActive(false);
        }

        SetPlayerTurn(currCardNum);
    }
    public void SendCardToPlayer()
    {
        if(sumCardDam == 0)
        {
            sumCardDam++;
        }
        while (sumCardDam > 0)
        {
            if (currPlayerTurn == 0)
            {
                player.AddCard(deck[deck.Count - 1]);
                deck[deck.Count - 1].GetComponent<Card>().SetCardOwner(CARD_OWNER.PLAYER);
                deck[deck.Count - 1].transform.parent = player.gameObject.transform;
            }
            else
            {
                enemy.AddCard(deck[deck.Count - 1]);
                deck[deck.Count - 1].GetComponent<Card>().SetCardOwner(CARD_OWNER.ENEMY);
                deck[deck.Count - 1].transform.parent = enemy.gameObject.transform;
            }
            deck.RemoveAt(deck.Count - 1);

            if (deck.Count == 0)
            {
                MoveCardToDeck();
            }
            sumCardDam--;
        }
        SetPlayerTurn();
        ClearCardDam();
        SetCntText();
    }
    void MoveCardToDeck()
    {
        Debug.Log("덱으로 돌아간다");
        int i = 0;
        while (usedCardList.Count > 1)
        {
            deck.Add(usedCardList[i]);
            usedCardList[i].GetComponent<Card>().SetCardOwner(CARD_OWNER.DECK);
            usedCardList[i].transform.position = deckPos;
            usedCardList.RemoveAt(i);
            //i++;
        }
        ShuffleCard();
    }
    public void ClearCardDam()
    {
        currCardDam = 0;
        sumCardDam = 0;
        SetTotalDamText();
    }
    void SetCntText()
    {
        string tmp = "카드 수 : ";
        tmp += deck.Count.ToString();
        boardCnt.text = tmp;
    }
    void SetCurrentTypeText()
    {
        string tmp = "";
        switch(currType)
        {
            case CARD_TYPE.CLOVER:
                tmp += "CLOVER";
                break;
            case CARD_TYPE.DIAMOND:
                tmp += "DIAMOND";
                break;
            case CARD_TYPE.HEART:
                tmp += "HEART";
                break;
            case CARD_TYPE.SPADE:
                tmp += "SPADE";
                break;
            case CARD_TYPE.JOKER:
                tmp += "JOKER";
                break;
        }
        currTypeText.text = tmp;
    }
    void SetTotalDamText()
    {
        string tmp = "누적 데미지 : ";
        tmp += sumCardDam.ToString();
        totalDamText.text = tmp;
    }
    public void SetCloverType()
    {
        currType = CARD_TYPE.CLOVER;
        ui7.gameObject.SetActive(false);
        SetPlayerTurn(15);
        SetCurrentTypeText();
    }
    public void SetDiamondType()
    {
        currType = CARD_TYPE.DIAMOND;
        ui7.gameObject.SetActive(false);
        SetPlayerTurn(15);
        SetCurrentTypeText();
    }
    public void SetHeartType()
    {
        currType = CARD_TYPE.HEART;
        ui7.gameObject.SetActive(false);
        SetPlayerTurn(15);
        SetCurrentTypeText();
    }
    public void SetSpadeType()
    {
        currType = CARD_TYPE.SPADE;
        ui7.gameObject.SetActive(false);
        SetPlayerTurn(15);
        SetCurrentTypeText();
    }
}
