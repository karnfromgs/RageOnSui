using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.RE.SUI.API_Method;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hackathon.RE.SUI.Wallet
{
    public class sp_GeoNFTManager : MonoBehaviour
    {
        public static sp_GeoNFTManager Instance = null;

        public GameObject noNft;
        public GameObject item;
        public GameObject itemParent;
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
            noNFT.text = "NO ANY GEO TAGGED NFT IN THE WALLET";
        }

        // Start is called before the first frame update
        public async void Init(){
            count = 0;
            DeleteOldUI();
            var allToken = SUIWWalletResponse.NftsInWallet;
            var allGeoNfTs = sp_SUINFTInfo.Instance.geoTagNfts;

            if (allGeoNfTs.Count == 0 || allToken.Count == 0){
                NoNftMessage();
                return;
            }

            loading.SetActive(true);
            sp_SuiUI.Instance.loading.SetActive(true);

            if (sp_SuiUI.Instance.IsLoading.ContainsKey("geo")){
                sp_SuiUI.Instance.IsLoading["geo"] = true;
            }
            else{
                sp_SuiUI.Instance.IsLoading.Add("geo", true);
            }

            await WhatsMyIP.GetCurrentCountryCode();

            foreach (var nft in allGeoNfTs){
                if (allToken.Contains(nft.nft.nftAddress)){
                    if (bl_DataBase.Instance != null && !sp_SuiUI.Instance.viewWallet){
                        bl_DataBase.Instance.LocalUser.ShopData.AddPurchase(new bl_ShopPurchase(){
                            TypeID = (int)ShopItemType.WeaponCamo,
                            ID = bl_CustomizerData.Instance.GetGlobalCamoID(nft.nft.nftName)
                        });
                    }

                    GameObject item = Instantiate(this.item, itemParent.transform, false);
                    TextMeshProUGUI info = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    RawImage icon = item.transform.GetChild(1).GetComponent<RawImage>();
                    item.SetActive(true);

                    var nftCamoInfo = nft.geoNft.SingleOrDefault(x => x.countryName.Equals(WhatsMyIP.Country)) ??
                                      nft.defaultNft;

                    icon.texture = nftCamoInfo.overridePreview;
                    info.text = $"Country : {WhatsMyIP.Country}";
                    count++;
                }
            }

            if (count == 0){
                NoNftMessage();
            }
            
            loading.SetActive(false);
            
            sp_SuiUI.Instance.IsLoading["geo"] = false;
            
            if (sp_SuiUI.Instance.IsLoading.ContainsKey("dynamic") && sp_SuiUI.Instance.IsLoading["dynamic"] == false &&
                sp_SuiUI.Instance.IsLoading.ContainsKey("collectable") &&
                sp_SuiUI.Instance.IsLoading["collectable"] == false){
                
                sp_SuiUI.Instance.loading.SetActive(false);
            }
        }
    }
}