using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_KeyPanel : MonoBehaviour
{
    [SerializeField] private KeyHolder keyHolder;
    private Transform container;
    private Transform keyTemplate;

    private void Awake()
    {
        container = transform.Find("Container");
        keyTemplate = container.Find("KeyTemplate");
        keyTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;
    }

    private void KeyHolder_OnKeysChanged(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container) 
        {
            if (child == keyTemplate) continue;
            Destroy(child.gameObject);
        }
        
        List<Key.KeyType> keyList = keyHolder.GetKeyList();
        for (int i = 0; i < keyList.Count; i++)
        {
            Key.KeyType keyType = keyList[i];
            Transform keyTransform = Instantiate(keyTemplate, container);
            keyTransform.gameObject.SetActive(true);
            keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(100 * i, 0);
            Image keyImage = keyTransform.Find("Image").GetComponent<Image>();
            switch (keyType) {
                default:
                case Key.KeyType.Red:
                    keyImage.color = new Color32(255,0,0,255);
                    break;
                case Key.KeyType.Green:
                    keyImage.color = new Color32(67,255,0,255);
                    break;
                case Key.KeyType.Blue:
                    keyImage.color = new Color32(0,16,225,255);
                    break;
                case Key.KeyType.Purple:
                    keyImage.color = new Color32(255,0,230,255);
                    break;
            }
        }
    }
}
