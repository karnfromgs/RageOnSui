using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Hackathon.RE.SUI.API_Method
{
    public class SUIAPIMethods : MonoBehaviour
    {
        static readonly string apiKey = "mw_testX2TA7hQAeLIyPrVd19tY3LwcTV1GWoh7dYn";
        private const string MintNftURL = "https://api-staging.mirrorworld.fun/v2/sui/testnet/asset/rage-effect/mint";
        private const string NftInWalletURL = "https://api.mirrorworld.fun/v2/sui/testnet/asset/nft/owner";
        private const string JsonUpdateURL = "http://api.rageeffect.io/php//nft/script//update_metadata.php";

        private const string NftInfoURL =
            "https://api-staging.mirrorworld.fun/v2/sui/testnet/asset/rage-effect/find/";

        public static async Task<JObject> GetMetaData(string metadataURL){
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(metadataURL);
            response.EnsureSuccessStatusCode();
            string result =  await response.Content.ReadAsStringAsync();
            return JObject.Parse(result);
        }
        
        public static string FetchMetadataUrl(string response){
            JObject result = JObject.Parse(response);
            return result["data"]![0]!["metadata_url"]!.ToString();
        }

        public static async Task<string> UpdateJsonFile(string metadataLink, JObject updatedContent){
            using HttpClient client = new HttpClient();
            try{
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(updatedContent.ToString()), "updateContent");
                content.Add(new StringContent(metadataLink), "metadataLink");

                HttpResponseMessage response = await client.PostAsync(JsonUpdateURL, content);

                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();

                return responseContent;
            }
            catch (Exception e){
                return "Error";
            }
        }

        public static string UpdatedBody(string key, string value){
            return new JObject(){
                [$"{key}"] = value
            }.ToString();
        }

        public static async Task<string> MintNft(string nftName, string description, string imageURL,
            string metadataURL,
            string
                toWalletAddress, string feePayerWallet){
            JObject json = new JObject{
                ["name"] = nftName,
                ["description"] = description,
                ["image_url"] = imageURL,
                ["metadata_url"] = metadataURL,
                ["to_wallet_address"] = toWalletAddress,
                ["fee_payer_wallet"] = feePayerWallet
            };

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            StringContent content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try{
                HttpResponseMessage response = await client.PostAsync(MintNftURL, content);

                if (response.IsSuccessStatusCode){
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }

                return "Error";
            }
            catch (HttpRequestException ex){
                return "Error";
            }
        }


        public static async Task<string> FetchNftFromWallet(string walletAddress){
            JObject json = new JObject{
                ["owner_address"] = walletAddress
            };

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", "mw_iaQApysYvH0KEtT8KAjhuoAsiTosRQC0qW5");
            StringContent content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try{
                HttpResponseMessage response = await httpClient.PostAsync(NftInWalletURL, content);
                if (response.IsSuccessStatusCode){
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }

                return "Error";
            }
            catch (Exception ex){
                return "Error";
            }
        }

        public static async Task<string> GetNftInfo(string nftID){
            string apiUrl = $"{NftInfoURL}{nftID}";

            using HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            try{
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode){
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }

                return "Error";
            }
            catch (HttpRequestException ex){
                return "Error";
            }
        }
    }
}