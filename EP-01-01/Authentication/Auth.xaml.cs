using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace EP_01_01.Authentication
{
    public partial class Auth : Window
    {
        private static readonly Regex LoginRegex = new Regex(@"^[a-zA-Z0-9]{6,}$");
        private static readonly Regex PasswordRegex = new Regex(@"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?/]{6,}$");


        private readonly Entities db;
        private bool isNav = false;
        public Auth()
        {
            InitializeComponent();
            db = new Entities();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLogin.Text;
            string password = UserPassword.Password;

            try
            {
                if (!LoginRegex.IsMatch(login))
                {
                    MessageBox.Show("Логин введён неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Логин включает себя минимум 6 английских символов.", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information); ;
                    return;
                }

                var usersToCheck = db.Users.AsNoTracking().FirstOrDefault(u => u.Login == login);

                if (usersToCheck == null)
                {
                    MessageBox.Show("Пользователя с таким логином нет в системе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!PasswordRegex.IsMatch(password))
                {
                    MessageBox.Show("Пароль введён неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Пароль включает себя минимум 6 английских символов.", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information); ;
                    return;
                }

                MessageBox.Show("Вы авторизовались в систему!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                isNav = true;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();

            }
            catch (InvalidOperationException invalidOpEx)
            {
                MessageBox.Show($"Ошибка операкции: {invalidOpEx}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LinkToRegWindow_Click(object sender, RoutedEventArgs e)
        {
            isNav = true;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isNav)
            {
                return;
            }
            
            var result = MessageBox.Show("Вы действительно хотите выйти из приложения?",
                "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) { e.Cancel = false; } else { e.Cancel = true; }
            
        }
    }
}















