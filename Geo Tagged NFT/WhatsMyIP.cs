using System;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class WhatsMyIP
{
    // read / write IPs
    public static string IP{
        get => PlayerPrefs.GetString("IP", String.Empty);
        set => PlayerPrefs.SetString("IP", value);
    }

    // read / write Country
    public static string Country{
        get => PlayerPrefs.GetString("Country", String.Empty);
        set => PlayerPrefs.SetString("Country", value);
    }

    public static async Task<string> GetCurrentCountryCode(){
        using HttpClient client = new HttpClient();
        try{
            HttpResponseMessage resIP = await client.GetAsync("https://api.ipify.org");
            resIP.EnsureSuccessStatusCode();
            string ipAddress = await resIP.Content.ReadAsStringAsync();
            if (ipAddress.Equals(IP)){
                return Country;
            }

            try{
                string json = await client.GetStringAsync($"http://www.geoplugin.net/json.gp?ip={ipAddress}");
                JObject resCountry = JObject.Parse(json);
                string countryName = resCountry["geoplugin_countryCode"]!.ToString();
                IP = ipAddress;
                Country = countryName;
                return countryName;
            }
            catch (Exception ex){
                return "Error";
            }
        }
        catch (Exception e){
            return "Error";
        }
    }
}