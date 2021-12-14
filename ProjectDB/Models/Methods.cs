using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ProjectDB.Models
{
    public class Methods
    {
        public Methods()
        {
        }

        public IConfigurationRoot GetConnection()

        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

            return builder;

        }

        public bool VerifyAccount(out string errormsg, Person person)
        {
            try

            {
                if (person != null)

                {
                    string connectionstring = GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;

                    SqlConnection con = new SqlConnection(connectionstring);
                    SqlDataReader reader = null;
                    SqlCommand cmd = new SqlCommand("SELECT Pe_Losenord FROM Tbl_Person WHERE Pe_Anvandarnamn = '@user';", con);
                    cmd.CommandType = CommandType.Text;
                    
                    cmd.Parameters.AddWithValue("@user", person.Username);

                    con.Open();

                    cmd.ExecuteNonQuery();

                    reader = cmd.ExecuteReader();

                    string password = "";
                    errormsg = "";

                    while (reader.Read())
                    {
                        password = reader["Pe_Losenord"].ToString();
                    }

                    reader.Close();

                    con.Close();

                    if (password == person.Password)
                    {
                        
                        return true;
                    } else {
                        return false;
                    }

                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

            errormsg = "";
            return false;

        }
    }
}
