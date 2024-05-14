using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Shop : MonoBehaviour
{
    [SerializeField] Button[] buyButtons;
    [SerializeField] TextMeshProUGUI[] boughtTexts;
    [SerializeField] int[] prices;

    [SerializeField] GameObject shopPanel;

    // public delegate void BuySecondPosition();
    // public event BuySecondPosition buySecondPosition;

    public static Shop instance;

    private void Awake()
    {
        instance = this;
    }

    

    private void Start()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            if(!PlayerPrefs.HasKey("Position" + i))
            {
            PlayerPrefs.SetInt("Position" + 1, 0);
            }
            else
            {
                if(PlayerPrefs.GetInt("Position" + i) == 1)
                {
                    buyButtons[i].interactable = false;
                    boughtTexts[i].text = "Bought";

                    // if(i == 2) buySecondPosition.Invoke();
                }
            }
        }
        Check();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            shopPanel.SetActive(!shopPanel.activeInHierarchy);
            Check();

            if(shopPanel.activeInHierarchy) Time.timeScale = 0;
            else Time.timeScale = 1;

        } 
    }

    private void OnEnable()
    {
        Check();
    }

    void Check()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            // if(PlayerPrefs.GetInt("Position" + i) == 1) break;

            if(Player.instance.currentMoney < prices[i])
            {
                buyButtons[i].interactable = false;
                boughtTexts[i].text = "Few coins";
            }
            else
            {
                buyButtons[i].interactable = true;
                boughtTexts[i].text = "Buy";
            }

            if(PlayerPrefs.GetInt("Position" + i) == 1)
            {
                buyButtons[i].interactable = false;
                boughtTexts[i].text = "Bought";
            }

        }
    }

    public void Buy(int index)
    {
        buyButtons[index].interactable = false;
        boughtTexts[index].text = "Bought";

        PlayerPrefs.SetInt("Position" + index, 1);

        Player.instance.AddMoney(-prices[index]);

        Check();

        // if(index == 2) buySecondPosition.Invoke();
    }

[ContextMenu("Delete PLayer Prefs")]
    void DeletePlayerPrefs() => PlayerPrefs.DeleteAll();
}
