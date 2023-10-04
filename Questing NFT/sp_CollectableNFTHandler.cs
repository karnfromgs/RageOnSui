using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hackathon.RE.SUI.API_Method;
using Hackathon.RE.SUI.Wallet;
using MFPS.Addon.Customizer;
using TMPro;
using UnityEngine;

public class sp_CollectableNFTHandler : MonoBehaviour
{
    public GameObject collectableNft;
    public GameObject[] spawnLocations;

    public GameObject message;
    public TextMeshProUGUI messageText;

    void Start(){
        foreach (var globalCamo in bl_CustomizerData.Instance.GlobalCamos){
            if (globalCamo.camoType == GlobalCamo.CamoType.Glitch &&
                globalCamo.Unlockability.IsUnlocked(bl_CustomizerData.Instance.GetGlobalCamoID(globalCamo.Name))){
                Debug.Log(globalCamo.Name);
                Debug.Log(globalCamo.Unlockability.IsUnlocked(
                    bl_CustomizerData.Instance.GetGlobalCamoID(globalCamo.Name)));
                var collectableNftData =
                    SUIWWalletResponse.ColletableNftsData.Single(x => x.nftName.Equals(globalCamo.Name));
                if (!(bool)collectableNftData.Metadata["claimed"]){
                    if (GetProbability()){
                        GameObject card = Instantiate(collectableNft,
                            spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position,
                            Quaternion.identity);
                        sp_CollectableItem collectableItem = card.GetComponent<sp_CollectableItem>();
                        collectableItem.Init(globalCamo, collectableNftData.metadataUrl, this);
                    }
                    else{
                        Debug.Log("Next Time");
                    }
                    // Check Probability Function
                    // Spawn probability 
                }
            }
        }
    }

    public bool GetProbability(int percentage = 100){
        if (percentage == 100) return true;
        float probability = percentage / 100f;
        float randomValue = Random.Range(0f, 1f);
        return randomValue < probability;
    }

    public void ShowMessage(int amount){
        StartCoroutine(MessageUI(amount));
    }


    public IEnumerator MessageUI(int textMessage){
        string messageWithCommas = textMessage.ToString("N0");
        messageText.text = $" {messageWithCommas} coins added to account".ToUpper();
        message.SetActive(true);
        yield return new WaitForSeconds(5);
        message.SetActive(false);
    }
}