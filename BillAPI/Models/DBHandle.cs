using System.Data;
using System.Data.SqlClient;

namespace BillAPI.Models
{
    public class DBHandle
    {
        private SqlConnection con;

        private void Connection()
        {           
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration configuration = builder.Build();
            string constring = configuration.GetValue<string>("ConnectionStrings:DBCS");
            con = new SqlConnection(constring);
        }

        public List<TariffDetails> GetTariffDetailsById(int id)
        {
            Connection();
            List<TariffDetails> tariffDetails = new List<TariffDetails>();

            SqlCommand cmd = new SqlCommand("Sp_GetTariffDetailsByID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                tariffDetails.Add(
                     new TariffDetails
                     {
                         tariffDetailsID = Convert.ToInt32(dr["tariffId"]),
                         startUnit = Convert.ToInt32(dr["startUnit"]),
                         endUnit = Convert.ToInt32(dr["endUnit"]),
                         energyRate = Convert.ToDecimal(dr["energyRate"]),
                         serviceCharge= Convert.ToDecimal(dr["serviceCharge"])
                     });
            }

            return tariffDetails;
        }
    }
}
