using Hackathon.RE.SUI.API_Method;
using UnityEngine;

namespace Hackathon.RE.SUI.Wallet
{
    public class sp_SUIWalletManager : MonoBehaviour
    {
        public GameObject card;
        public GameObject content;

        async void Start(){
            foreach (var walletDetail in sp_SUIWalletInfo.Instance.walletDetails){
                GameObject instantiate = Instantiate(card, content.transform, false);
                sp_SUIWalletItem spSuiWalletInfo = instantiate.GetComponent<sp_SUIWalletItem>();
                spSuiWalletInfo.Setup(walletDetail);
            }
            
            /*Debug.Log(await SUIAPIMethods.MintNft("Rage Effect Dynamic NFT",
                "This is a test Unlimited Coin Glitch NFT",
                "http://localhost/nft/image/logo.png",
                "http://localhost/nft/script/metadata/glitch_details.json",
                "0xab34e27430c9216a30080ed95c42e2432a87706fb186330abb5a3c244e16fa99",
                "0xab34e27430c9216a30080ed95c42e2432a87706fb186330abb5a3c244e16fa99"));*/
        }
    }
}