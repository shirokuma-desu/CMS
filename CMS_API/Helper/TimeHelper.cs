namespace CMS_API.Helper
{
    public class TimeHelper
    {
        public static int ToUnixTimeStamp(DateTime datetime)
        {
            return (int)(datetime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
