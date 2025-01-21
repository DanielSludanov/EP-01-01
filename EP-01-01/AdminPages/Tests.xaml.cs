using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EP_01_01.AdminPages
{
    public partial class Tests : Page
    {
        private readonly Entities db;
        public Tests()
        {
            InitializeComponent();
            db = new Entities();
            DataGridTests.ItemsSource = db.Tests.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddTest());
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var testToDelete = DataGridTests.SelectedItems.Cast<Test>().Select(t => t.ID).ToList();
            if (MessageBox.Show($"Вы точно хотите удалить {testToDelete.Count()} тестов?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var testsToDelete = db.Tests.Where(t => testToDelete.Contains(t.ID)).ToList();
                    db.Tests.RemoveRange(testsToDelete);
                    db.SaveChanges();
                    MessageBox.Show("Тесты удалены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataGridTests.ItemsSource = db.Tests.ToList();
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    MessageBox.Show($"Ошибка операции: {invalidOpEx}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
