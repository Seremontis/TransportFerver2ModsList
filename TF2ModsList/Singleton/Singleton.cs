using System;
using System.Collections.Generic;
using System.Text;

namespace TF2ModsList.Models
{
    //singleton
    public class Singleton
    {
        private static DataApp _instance;

        public static DataApp Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataApp();
                return _instance;
            }
        }
    }
}
