namespace SerpoServer.Database.Models
{
    public class spo_settings
    {
        public bool cache;
        public bool compress;
        public string connstring;
        public string dbtype;
        public string emailport;
        public string emailpsw;
        public string emailserver;
        public string emailuser;
        public int imageHeight;
        public int imageWidth;
        public int logoutHours;
        public string redisConn;
        public string storagePath;
        public bool telemetry;
    }
}