using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace EP_01_01.AdminPages
{
    public partial class AddTest : Page
    {
        private static readonly Regex NameRegex = new Regex(@"^[a-zA-ZА-Яа-я0-9\s]{6,}$");
        private readonly Entities db;
        public AddTest()
        {
            InitializeComponent();
            db = new Entities();
            TestDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
        }

        private void TestDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestDatePicker.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Нельзя выбирать дату в прошлом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                TestDatePicker.SelectedDate = DateTime.Today;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string TestTitle = HeaderTitleBox.Text;
            if (!TestDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату проведения теста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            DateTime selectedDate = TestDatePicker.SelectedDate.Value;

            try
            {
 
                if (!NameRegex.IsMatch(TestTitle))
                {
                    MessageBox.Show("Название теста введено неверено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Название теста должно содержать русские или английские символы. Минимальное количество символов - 6.", "Подсказка",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var testToCheck = db.Tests.AsNoTracking().FirstOrDefault(t => t.TestDate == selectedDate);

                if (testToCheck != null)
                {
                    MessageBox.Show("На эту дату уже существует тест! Выберите другую дату!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var test = new Test
                {
                    Test1 = TestTitle,
                    TestDate = selectedDate,
                };
                db.Tests.Add(test);
                db.SaveChanges();
                MessageBox.Show("Тест добавлен в систему", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                FrameManager.MainFrame.Navigate(new Tests());
            }
            catch (InvalidOperationException invalidOpEx)
            {
                MessageBox.Show($"Ошибка операции: {invalidOpEx}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
