using System;
using Hackathon.RE.SUI.API_Method;
using Hackathon.RE.SUI.Wallet;
using MFPS.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sp_SUIWalletUI : MonoBehaviour
{
    public TextMeshProUGUI walletAddress;
    public GameObject walletContent;

    private void Start(){
        walletAddress.text = sp_SUIWalletInfo.WalletPublicAddress;
    }

    public void OnDisconnect(){
        if (bl_DataBase.Instance != null){
            bl_DataBase.Instance.LocalUser.ShopData = new bl_ShopUserData();
        }

        SceneManager.LoadScene("SUI");
    }

    public void ViewNfTs(){
        walletContent.SetActive(true);
    }
}