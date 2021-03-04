namespace Tengri
{
    public class Md5
    {
        public static string GenerateHash(string input) 
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
        
                // Convert the byte array to hexadecimal string
                var sb = "";
                
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb += hashBytes[i].ToString("X2");
                }
                
                return sb;
            }
        }        
    }
}