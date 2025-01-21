using System.Windows;

namespace EP_01_01
{
    public partial class App : Application
    {
        public int CurrentUserID { get ; set; }

        public void SetCurrentUserID (int UserID)
        {
            CurrentUserID = UserID;
        }
    }
}
