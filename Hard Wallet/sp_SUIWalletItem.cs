using Hackathon.RE.SUI.API_Method;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hackathon.RE.SUI.Wallet
{
    public class sp_SUIWalletItem : MonoBehaviour
    {
        public Image Image;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI PublicAddress;

        private sp_SUIWalletInfo.WalletDetails _walletInfo;

        public void Setup(sp_SUIWalletInfo.WalletDetails walletInfo){
            _walletInfo = walletInfo;
            Name.text = _walletInfo.name;
            PublicAddress.text = _walletInfo.PublicAddress;
            Image.sprite = _walletInfo.sprite;
            Image.color = _walletInfo.color;
        }


        public async void OnPointerClick(){
            SUIWWalletResponse.NftsInWallet.Clear();

            sp_SuiUI.Instance.loading.SetActive(true);

            sp_SUIWalletInfo.WalletPublicAddress = _walletInfo.PublicAddress;
            sp_SUIWalletInfo.WalletPrivateAddress = _walletInfo.PrivateAddress;
            string response = await SUIAPIMethods.FetchNftFromWallet(sp_SUIWalletInfo.WalletPublicAddress);
            sp_SuiUI.Instance.loading.SetActive(false);

            if (response.Contains("Error")){
                sp_SuiUI.Instance.error.SetActive(true);
            }
            else{
                JObject result = JObject.Parse(response);
                Debug.Log(result);
                if (result["status"]!.ToString().Equals("success")){
                    JArray data = (JArray)result["data"]!;
                    foreach (var jToken in data){
                        var nftObject = (JObject)jToken;
                        SUIWWalletResponse.NftsInWallet.Add(nftObject["id"]?.ToString());
                    }

                    if (sp_DynamicNFTManager.Instance) sp_DynamicNFTManager.Instance.Init();
                    if (sp_GeoNFTManager.Instance) sp_GeoNFTManager.Instance.Init();

                    sp_SuiUI.Instance.OpenWindow("wallet-content");
                }
                else{
                    sp_SuiUI.Instance.error.SetActive(true);
                }
            }
        }
    }
}