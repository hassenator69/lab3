using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Input;
using lab3.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab3.Models
{
    public class DBMetoder
    {
        public DBMetoder() { }

        public int InsertMusiker(Musikerdetalj md, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";

            String sqlstring = "INSERT INTO Skivor (Skivor_Name, Skivor_Genre, Skivor_Musiker_Id) VALUES (@Skivor_Name, @Skivor_Genre, @Skivor_Musiker_Id)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);


            dbCommand.Parameters.Add("Skivor_Name", SqlDbType.NVarChar, 35).Value = md.Skivor_Name;
            dbCommand.Parameters.Add("Skivor_Genre", SqlDbType.NVarChar, 25).Value = md.Skivor_Genre;
            dbCommand.Parameters.Add("Skivor_Musiker_Id", SqlDbType.Int).Value = md.Skivor_Musiker_Id;
            dbCommand.Parameters.Add("Musiker_Id", SqlDbType.Int).Value = md.Musiker_Id;
            dbCommand.Parameters.Add("Skivor_ID", SqlDbType.Int).Value = md.Skivor_ID;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1)
                {
                    errormsg = " ";
                }
                else
                {
                    errormsg = "Det skapas inte en skiva i databasen.";
                }
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }

        }

        public int DeleteMusiker(int Skivor_ID, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";

            String sqlstring = "DELETE FROM Skivor WHERE Skivor_ID = @skivor_id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("skivor_id", SqlDbType.Int).Value = Skivor_ID;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else
                {
                    errormsg = "Det raderas inte en skiva från databasen.";
                }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int UpdateSkivor(int Skivor_ID, Musikerdetalj updatedSkivor, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";

            String sqlstring = "UPDATE Skivor SET Skivor_Name = @Skivor_Name, Skivor_Genre = @Skivor_Genre, Skivor_Musiker_Id = @Skivor_Musiker_Id WHERE Skivor_ID = @Skivor_ID";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("Skivor_Name", SqlDbType.NVarChar, 35).Value = updatedSkivor.Skivor_Name;
            dbCommand.Parameters.Add("Skivor_Genre", SqlDbType.NVarChar, 25).Value = updatedSkivor.Skivor_Genre;
            dbCommand.Parameters.Add("Skivor_Musiker_Id", SqlDbType.Int).Value = updatedSkivor.Skivor_Musiker_Id;
            dbCommand.Parameters.Add("@Skivor_ID", SqlDbType.Int).Value = Skivor_ID;


            try
            {
                dbConnection.Open();
                int rowsAffected = dbCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    errormsg = " "; //Success
                }
                else
                {
                    errormsg = "Ingen uppdatering i databasen genomfördes.";
                }

                return rowsAffected;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }




        public List<Musikerdetalj>? GetMusikerWithDataSet(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";

            String sqlstring = "SELECT * FROM Skivor INNER JOIN Musiker ON Skivor.Skivor_Musiker_Id = Musiker.Musiker_Id;";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Musikerdetalj> Musikerlist = new List<Musikerdetalj>();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "mySkivor");

                int count = 0;
                int i = 0;
                count = myDS.Tables["mySkivor"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Musikerdetalj md = new Musikerdetalj();
                        md.Skivor_ID = Convert.ToInt32(myDS.Tables["mySkivor"].Rows[i]["Skivor_ID"]);
                        md.Skivor_Name = myDS.Tables["mySkivor"].Rows[i]["Skivor_Name"].ToString();
                        md.Skivor_Genre = myDS.Tables["mySkivor"].Rows[i]["Skivor_Genre"].ToString();
                        md.Skivor_Musiker_Id = Convert.ToInt32(myDS.Tables["mySkivor"].Rows[i]["Skivor_Musiker_Id"]);
                        md.Musiker_Name = myDS.Tables["mySkivor"].Rows[i]["Musiker_Name"].ToString();

                        i++;
                        Musikerlist.Add(md);
                    }
                    errormsg = " ";
                    return Musikerlist;
                }
                else
                {
                    errormsg = "Det hämtas inget musikbibliotek.";
                    return null;
                }
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

        public Musikerdetalj GetMusiker(int Skivor_ID, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";

            String sqlstring = "SELECT * FROM Skivor WHERE Skivor_ID = @skivor_id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("Skivor_Id", SqlDbType.Int).Value = Skivor_ID;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Musikerdetalj> Musikerlist = new List<Musikerdetalj>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "mySkivor");

                int count = 0;
                int i = 0;
                count = myDS.Tables["mySkivor"].Rows.Count;

                if (count > 0)
                {

                    Musikerdetalj md = new Musikerdetalj();
                    md.Skivor_ID = Convert.ToInt32(myDS.Tables["mySkivor"].Rows[i]["Skivor_ID"]);
                    md.Skivor_Name = myDS.Tables["mySkivor"].Rows[i]["Skivor_Name"].ToString();
                    md.Skivor_Genre = myDS.Tables["mySkivor"].Rows[i]["Skivor_Genre"].ToString();
                    md.Skivor_Musiker_Id = Convert.ToInt32(myDS.Tables["mySkivor"].Rows[i]["Skivor_Musiker_Id"]);
                   // md.Musiker_Name = myDS.Tables["mySkivor"].Rows[i]["Musiker_Name"].ToString();


                    errormsg = " ";
                    return md;
                }
                else
                {
                    errormsg = "Det hämtas ingen skiva.";
                    return null;
                }
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

        public List<Musikerdetalj> SearchMusiker(string input, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";

            String sqlString = "SELECT * FROM Skivor WHERE Skivor_Name LIKE @input";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            dbCommand.Parameters.Add("input", SqlDbType.NVarChar, 255).Value = input;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet ds = new DataSet();

            List<Musikerdetalj> musikerList = new List<Musikerdetalj>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                myAdapter.Fill(ds, "Skivor");

                foreach (DataRow row in ds.Tables["Skivor"].Rows)
                {
                    Musikerdetalj m = new Musikerdetalj();
                    m.Skivor_Name = row["SKivor_Name"].ToString();
                    m.Skivor_Genre = row["Skivor_Genre"].ToString();
                    
                    m.Skivor_ID = Convert.ToInt16(row["Skivor_ID"]);
                    m.Skivor_Musiker_Id = Convert.ToInt16(row["Skivor_Musiker_Id"]);


                    musikerList.Add(m);
                }
                return musikerList;

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