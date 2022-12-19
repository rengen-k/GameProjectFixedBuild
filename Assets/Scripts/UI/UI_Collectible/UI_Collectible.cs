using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Collectible : MonoBehaviour
{
    private TextMeshProUGUI collectibleText;
    private GameObject globalGameState;

    private void Awake()
    {

        globalGameState = GameObject.Find("GlobalGameState");
        collectibleText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        collectibleText.text = globalGameState.GetComponent<GameState>().thisLevelCollectibles.ToString();
    }
}
