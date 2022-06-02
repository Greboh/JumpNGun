namespace JumpNGun
{
    public class Database
    {
        private static Database _instance;

        public static Database Instance
        {
            get
            {
                _instance ??= new Database();

                return _instance;
            }
        }
        
        
        
        public void OpenConnection()
        {
            
        }

        public void CloseConnection()
        {
            
        }

    }
}