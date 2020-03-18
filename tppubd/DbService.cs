using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace tppubd
{
    public class DbService
    {

        public static double SaveImage(byte[] image)
        {
            string queryStmt = "INSERT INTO dbo.Images(Content) VALUES(@Content); SELECT SCOPE_IDENTITY()";
            using (var con =
                new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Mimobod;Integrated Security=true"))
            using (var cmd = new SqlCommand(queryStmt, con))
            {
                {
                    var paramImage = cmd.Parameters.Add("@Content", SqlDbType.Image);
                    paramImage.Value = image;

                    con.Open();
                    double.TryParse(cmd.ExecuteScalar().ToString(), out var index);
                    con.Close();

                    return index;
                }
            }
        }

        public static void SaveToDatabase(
            double imageIndex,
            string accessory,
            double? age,
            string emotionType,
            string facialHair,
            int gender,
            int glasses,
            string hairColor,
            double? smile,
            int faceNumber)
        {
            string queryStmt =
            "INSERT INTO dbo.FacesData(ImageIndex, Accessory, Age, EmotionType, FacialHair, Gender, Glasses, HairColor, Smile, FaceNumber)" +
            "VALUES(@ImageIndex, @Accessory, @Age, @EmotionType, @FacialHair, @Gender, @Glasses, @HairColor, @Smile, @FaceNumber)";

            using (var con =
                new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Mimobod;Integrated Security=true"))
            using (var cmd = new SqlCommand(queryStmt, con))
            {
                var param = cmd.Parameters.Add("@ImageIndex", SqlDbType.Float);
                param.Value = imageIndex;
                var paramAccessory = cmd.Parameters.Add("@Accessory", SqlDbType.VarChar);
                paramAccessory.Value = accessory;
                var paramAge = cmd.Parameters.Add("@Age", SqlDbType.Float);
                paramAge.Value = age;
                var paramEmotionType = cmd.Parameters.Add("@EmotionType", SqlDbType.VarChar);
                paramEmotionType.Value = emotionType;
                var paramFacialHair = cmd.Parameters.Add("@FacialHair", SqlDbType.VarChar);
                paramFacialHair.Value = facialHair;
                var paramGender = cmd.Parameters.Add("@Gender", SqlDbType.Int);
                paramGender.Value = gender;
                var paramGlasses = cmd.Parameters.Add("@Glasses", SqlDbType.Int);
                paramGlasses.Value = glasses;
                var paramHairColor = cmd.Parameters.Add("@HairColor", SqlDbType.VarChar);
                paramHairColor.Value = hairColor;
                var paramSmile = cmd.Parameters.Add("@Smile", SqlDbType.Float);
                paramSmile.Value = smile;
                var paramFaceNumber = cmd.Parameters.Add("@FaceNumber", SqlDbType.Int);
                paramFaceNumber.Value = faceNumber;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
