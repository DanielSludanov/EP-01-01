using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EP_01_01.AdminPages
{
  
    public partial class AddQuestion : Page
    {
        private readonly Entities db;
        private readonly Question currentQuestion;
        public AddQuestion(Question selectedQuestion = null)
        {
            InitializeComponent();
            currentQuestion = selectedQuestion;
            db = new Entities();
            LoadUserTests();
            LoadQuestionData();
        }

        private void LoadUserTests()
        {
            var tests = db.Tests.ToList();
            UserTests.ItemsSource = tests;
            UserTests.DisplayMemberPath = "Test1";
            UserTests.SelectedValuePath = "ID";
        }

        private void LoadQuestionData()
        {
            if (currentQuestion != null)
            {
                HeaderDifficultyBox.Text = currentQuestion.QuestionType;
                HeaderQuestionBox.Text = currentQuestion.QuestionText;
                HeaderAnswer1Box.Text = currentQuestion.Answer1;
                HeaderAnswer2Box.Text = currentQuestion.Answer2;
                HeaderAnswer3Box.Text = currentQuestion.Answer3;
                HeaderAnswer4Box.Text = currentQuestion.Answer4;
                CorrectAnswerBox.Text = currentQuestion.CorrectAnswer.ToString();

                var testQuestion = db.TestQuestions.FirstOrDefault(tq => tq.QuestionID == currentQuestion.ID);
                if (testQuestion != null)
                {
                    UserTests.SelectedValue = testQuestion.TestID;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            
            string difficult = HeaderDifficultyBox.Text;
            string questionText = HeaderQuestionBox.Text;
            string answer1 = HeaderAnswer1Box.Text;
            string answer2 = HeaderAnswer2Box.Text;
            string answer3 = HeaderAnswer3Box.Text;
            string answer4 = HeaderAnswer4Box.Text;

            
            if (!int.TryParse(CorrectAnswerBox.Text, out int correctAnswer) || correctAnswer < 1 || correctAnswer > 4)
            {
                MessageBox.Show("Введите правильный номер ответа (от 1 до 4).");
                return;
            }

            if (UserTests.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, выберите тест.");
                return;
            }

            int selectedTestId = (int)UserTests.SelectedValue;

            try
            {
                if (currentQuestion == null)
                {
                    
                    var newQuestion = new Question
                    {
                        QuestionType = difficult,
                        QuestionText = questionText,
                        Answer1 = answer1,
                        Answer2 = answer2,
                        Answer3 = answer3,
                        Answer4 = answer4,
                        CorrectAnswer = correctAnswer
                    };

                    db.Questions.Add(newQuestion);
                    db.SaveChanges();

                    var newTestQuestion = new TestQuestion
                    {
                        TestID = selectedTestId,
                        QuestionID = newQuestion.ID
                    };

                    db.TestQuestions.Add(newTestQuestion);
                    db.SaveChanges();

                    MessageBox.Show("Вопрос успешно добавлен!");
                }
                else
                {
                    
                    var existingQuestion = db.Questions.FirstOrDefault(q => q.ID == currentQuestion.ID);
                    if (existingQuestion != null)
                    {
                        existingQuestion.QuestionType = difficult;
                        existingQuestion.QuestionText = questionText;
                        existingQuestion.Answer1 = answer1;
                        existingQuestion.Answer2 = answer2;
                        existingQuestion.Answer3 = answer3;
                        existingQuestion.Answer4 = answer4;
                        existingQuestion.CorrectAnswer = correctAnswer;

                        
                        var testQuestion = db.TestQuestions.FirstOrDefault(tq => tq.QuestionID == currentQuestion.ID);
                        if (testQuestion != null)
                        {
                            testQuestion.TestID = selectedTestId;
                        }

                        db.SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены!");
                    }
                    else
                    {
                        MessageBox.Show("Вопрос не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                
                FrameManager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

    }
}