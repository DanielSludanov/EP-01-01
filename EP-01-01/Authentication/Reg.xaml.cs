using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace EP_01_01.Authentication
{
    public partial class Reg : Window
    {
        private static readonly Regex LoginRegex = new Regex(@"^[a-zA-Z0-9]{6,}$");
        private static readonly Regex PasswordRegex = new Regex(@"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?/]{6,}$"); private readonly Entities db;
        
        private bool isNav = false;

        public Reg()
        {
            InitializeComponent();
            db =  new Entities();
            LoadUserRoles();
        }

        private void LoadUserRoles()
        {
            var roles = db.Roles.ToList();
            UserRoles.ItemsSource = roles;
            UserRoles.DisplayMemberPath = "RoleTitle";
            UserRoles.SelectedValuePath = "ID";
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

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLogin.Text;
            string password = UserPassword.Password;
            int? UserRole = (int?)UserRoles.SelectedValue;

            try
            {
                if ((string.IsNullOrEmpty(login)) && (string.IsNullOrEmpty(password)) && (UserRoles.SelectedItem == null)) {
                    MessageBox.Show("Заполните все поля, чтобы зарегистрироваться в систему!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                if (!LoginRegex.IsMatch(login))
                {
                    MessageBox.Show("Логин введён неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Логин включает себя минимум 6 английских символов.", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information); ;
                    return;
                }

                var usersToCheck = db.Users.AsNoTracking().FirstOrDefault(u => u.Login == login);

                if (usersToCheck != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже в системе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!PasswordRegex.IsMatch(password))
                {
                    MessageBox.Show("Пароль введён неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Пароль включает себя минимум 6 английских символов.", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information); ;
                    return;
                }

                if (UserRoles.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var User = new User
                {
                    Login = login,
                    Password = password,
                    Role = UserRole,
                };
                db.Users.Add(User);
                db.SaveChanges();

                MessageBox.Show("Вы успешно зарегистрировались в систему", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                isNav = true;
                Auth auth = new Auth();
                auth.Show();
                this.Close();

            }
            catch (InvalidOperationException invalidOpEx)
            {
                MessageBox.Show($"Ошибка операции: {invalidOpEx}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LinkToAuthWindow_Click(object sender, RoutedEventArgs e)
        {
            isNav = true;
            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }
    }
}