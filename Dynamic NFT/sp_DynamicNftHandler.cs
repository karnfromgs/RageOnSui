using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.RE.SUI.Wallet;
using MFPS.Addon.GameResumePro;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Hackathon.RE.SUI.API_Method
{
    public class sp_DynamicNftHandler : MonoBehaviour
    {
        public bl_GameResumeProUI gameResumeProUI;
        public GameObject loading;

        public static sp_DynamicNftHandler Instance;
        public Dictionary<string, DateTime> TimeInfos = new Dictionary<string, DateTime>();

        private void Awake(){
            if (Instance == null){
                Instance = this;
            }
        }

        private async Task RefreshNftData(){
            var allToken = SUIWWalletResponse.NftsInWallet;
            var allGeoNfTs = sp_SUINFTInfo.Instance.geoTagNfts;
            var allDynamicNfTs = sp_SUINFTInfo.Instance.dynamicNfts;

            if (allToken.Count == 0) return;

            /*if (allGeoNfTs.Count > 0){
                Debug.Log(await WhatsMyIP.GetCurrentCountryCode());
            }*/

            if (allDynamicNfTs.Count > 0){
                SUIWWalletResponse.DynamicNftsData.Clear();

                foreach (var nft in allDynamicNfTs){
                    if (allToken.Contains(nft.nft.nftAddress)){
                        string nftMetadataUrl =
                            SUIAPIMethods.FetchMetadataUrl(await SUIAPIMethods.GetNftInfo(nft.nft.nftAddress));
                        JObject metaData = await SUIAPIMethods.GetMetaData(nftMetadataUrl);

                        SUIWWalletResponse.DynamicNftsData.Add(new SUIWWalletResponse.DynamicNftData{
                            nftName = nft.nft.nftName,
                            nftAddress = nft.nft.nftAddress,
                            metadataUrl = nftMetadataUrl,
                            Metadata = metaData
                        });
                    }
                }
            }

            Debug.Log("NFT Data Refreshed..");
        }

        public async void OnLeaveRoomOrGameOver(){
            loading.SetActive(true);

            var utcNow = DateTime.UtcNow;

            foreach (var dateTime in TimeInfos){
                int totalSeconds = (int)(utcNow - dateTime.Value).TotalSeconds;
                var dynamicNftData = SUIWWalletResponse.DynamicNftsData.Single(x => x.nftName.Equals(dateTime.Key));
                var metadata = dynamicNftData.Metadata;
                string type = (totalSeconds + (int)metadata["total_playtime"]) >= (int)metadata["playtime_required"]
                    ? "legendary"
                    : "rare";
                string url = String.Empty;

                foreach (var dynamicNft in sp_SUINFTInfo.Instance.dynamicNfts){
                    foreach (var nftCamoInfo in dynamicNft.dynamicNft){
                        if (nftCamoInfo.countryName.Equals(type)){
                            url = nftCamoInfo.url;
                            break;
                        }
                    }
                }

                var updatedMetadata = new JObject{
                    ["playtime_required"] = metadata["playtime_required"],
                    ["type"] = type,
                    ["image_url"] = url,
                    ["total_playtime"] = (int)metadata["total_playtime"] + totalSeconds
                };

                Debug.Log(await SUIAPIMethods.UpdateJsonFile(
                    dynamicNftData.metadataUrl.Substring("https://api.rageeffect.io/php/nft/script/".Length), updatedMetadata));
            }

            if (TimeInfos.Count != 0){
                await RefreshNftData();
            }

            loading.SetActive(false);

            if (bl_GameManager.Instance.GameFinish){
                gameResumeProUI.LeaveRoomAfterSafeMetadat();
            }
            else{
                bl_UIReferences.Instance.LeaveRoomAfterSafeMetadat();
            }
        }
    }
}