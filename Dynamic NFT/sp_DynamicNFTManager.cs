using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.RE.SUI.API_Method;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hackathon.RE.SUI.Wallet

{
    public class sp_DynamicNFTManager : MonoBehaviour
    {
        public GameObject item;
        public GameObject noNft;
        public GameObject itemParent;
        public static sp_DynamicNFTManager Instance = null;
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
            noNFT.text = "NO ANY DYNAMIC NFT IN THE WALLET";
        }

        public async void Init(){
            count = 0;
            DeleteOldUI();
            var allToken = SUIWWalletResponse.NftsInWallet;
            var allDynamicNfTs = sp_SUINFTInfo.Instance.dynamicNfts;
            SUIWWalletResponse.DynamicNftsData.Clear();

            if (allDynamicNfTs.Count == 0 || allToken.Count == 0){
                NoNftMessage();
                return;
            }

            loading.SetActive(true);
            sp_SuiUI.Instance.loading.SetActive(true);
            
            if (sp_SuiUI.Instance.IsLoading.ContainsKey("dynamic")){
                sp_SuiUI.Instance.IsLoading["dynamic"] = true;
            }
            else{
                sp_SuiUI.Instance.IsLoading.Add("dynamic", true);
            }

            foreach (var nft in allDynamicNfTs){
                if (allToken.Contains(nft.nft.nftAddress)){
                    if (bl_DataBase.Instance != null && !sp_SuiUI.Instance.viewWallet){
                        bl_DataBase.Instance.LocalUser.ShopData.AddPurchase(new bl_ShopPurchase(){
                            TypeID = (int)ShopItemType.WeaponCamo,
                            ID = bl_CustomizerData.Instance.GetGlobalCamoID(nft.nft.nftName)
                        });
                    }

                    string nftMetadataUrl =
                        SUIAPIMethods.FetchMetadataUrl(await SUIAPIMethods.GetNftInfo(nft.nft.nftAddress));
                    JObject metaData = await SUIAPIMethods.GetMetaData(nftMetadataUrl);

                    SUIWWalletResponse.DynamicNftsData.Add(new SUIWWalletResponse.DynamicNftData{
                        nftName = nft.nft.nftName,
                        nftAddress = nft.nft.nftAddress,
                        metadataUrl = nftMetadataUrl,
                        Metadata = metaData
                    });

                    GameObject item = Instantiate(this.item, itemParent.transform, false);
                    TextMeshProUGUI info = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    RawImage icon = item.transform.GetChild(1).GetComponent<RawImage>();
                    item.SetActive(true);

                    var nftCamoInfo = nft.dynamicNft.Single(x => x.countryName.Equals(metaData["type"]!.ToString()));
                    icon.texture = nftCamoInfo.overridePreview;
                    info.text =
                        $"Type: {metaData["type"]!}\nObjective: {ConvertTime((int)metaData["playtime_required"]!)}\nProgress: {ConvertTime((int)metaData["total_playtime"]!)}";
                    count++;
                }
            }
            
            
            if (count == 0){
                NoNftMessage();
            }

            loading.SetActive(false);

            sp_SuiUI.Instance.IsLoading["dynamic"] = false;
            
            /*if (sp_SuiUI.Instance.IsLoading.ContainsKey("geo") && sp_SuiUI.Instance.IsLoading["geo"] == false &&
                sp_SuiUI.Instance.IsLoading.ContainsKey("collectable") && sp_SuiUI.Instance.IsLoading["collectable"] == false){
                sp_SuiUI.Instance.loading.SetActive(false);
            }*/
                
            if (sp_SuiUI.Instance.IsLoading.ContainsKey("collectable") && sp_SuiUI.Instance.IsLoading["collectable"] == false){
                sp_SuiUI.Instance.loading.SetActive(false);
            }
        }

        public string ConvertTime(int inputSeconds){
            TimeSpan time = TimeSpan.FromSeconds(inputSeconds);

            if (time.TotalSeconds == 0)
                return "0s";

            string output = "";

            if (time.Hours > 0)
                output += $"{time.Hours}h ";

            if (time.Minutes > 0)
                output += $"{time.Minutes}m ";

            if (time.Seconds > 0)
                output += $"{time.Seconds}s";

            return output.Trim();
        }
    }
}