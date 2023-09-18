using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hackathon.RE.SUI.API_Method
{
    public class SUIWWalletResponse : MonoBehaviour
    {
        public static List<string> NftsInWallet = new List<string>();
        public static List<DynamicNftData> DynamicNftsData = new List<DynamicNftData>();
        public static List<CollectableNftData> ColletableNftsData = new List<CollectableNftData>();

        [Serializable]
        public class DynamicNftData
        {
            public string nftName;
            public string nftAddress;
            public string metadataUrl;
            public JObject Metadata;
        }
        
        [Serializable]
        public class CollectableNftData
        {
            public string nftName;
            public string nftAddress;
            public string metadataUrl;
            public JObject Metadata;
        }


    
    }
}