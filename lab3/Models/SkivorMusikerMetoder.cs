using System;
using System.Data;
using System.Data.SqlClient;

namespace lab3.Models
{
	public class SkivorMusikerMetoder
	{
		public SkivorMusikerMetoder() { }

        public List<SkivorMusikerModel> GetSkivorMusikerModel(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";
            String sqlstring = "SELECT Skivor.Skivor_Name, Musiker.Musiker_Name FROM Skivor INNER JOIN Musiker ON Skivor.Skivor_Musiker_Id = Musiker.Musiker_Id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            List<SkivorMusikerModel> SkivorMusikerModelLista = new List<SkivorMusikerModel>();

            errormsg = "";

            try
            {
                dbConnection.Open();
                dataAdapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        SkivorMusikerModel sm = new SkivorMusikerModel();
                        sm.Skivor = row["Skivor_Name"].ToString();
                        sm.Musiker = row["Musiker_Name"].ToString();
                        //sm.Skivor_Musiker_Id = Convert.ToInt16(row["Skivor_Musiker_Id"]);


                        SkivorMusikerModelLista.Add(sm);
                    }
                }

                return SkivorMusikerModelLista;
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

        public List<SkivorMusikerModel> GetSkivorMusikerModel(int musikerId, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";
            String sqlstring = "SELECT Skivor.Skivor_Name, Musiker.Musiker_Name FROM Skivor INNER JOIN Musiker ON Skivor.Skivor_Musiker_Id = Musiker.Musiker_Id WHERE Musiker.Musiker_Id = @MusikerId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@musikerId", SqlDbType.Int).Value = musikerId;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            List<SkivorMusikerModel> SkivorMusikerModelLista = new List<SkivorMusikerModel>();

            errormsg = "";

            try
            {
                dbConnection.Open();
                dataAdapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        SkivorMusikerModel sm = new SkivorMusikerModel();
                        sm.Skivor = row["Skivor_Name"].ToString();
                        sm.Musiker = row["Musiker_Name"].ToString();


                        SkivorMusikerModelLista.Add(sm);
                    }
                }

                return SkivorMusikerModelLista;
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

