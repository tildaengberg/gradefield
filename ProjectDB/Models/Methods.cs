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

        public bool AddImage(out string errormsg, Person person)
        {
            try

            {



                if (person != null)

                {
                    string connectionstring = GetConnection().GetSection("ConnectionStrings").GetSection("MyConnectionString").Value;

                    SqlConnection con = new SqlConnection(connectionstring);
                    SqlCommand cmd = new SqlCommand("Insert into Tbl_Image(Im_Title,Im_Data) values(@FileNames,@Filepic)", con);

                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@FileNames", bild.Title);

                    cmd.Parameters.AddWithValue("@Filepic", bytes);

                    con.Open();

                    cmd.ExecuteNonQuery();

                    con.Close();
                    errormsg = "";
                    return bytes;



                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

            errormsg = "";
            return null;

        }
    }
}
