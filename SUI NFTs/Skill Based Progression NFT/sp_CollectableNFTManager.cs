using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hackathon.RE.SUI.API_Method;
using Hackathon.RE.SUI.Wallet;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sp_CollectableNFTManager : MonoBehaviour
{
    public GameObject item;
    public GameObject noNft;
    public GameObject itemParent;
    public static sp_CollectableNFTManager Instance = null;
    public GameObject loading;
    private int count;

    private void Awake(){
        if (Instance == null){
            Instance = this;
        }
    }

    public void Start(){
        Init();
    }

    void DeleteOldUI(){
        for (int i = 1; i < itemParent.transform.childCount; i++){
            Destroy(itemParent.transform.GetChild(i).gameObject);
        }
    }

    void NoNftMessage(){
        TextMeshProUGUI noNFT = Instantiate(this.noNft, itemParent.transform, false)
            .GetComponent<TextMeshProUGUI>();
        noNFT.text = "NO ANY COLLECTABLE NFT IN THE WALLET";
    }

    public async void Init(){
        count = 0;
        DeleteOldUI();
        var allToken = SUIWWalletResponse.NftsInWallet;
        var allCollectableNfTs = sp_SUINFTInfo.Instance.collectableNfts;
        SUIWWalletResponse.ColletableNftsData.Clear();

        if (allCollectableNfTs.Count == 0 || allToken.Count == 0){
            NoNftMessage();
            return;
        }

        loading.SetActive(true);
        sp_SuiUI.Instance.loading.SetActive(true);

        if (sp_SuiUI.Instance.IsLoading.ContainsKey("collectable")){
            sp_SuiUI.Instance.IsLoading["collectable"] = true;
        }
        else{
            sp_SuiUI.Instance.IsLoading.Add("collectable", true);
        }

        foreach (var nft in allCollectableNfTs){
            if (allToken.Contains(nft.collectableNft.nftAddress)){
                var globalID = bl_CustomizerData.Instance.GetGlobalCamoID(nft.collectableNft.nftName);

                if (bl_DataBase.Instance != null && !sp_SuiUI.Instance.viewWallet){
                    bl_DataBase.Instance.LocalUser.ShopData.AddPurchase(new bl_ShopPurchase(){
                        TypeID = (int)ShopItemType.WeaponCamo,
                        ID = globalID
                    });
                }

                string nftMetadataUrl =
                    SUIAPIMethods.FetchMetadataUrl(await SUIAPIMethods.GetNftInfo(nft.collectableNft.nftAddress));
                JObject metaData = await SUIAPIMethods.GetMetaData(nftMetadataUrl);

                SUIWWalletResponse.ColletableNftsData.Add(new SUIWWalletResponse.CollectableNftData{
                    nftName = nft.collectableNft.nftName,
                    nftAddress = nft.collectableNft.nftAddress,
                    metadataUrl = nftMetadataUrl,
                    Metadata = metaData
                });

                GameObject item = Instantiate(this.item, itemParent.transform, false);
                TextMeshProUGUI info = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                RawImage icon = item.transform.GetChild(1).GetComponent<RawImage>();
                item.SetActive(true);

                icon.texture = bl_CustomizerData.Instance.GlobalCamos[globalID].Preview;
                info.text =
                    $"Status: {((bool)metaData["claimed"] ? "<color=\"green\">Claimed</color>" : "<color=\"red\">Unclaimed</color>")}";
                count++;
            }
            
        }

        if (count == 0){
            NoNftMessage();
        }
        
        loading.SetActive(false);

        sp_SuiUI.Instance.IsLoading["collectable"] = false;
            
        /*if (sp_SuiUI.Instance.IsLoading.ContainsKey("geo") && sp_SuiUI.Instance.IsLoading["geo"] == false &&
            sp_SuiUI.Instance.IsLoading.ContainsKey("dynamic") && sp_SuiUI.Instance.IsLoading["dynamic"] == false){
            sp_SuiUI.Instance.loading.SetActive(false);
        }*/
            
        if (sp_SuiUI.Instance.IsLoading.ContainsKey("dynamic") && sp_SuiUI.Instance.IsLoading["dynamic"] == false){
            sp_SuiUI.Instance.loading.SetActive(false);
        }
    }
}