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
                    errormsg = "Fel lösenord";
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




        public List<Course> GetFailed(out string errormsg, string username)
        {


            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en user i databasen
            String sqlstring = "SELECT Ku_Namn, Ku_HP FROM Tbl_KursPerson INNER JOIN Tbl_Kurs ON Tbl_KursPerson.KP_Kurs = Tbl_Kurs.Ku_ID INNER JOIN Tbl_Person ON Tbl_KursPerson.KP_Person = Tbl_Person.Pe_ID INNER JOIN Tbl_Status ON Tbl_KursPerson.KP_Status = Tbl_Status.St_ID WHERE Pe_Anvandarnamn = @user AND St_Kursstatus = 'Oavslutad'; ";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("user", System.Data.SqlDbType.NVarChar, 30).Value = username;

            SqlDataReader reader = null;

            List<Course> courses = new List<Course>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course();
                    course.Name = reader["Ku_Namn"].ToString();
                    course.HP = Convert.ToDouble(reader["Ku_HP"]);

                    courses.Add(course);

                }
                reader.Close();
                return courses;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<Course> GetOngoing(out string errormsg, string username)
        {


            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en user i databasen
            String sqlstring = "SELECT Ku_Namn, Ku_HP FROM Tbl_KursPerson INNER JOIN Tbl_Kurs ON Tbl_KursPerson.KP_Kurs = Tbl_Kurs.Ku_ID INNER JOIN Tbl_Person ON Tbl_KursPerson.KP_Person = Tbl_Person.Pe_ID INNER JOIN Tbl_Status ON Tbl_KursPerson.KP_Status = Tbl_Status.St_ID WHERE Pe_Anvandarnamn = @user AND St_Kursstatus = 'Pågående'; ";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("user", System.Data.SqlDbType.NVarChar, 30).Value = username;

            SqlDataReader reader = null;

            List<Course> courses = new List<Course>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course();
                    course.Name = reader["Ku_Namn"].ToString();
                    course.HP = Convert.ToDouble(reader["Ku_HP"]);

                    courses.Add(course);

                }
                reader.Close();
                return courses;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }



        public List<Grade> Grades(out string errormsg, string username)
        {

            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en user i databasen
            String sqlstring = "SELECT Be_Kursbetyg, COUNT(Tbl_KursPerson.KP_Betyg) AS Frequency FROM Tbl_KursPerson INNER JOIN Tbl_Betyg ON Tbl_KursPerson.KP_Betyg = Tbl_Betyg.Be_ID INNER JOIN Tbl_Person ON Tbl_KursPerson.KP_Person = Tbl_Person.Pe_ID WHERE Tbl_Person.Pe_Anvandarnamn = @user GROUP BY Tbl_Betyg.Be_Kursbetyg ORDER BY COUNT(Tbl_KursPerson.KP_Betyg) DESC ";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataReader reader = null;

            dbCommand.Parameters.Add("user", System.Data.SqlDbType.NVarChar, 30).Value = username;

            List<Grade> stats = new List<Grade>();
            errormsg = "";



            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {

                    Grade grade = new Grade();

                    if (reader["Be_Kursbetyg"].ToString() != "NAN")
                    {
                        grade.GradeType = reader["Be_Kursbetyg"].ToString();
                        grade.Frequency = Convert.ToInt16(reader["Frequency"]);
                        stats.Add(grade);
                    }

                }
                reader.Close();
                return stats;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }

        }

        public List<Course> GetCourses(out string errormsg, string username)
        {


            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en user i databasen
            String sqlstring = "SELECT Ku_ID, Ku_Namn, Ku_HP, St_Kursstatus FROM Tbl_KursPerson INNER JOIN Tbl_Kurs ON Tbl_KursPerson.KP_Kurs = Tbl_Kurs.Ku_ID INNER JOIN Tbl_Person ON Tbl_KursPerson.KP_Person = Tbl_Person.Pe_ID INNER JOIN Tbl_Status ON Tbl_KursPerson.KP_Status = Tbl_Status.St_ID WHERE Pe_Anvandarnamn = @user ; ";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("user", System.Data.SqlDbType.NVarChar, 30).Value = username;

            SqlDataReader reader = null;

            List<Course> courses = new List<Course>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course();
                    course.ID = Convert.ToInt16(reader["Ku_ID"]);
                    course.Name = reader["Ku_Namn"].ToString();
                    course.HP = Convert.ToDouble(reader["Ku_HP"]);
                    course.Status = reader["St_Kursstatus"].ToString();

                    courses.Add(course);

                }
                reader.Close();
                return courses;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }



        }


        public Course GetCourse(out string errormsg, string username, int selected)
        {


            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en user i databasen
            String sqlstring = "SELECT Ku_ID, Ku_Namn, Ku_HP, In_Institutionsnamn, St_Kursstatus, Be_Kursbetyg FROM Tbl_KursPerson INNER JOIN Tbl_Kurs ON Tbl_KursPerson.KP_Kurs = Tbl_Kurs.Ku_ID INNER JOIN Tbl_Person ON Tbl_KursPerson.KP_Person = Tbl_Person.Pe_ID INNER JOIN Tbl_Status ON Tbl_KursPerson.KP_Status = Tbl_Status.St_ID INNER JOIN Tbl_Betyg ON Tbl_KursPerson.KP_Betyg = Tbl_Betyg.Be_ID INNER JOIN Tbl_Institution ON Tbl_Kurs.Ku_Institution = Tbl_Institution.In_ID WHERE Pe_Anvandarnamn = @user AND Ku_ID = @sel ";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataReader reader = null;

            dbCommand.Parameters.Add("user", System.Data.SqlDbType.NVarChar, 30).Value = username;
            dbCommand.Parameters.Add("sel", System.Data.SqlDbType.Int).Value = selected;


            Course course = new Course();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    
                    course.ID = Convert.ToInt16(reader["Ku_ID"]);
                    course.Name = reader["Ku_Namn"].ToString();
                    course.HP = Convert.ToDouble(reader["Ku_HP"]);
                    course.Institution = reader["In_Institutionsnamn"].ToString();
                    course.Status = reader["St_Kursstatus"].ToString();
                    course.Betyg = reader["Be_Kursbetyg"].ToString();

                }
                reader.Close();

                return course;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
