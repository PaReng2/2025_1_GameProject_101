using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject cardPrefab;
    public Sprite[] cardimages;

    public Transform deckArea;
    public Transform ganArea;

    public Button drawButton;
    public TextMeshProUGUI deckCountText;

    public float cardSpacing = 2.0f;
    public int maxGandSize = 6;

    public GameObject[] handCards;
    public int deckCount;

    public GameObject[] deckCards;
    public int handCount;
    // Start is called before the first frame update
    public int[] prefebdinedDeck = new int[]
        {
            1,1,1,1,1,1,1,1,
            2,2,2,2,2,2,
            3,3,3,3,
            4,4
        };





    void Start()
    {
        deckCards = new GameObject[prefebdinedDeck.Length];
        handCards = new GameObject[handCount];

        InitializeDeck();
        shuffleDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void shuffleDeck()
    {
        for (int i = 0; i < deckCount - 1; i++)
        {
            int j = Random.Range(i, deckCount);
            
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }

    void InitializeDeck()
    {
        deckCount = prefebdinedDeck.Length;

        for (int I = 0; I < prefebdinedDeck.Length; I++)
        {
            int value = prefebdinedDeck[I];
            int imageIndex = value - 1;
            if (imageIndex >= cardimages.Length || imageIndex < 0)
            {
                imageIndex = 0;
            }
            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(false);

            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null)
            {
                cardComp.InitCard(value, cardimages[imageIndex]);
            }
            deckCards[I] = newCardObj;
        }
    }
}
