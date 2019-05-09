using PMSIDAL.Pricing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSDAL.Pricing
{
    public class PricingDetailsDAL: IPricingDetailsDAL
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public bool InsertBulkPricingDetails(string ImportValues)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.USP_ImportPricingDetails", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@ImportValues", SqlDbType.VarChar).Value = ImportValues;
                connection.Open();
                sqlCommand.CommandTimeout = 120;
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
            return true;
        }

        public DataSet FetchPricingDetails()
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection connection=new SqlConnection(ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand("dbo.USP_FetchProductPricingDetails", connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                    dataAdapter.Fill(dataSet);
                    connection.Close();
                    dataAdapter.Dispose();
                }
                return dataSet;
            }
            finally
            {
                dataSet = null;
            }
        }
    }
}
