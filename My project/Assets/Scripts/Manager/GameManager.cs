using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        //연결할 매니저
        public ItemManager ItemManager;
        public TileManager TileManager;

        #region Singleton
        public static GameManager instance;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(gameObject);

            ItemManager = GetComponent<ItemManager>();
            TileManager = GetComponent <TileManager>();
        }
        #endregion
    }
}
