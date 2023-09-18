using System.Collections.Generic;
using System.Linq;
using Hackathon.RE.SUI.API_Method;
using Hackathon.RE.SUI.Wallet;
using MFPS.Addon.Customizer;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class sp_CollectableItem : MonoBehaviour
{
    public Renderer[] renderers;

    public bool isClaimed = false;
    private int _coinType;
    private int _totalCoin;
    private string _metadataUrl;
    private sp_CollectableNFTHandler _collectableNftHandler;

    private async void OnTriggerEnter(Collider other){
        if (other.tag.Equals("Player") && !isClaimed){
            isClaimed = true;

            if (bl_DataBase.Instance != null){
                bl_DataBase.Instance.SaveNewCoins(_totalCoin,
                    _coinType);
                _collectableNftHandler.ShowMessage(_totalCoin);
            }
            else{
                Debug.Log("Save New Coins");
            }

            var updatedMetadata = new JObject{
                ["claimed"] = true
            };

            Debug.Log(await SUIAPIMethods.UpdateJsonFile(
                _metadataUrl.Substring("https://api.rageeffect.io/php/nft/script/".Length), updatedMetadata));

            Destroy(gameObject);
        }
    }

    public void Init(GlobalCamo globalCamo, string metadataUrl, sp_CollectableNFTHandler collectableNftHandler){
        this._metadataUrl = metadataUrl;
        this._collectableNftHandler = collectableNftHandler;

        List<sp_SUINFTInfo.CollectableNft> allCollectableNfTs = sp_SUINFTInfo.Instance.collectableNfts;
        sp_SUINFTInfo.CollectableNft collectableNfT =
            allCollectableNfTs.Single(x => x.collectableNft.nftName.Equals(globalCamo.Name));

        _totalCoin = collectableNfT.coinToAdd;
        _coinType = collectableNfT.coinToApply;

        foreach (Renderer renderer in renderers){
            renderer.material.mainTexture = globalCamo.Preview;
        }
    }
}