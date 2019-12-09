using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mimobod
{
    public class DbService
    {
        public static void SaveToDatabase(byte[] image, List<string> emotions)
        {
            string queryStmt = "INSERT INTO dbo.mimobod(Content, Emotions) VALUES(@Content, @Emotions)";

            using (var con =
                new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Mimobod;Integrated Security=true"))
            using (var cmd = new SqlCommand(queryStmt, con))
            {
                var paramImage = cmd.Parameters.Add("@Content", SqlDbType.Image);
                paramImage.Value = image;

                var paramEmotions = cmd.Parameters.Add("@Emotions", SqlDbType.VarChar);
                int i = 1;
                foreach (var emotion in emotions)
                {
                    paramEmotions.Value += $"Face №{i} emotion - {emotion}";
                    i++;
                }

                if (paramEmotions.Value == null)
                {
                    paramEmotions.Value = "Can't detect faces";
                }

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
