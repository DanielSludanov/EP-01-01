using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EP_01_01.AdminPages
{
    public partial class Questions : Page
    {
        
        private readonly Entities db;
        public Questions()
        {
            InitializeComponent();
            db = new Entities();
            
            DataGridQuestions.ItemsSource = db.Questions.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddQuestion(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Получаем ID выбранных вопросов
            var questionIdsToDelete = DataGridQuestions.SelectedItems
                .Cast<Question>()
                .Select(q => q.ID)
                .ToList();

            if (questionIdsToDelete.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один вопрос для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить {questionIdsToDelete.Count} вопросов?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    
                    var testQuestionsToDelete = db.TestQuestions
                    .Where(tq => tq.QuestionID.HasValue && questionIdsToDelete.Contains(tq.QuestionID.Value)).ToList();
                    db.TestQuestions.RemoveRange(testQuestionsToDelete);

                    
                    var questionsToDelete = db.Questions
                        .Where(q => questionIdsToDelete.Contains(q.ID))
                        .ToList();
                    db.Questions.RemoveRange(questionsToDelete);

                    
                    db.SaveChanges();

                    MessageBox.Show("Вопросы успешно удалены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    DataGridQuestions.ItemsSource = db.Questions.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении вопросов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddQuestion((sender as Button).DataContext as Question));
        }
    }
}