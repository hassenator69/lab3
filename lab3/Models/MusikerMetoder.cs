using System;
using System.Data;
using System.Data.SqlClient;

namespace lab3.Models
{
	public class MusikerMetoder
	{
		public MusikerMetoder() { }

        public List<MusikerModel> GetMusikerLista(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=localhost,1433;Database=Lab3;User Id=sa;Password=H@ssesDB;";
            String sqlstring = "Select * From Musiker";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(dbCommand);

            DataSet dataSet = new DataSet();
            List<MusikerModel> MusikerModelLista = new List<MusikerModel>();

            errormsg = "";

            try
            {
                dbConnection.Open();
                dataAdapter.Fill(dataSet, "Musiker");

                if (dataSet.Tables.Contains("Musiker") && dataSet.Tables["Musiker"].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables["Musiker"].Rows)
                    {
                        MusikerModel mm = new MusikerModel();
                        mm.Musiker = row["Musiker_Name"].ToString();
                        mm.Id = Convert.ToInt16(row["Musiker_Id"]);

                        MusikerModelLista.Add(mm);
                    }
                }

                return MusikerModelLista;
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


