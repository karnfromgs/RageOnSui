using System;
using System.Collections.Generic;
using System.Linq;
using MFPSEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hackathon.RE.SUI.Wallet
{
    [CreateAssetMenu(fileName = "NFT Infos")]
    public class sp_SUINFTInfo : ScriptableObject
    {
        [Header("Geo Tagged NFT")] public List<GeoNft> geoTagNfts;
        [Header("Dynamic NFT")] public List<DynamicNft> dynamicNfts;
        [Header("Collectable NFT")] public List<CollectableNft> collectableNfts;

        [Serializable]
        public class Nft
        {
            [StringInList(typeof(WeatherPropertyDrawer), "AllGlobalCamos")]
            public string nftName;
            public string nftAddress;
        }

        [Serializable]
        public class NftCamoInfo
        {
            public string countryName;
            [SpritePreview(50)] public Texture2D overridePreview;
            public Material material;
            public string url;
        }

        [Serializable]
        public class CollectableNft
        {
            public Nft collectableNft;
            public int coinToAdd = 100;
            [MFPSCoinID] public int coinToApply;
        }

        [Serializable]
        public class GeoNft
        {
            public Nft nft;
            public List<NftCamoInfo> geoNft;

            [FormerlySerializedAs("defaultGeoNft")]
            public NftCamoInfo defaultNft;

            public Texture2D GeoTagNftTexture{
                get{
                    var matchedItem = geoNft.SingleOrDefault(x => x.countryName.Equals(WhatsMyIP.Country));
                    if (matchedItem != null){
                        return matchedItem.overridePreview;
                    }

                    return defaultNft.overridePreview;
                }
            }

            public Material GeoTagNftMaterial{
                get{
                    var matchedItem = geoNft.SingleOrDefault(x => x.countryName.Equals(WhatsMyIP.Country));
                    if (matchedItem != null){
                        return matchedItem.material;
                    }

                    return defaultNft.material;
                }
            }
        }

        [Serializable]
        public class DynamicNft
        {
            public Nft nft;
            public List<NftCamoInfo> dynamicNft;
        }


        public static sp_SUINFTInfo Instance => Resources.Load<sp_SUINFTInfo>("NFT Infos");
    }
}