using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CARD_TYPE
{
    NONE = 0,
    SPADE,
    HEART,
    DIAMOND,
    CLOVER,
    JOKER
};
public enum CARD_OWNER
{
    DECK,
    USED,
    PLAYER,
    ENEMY
};
public class Card : MonoBehaviour
{
    public CARD_TYPE m_Type;
    public int m_Number;
    public int m_Damage;
    public CARD_OWNER m_Owner;
    private SpriteRenderer spriteRenderer;
    private PlayerCtrl player;
    private EnemyCtrl enemy;
    private BoardCtrl board;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerCtrl>();
        enemy = FindObjectOfType<EnemyCtrl>();
        board = FindObjectOfType<BoardCtrl>();
        m_Owner = CARD_OWNER.DECK;
        //m_Type = CARD_TYPE.NONE;
        //m_Number = m_Damage = 0;
    }
    public void SetCard(CARD_TYPE type, int number)
    {
        m_Type = type;
        m_Number = number;
        if(number == 1) // A카드
        {
            m_Damage = 4;
        }
        if(number == 2)
        {
            m_Damage = 2;
        }
        if(type == CARD_TYPE.JOKER)
        {
            m_Damage = 7;
        }
    }
    public CARD_TYPE GetCardType()
    {
        return m_Type;
    }
    public int GetCardNum()
    {
        return m_Number;
    }
    public int GetCardDamage()
    {
        return m_Damage;
    }
    public void SetCardOwner(CARD_OWNER owner)
    {
        Debug.Log(owner);
        m_Owner = owner;
    }
    public CARD_OWNER GetCardOwner()
    {
        return m_Owner;
    }
    private void OnMouseDown()
    {
        Debug.Log(m_Type + " " + m_Number + " " + m_Owner);
        //switch (m_Owner)
        //{
        //    //case CARD_OWNER.DECK:
        //    //    if (board)
        //    //        board.SendCardToPlayer();
        //    //    break;
        //    case CARD_OWNER.PLAYER:
        //        if (player)
        //            player.SendCardFromDeck(this.gameObject);
        //        break;
        //    case CARD_OWNER.ENEMY:
        //        if (enemy)
        //            enemy.SendCardFromDeck(this.gameObject);
        //        break;
        //}
        string tmp = transform.parent.name;
        if(tmp.Equals("Player"))
        {
            player.SendCardFromDeck(this.gameObject);
        }
        if (tmp.Equals("Enemy"))
        {
            enemy.SendCardFromDeck(this.gameObject);
        }
        if (tmp.Equals("Board"))
        {
            board.SendCardToPlayer();
        }
    }
}
