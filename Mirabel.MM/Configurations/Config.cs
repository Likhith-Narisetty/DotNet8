using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mirabel.MM.Configurations
{ 
    public class Config 
    {
        public string UID { get; set; }
        public string Password { get; set; }
    }

    #region "DataBaseConfig"
    public class DBConfigDEV : Config
    {
        public DBConfigDEV() 
        {
            UID = "web";
            Password = "Mir@b202L-sqlw@b";
        }
    }

    public class DBConfigSMOKE : Config
    {
        public DBConfigSMOKE() 
        {
            UID = "DEVMM"; 
            Password = "bGlmZWlyc2hAMjcwNTA5";
        }
    }

  
    public class DBConfigSTAGING : Config
    {
        public DBConfigSTAGING() 
        {
            UID = "mirabeldev";
            Password = "d2VsY29tZTMj";
        }
    }

    public class DBConfigPRODUCTION : Config
    { 
        public DBConfigPRODUCTION()
        {
            UID = "sa";
            Password = "d2VsY29tZTMj";
        }
    }

    #endregion

    #region "Encryption Config"
    public class AESEncryptLive
    {
        public string privatekey = "HBX2DES9YHXIK1NK5MX10YWV8FCHOMXC";
        public string publickey = "EYJHBGCIOIJIUZI1";
    }

    #endregion


}
