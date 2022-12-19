using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Collectible : MonoBehaviour
{
    // [SerializeField] private KeyHolder keyHolder;
    private int collectibleNum;
    private TextMeshProUGUI collectibleText;
    private GameObject globalGameState;
    private Transform container;
    private Transform collectibleTemplate;

    private void Awake()
    {

        globalGameState = GameObject.Find("GlobalGameState");
        collectibleText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        collectibleText.text = "" + globalGameState.GetComponent<GameState>().thisLevelCollectibles;
    }
}
