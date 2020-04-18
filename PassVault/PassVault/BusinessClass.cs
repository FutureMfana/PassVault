using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PassVault
{
    class BusinessClass
    {
        string otp;
        SqlConnection sqlCon;
        SqlCommand cmd;
        string sqlStr;

        #region getConnection()
        public string getConnection()
        {
            try
            {
                //sqlCon = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Student\\Documents\\Visual Studio 2015\\Projects\\MiKey Vault\\MiKey Vault\\MiKey Vault DB.mdf; Integrated Security = True");
                sqlCon = new SqlConnection("Data Source=DESKTOP-G1E2CG2;Initial Catalog=PassVault;Integrated Security=True");
                sqlCon.Open();
                sqlCon.Close();
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region addSite()
        public string addSite(string siteName, string pswd, int uid, string username = null, string email = null, string cmnts = "No comment")
        {
            try
            {
                sqlStr = $"INSERT INTO SiteInfo.Sites(SiteName, Username, Password, UID, Comments) VALUES('{siteName}', '{username}', '{pswd}', {10000}, '{cmnts}')";
                
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                cmd = new SqlCommand(sqlStr, sqlCon);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region addUser()
        public string addUser(string uname, string email, string pswd)
        {
            try
            {
                otp = generateOTP();
                if (otp.Length > 6 || otp.Length == 0)
                {
                    throw new Exception(otp);
                }
                sqlStr = $"INSERT INTO dbo.UserInfo (Username, Emailaddress, Password) VALUES ('{uname}','{email}','{pswd}')";

                cmd = new SqlCommand(sqlStr, sqlCon);
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                cmd.ExecuteNonQuery();

                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                cmd.Dispose();

                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region getAllSites()

        #endregion

        #region generatePassword()
        public void generatePassword(out string password) //exceptions to be handled by a calling method
        {
            string ucase = "QWERTYUIOPASDFGHJKLZXCVBNM";
            int ucase_count = 0;                                //used to check if there are ucase chars within the password

            string lcase = "qwertyuiopasdfghjklzxcvbnm";
            int lcase_count = 0;                                //used to check if there are lcase chars within the password

            string spchars = ",./;[]\\=-`~!@#$%^&*()_+}{:?><";
            int spchars_count = 0;                              //used to check if there are special chars within the password
            int intcount = 0;                                   //used to check if there are numeric chars within the password

            StringBuilder s = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 12; ++i)
            {
                int x = rnd.Next(0, 5);
                switch (x)
                {
                    case 0:
                        s.Append(ucase.Substring(rnd.Next(0, ucase.Length - 1), 1));
                        ucase_count++;
                        break;
                    case 1:
                        s.Append(lcase.Substring(rnd.Next(0, lcase.Length - 1), 1));
                        lcase_count++;
                        break;
                    case 2:
                        s.Append(spchars.Substring(rnd.Next(0, spchars.Length - 1), 1));
                        spchars_count++;
                        break;
                    default:
                        s.Append(rnd.Next(0, 9).ToString());
                        intcount++;
                        break;
                }
            }

            //this section ensures that a password contains at least one type of each character
            if (ucase_count == 0) {
                s.Append(ucase.Substring(rnd.Next(0, ucase.Length - 1), 1));
            }
            if (lcase_count == 0)
            {
                s.Append(lcase.Substring(rnd.Next(0, lcase.Length - 1), 1));
            }
            if (spchars_count == 0)
            {
                s.Append(spchars.Substring(rnd.Next(0, spchars.Length - 1), 1));
            }
            if (intcount == 0)
            {
                s.Append(rnd.Next(0, 9).ToString());
            }

            password = s.ToString();
        }
        #endregion

        #region generateOTP()
        private string generateOTP()
        {
            StringBuilder otp = new StringBuilder();
            try
            {
                while (otp.Length < 6)
                {
                    Random num = new Random();
                    otp.Append(num.Next(0, 9));
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion
    }
}