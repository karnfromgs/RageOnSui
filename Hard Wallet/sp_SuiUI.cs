using System;
using System.Collections.Generic;
using MFPS.Shop;
using UnityEngine;

namespace Hackathon.RE.SUI.Wallet
{
    public class sp_SuiUI : MonoBehaviour
    {
        public static sp_SuiUI Instance;

        [LovattoToogle]public bool viewWallet;

        [Header("Pop Up")]
        public GameObject loading;
        public GameObject error;

        public string firstOpen = String.Empty;

        public Dictionary<string, bool> IsLoading = new Dictionary<string, bool>();

        public List<Window> windows;

        [Serializable]
        public class Window
        {
            public string name;
            public GameObject content;
        }

        private void Awake(){
            if (Instance == null){
                Instance = this;
            }
            loading.SetActive(false);
        }

        // Start is called before the first frame update
        void Start(){
            OpenWindow(firstOpen);
        }

        public void OpenWindow(string name){
            foreach (Window window in windows){
                window.content.SetActive(window.name.Equals(name));
            }
        }

        public void OpenScene(string name){
            bl_UtilityHelper.LoadLevel(name);
        }

        public void OnBackToWallet(){
            if (bl_DataBase.Instance != null){
                bl_DataBase.Instance.LocalUser.ShopData = new bl_ShopUserData();
            }
            OpenWindow("wallet-menu");
        }
    }
}