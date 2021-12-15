using System;
using System.Collections.Generic;
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


            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en user i databasen
            String sqlstring = "SELECT Pe_Losenord FROM Tbl_Person WHERE Pe_Anvandarnamn = @user";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("user", System.Data.SqlDbType.NVarChar, 30).Value = person.Username;

            SqlDataReader reader = null;

            string password = "";

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    password = reader["Pe_Losenord"].ToString();
                    
                }
                reader.Close();



                if (person.Password == password)
                {

                    return true;
                }
                else
                {
                    errormsg = password;
                    return false;
                }

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return false;
            }
            finally
            {
                dbConnection.Close();
            }
        }





        public bool CreateAccount(out string errormsg, Person person)
        {


            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en user i databasen
            String sqlstring = "INSERT INTO [Tbl_Person]([Pe_Anvandarnamn], [Pe_Losenord], [Pe_Utbildning], [Pe_Examensdatum]) VALUES (@user, @password, @education, @datetime)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("user", System.Data.SqlDbType.NVarChar, 30).Value = person.Username;
            dbCommand.Parameters.Add("password", System.Data.SqlDbType.NVarChar, 30).Value = person.Password;
            dbCommand.Parameters.Add("education", System.Data.SqlDbType.NVarChar, 50).Value = person.Education;
            dbCommand.Parameters.Add("datetime", System.Data.SqlDbType.DateTime).Value = person.ExamDate;

            errormsg = "";
            bool success = false;

            try
            {
                dbConnection.Open();
                int i = dbCommand.ExecuteNonQuery();
                if(i == 1) {
                    success = true;
                    return success;
                }

                else
                {
                    errormsg = "Det går inte att lägga till en användare";
                    return false;
                }
               

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return false;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
