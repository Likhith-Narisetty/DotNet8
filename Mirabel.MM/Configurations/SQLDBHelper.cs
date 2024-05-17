using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using Mirabel.MM.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;


namespace Mirabel.MM.Configurations
{
    public class SQLDBHelper
    {
        static SqlTransaction gTrans;
        public IConfiguration Configuration { get; }
        public SQLDBHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string DBConfiguration(string DB) 
        {
            string connectionstring = "", Password = "", UID = "";
            DBConfigSMOKE SMOKE = new DBConfigSMOKE();
            DBConfigDEV DEV = new DBConfigDEV(); 
            DBConfigSTAGING STAGING = new DBConfigSTAGING();
            DBConfigPRODUCTION PRODUCTION = new DBConfigPRODUCTION();  

            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
                IConfigurationRoot root = builder.Build();
                string DataBase = "ConnectionStrings:" + DB;
                string connection = root[DataBase].ToString();
                string EnvKey = "Environment";
                string Environment = root.GetSection(EnvKey)?.Value;

                if (Environment == "DEV")
                {
                    UID = DEV.UID;
                    //Password = Decrypt(DEV.Password);
                    Password = DEV.Password;
                }
                else if (Environment == "SMOKE")
                {
                    UID = SMOKE.UID;
                    Password = Decrypt(SMOKE.Password); 
                }
                else if (Environment == "STAGING") 
                {
                    UID = STAGING.UID;
                    Password = Decrypt(STAGING.Password);
                }
                else
                {
                    UID = PRODUCTION.UID; 
                    Password = Decrypt(PRODUCTION.Password);

                }
                connectionstring = connection + "uid=" + UID + ";password=" + Password;
            }
            catch (Exception ex)
            {
                
            }
            return connectionstring;
        }

        internal static DataSet ExecuteDataset_CS1(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {
            string connectionstring = DBConfiguration("ConnectionString1");

            DataSet dsGen = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.CommandTimeout = Convert.ToInt32(SQLDBHelper.GetAppsettings("SqlCommandTimeout"));
                    cmd.Parameters.AddRange(pars);
                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsGen);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return dsGen;
        }


        internal static DataSet ExecuteDataset_CS2(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {
            string connectionstring = DBConfiguration("ConnectionString2");

            DataSet dsGen = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.CommandTimeout = Convert.ToInt32(SQLDBHelper.GetAppsettings("SqlCommandTimeout"));

                    cmd.Parameters.AddRange(pars);
                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsGen);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return dsGen;
        }

        public static string Decrypt(string Data)
        {
            byte[] deNC_data = Convert.FromBase64String(Data);
            string deNC_str = ASCIIEncoding.ASCII.GetString(deNC_data);
            return deNC_str;
        }

        public static string GetAppsettings(string Key)
        {
            string KeyElement = "";
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
                IConfigurationRoot root = builder.Build();
                KeyElement = root[Key];
            }
            catch (Exception ex)
            {
               
            }
            return KeyElement;
        }


    }
}
