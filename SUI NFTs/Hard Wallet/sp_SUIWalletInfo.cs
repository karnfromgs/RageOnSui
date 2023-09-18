using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hackathon.RE.SUI.Wallet
{
    [CreateAssetMenu(fileName = "Wallet Infos")]
    public class sp_SUIWalletInfo : ScriptableObject
    {

        public static sp_SUIWalletInfo Instance => Resources.Load<sp_SUIWalletInfo>("Wallet Infos");
    
        [Serializable]
        public class WalletDetails
        {
            public string name;
            public Sprite sprite;
            public Color color = Color.white;
            [HideInInspector][SerializeField] private string publicKey = "";
            [HideInInspector][SerializeField] private string privateKey = "";

            public string PublicAddress{
                get => publicKey;
                set => publicKey = value;
            }

            public string PrivateAddress{
                get => privateKey;
                set => privateKey = value;
            }
        }

        public List<WalletDetails> walletDetails;

        public static string WalletPublicAddress{
            get => PlayerPrefs.GetString("wallet-public-address", String.Empty);
            set => PlayerPrefs.SetString("wallet-public-address", value);
        }

        public static string WalletPrivateAddress{
            get => PlayerPrefs.GetString("wallet-private-address", String.Empty);
            set => PlayerPrefs.SetString("wallet-private-address", value);
        }
    }
}