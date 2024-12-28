using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WordSearchApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            string wordToFind = WordTextBox.Text;

            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(wordToFind))
            {
                MessageBox.Show("Пожалуйста, укажите файл и слово для поиска.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Указанный файл не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                SearchButton.IsEnabled = false;
                ResultTextBlock.Text = "Выполняется поиск...";

                int count = await CountWordOccurrencesAsync(filePath, wordToFind);
                ResultTextBlock.Text = $"Слово \"{wordToFind}\" встретилось {count} раз(а).";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SearchButton.IsEnabled = true;
            }
        }

        private async Task<int> CountWordOccurrencesAsync(string filePath, string word)
        {
            return await Task.Run(() =>
            {
                string content = File.ReadAllText(filePath);
                return content.Split(new[] { ' ', '\n', '\r', '\t', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                              .Count(w => string.Equals(w, word, StringComparison.OrdinalIgnoreCase));
            });
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Диалог выбора файла
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
