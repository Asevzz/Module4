using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using StudUchet.Classes;

namespace StudUchet
{
    public partial class MainWindow : Window
    {
        private List<StudentBase> students = new List<StudentBase>();
        private string dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "students.txt");

        public MainWindow()
        {
            InitializeComponent();
            EnsureDataDirectoryExists();
            LoadDataFromFile();
            SpecializationComboBox.ItemsSource = new List<string> { "НО", "ДО", "ИЯ", "ФК", "ПОИТ" };
        }

        private void EnsureDataDirectoryExists()
        {
            // Проверяем и создаем директорию для данных, если её нет
            string directoryPath = Path.GetDirectoryName(dataFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private void SpecializationComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SpecializationComboBox.SelectedItem != null)
            {
                string specialization = SpecializationComboBox.SelectedItem.ToString();
                List<int> availableCourses = GetAvailableCourses(specialization);
                CourseComboBox.ItemsSource = availableCourses;
                CourseComboBox.SelectedIndex = 0; // Устанавливаем первый курс по умолчанию
            }
        }

        private List<int> GetAvailableCourses(string specialization)
        {
            // Возвращаем список курсов в зависимости от выбранной специальности
            switch (specialization)
            {
                case "НО":
                case "ДО":
                case "ФК":
                    return new List<int> { 1, 2, 3 };
                case "ИЯ":
                case "ПОИТ":
                    return new List<int> { 1, 2, 3, 4 };
                default:
                    return new List<int> { 1 };
            }
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            string specialization = SpecializationComboBox.SelectedItem != null ? SpecializationComboBox.SelectedItem.ToString() : string.Empty;
            int course = CourseComboBox.SelectedItem != null ? int.Parse(CourseComboBox.SelectedItem.ToString()) : 1;
            string studentName = StudentNameTextBox.Text;
            List<double> grades = ParseGrades(GradesTextBox.Text);

            StudentBase student = CreateStudent(specialization, studentName, course, grades);
            if (student != null)
            {
                students.Add(student);
                StudentList.Items.Add($"{student.Name} - {student.GetCourseInfo()} - Средний балл: {student.CalculateAverageGrade():F2}");
            }
        }

        private List<double> ParseGrades(string gradesText)
        {
            // Разбиваем строку с оценками и преобразуем их в список double
            return gradesText
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(g => double.TryParse(g.Trim(), out var result) ? result : 0)
                .ToList();
        }

        private StudentBase CreateStudent(string specialization, string name, int course, List<double> grades)
        {
            // Создаем объект студента в зависимости от специальности
            switch (specialization)
            {
                case "ПОИТ":
                    return new StudentPOIT(name, course, grades);
                case "НО":
                    return new StudentNO(name, course, grades);
                case "ДО":
                    return new StudentDO(name, course, grades);
                case "ИЯ":
                    return new StudentIYA(name, course, grades);
                case "ФК":
                    return new StudentFK(name, course, grades);
                default:
                    return null;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем данные студентов в файл
            try
            {
                using (StreamWriter writer = new StreamWriter(dataFilePath))
                {
                    foreach (var student in students)
                    {
                        writer.WriteLine($"{student.GetType().Name};{student.Name};{student.Course};{string.Join(",", student.Grades)}");
                    }
                }
                MessageBox.Show("Данные успешно сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataFromFile()
        {
            // Загружаем данные студентов из файла, если он существует
            if (!File.Exists(dataFilePath)) return;

            try
            {
                foreach (var line in File.ReadAllLines(dataFilePath))
                {
                    var parts = line.Split(';');
                    if (parts.Length < 4) continue;

                    string studentType = parts[0];
                    string name = parts[1];
                    int course = int.Parse(parts[2]);
                    List<double> grades = parts[3].Split(',').Select(double.Parse).ToList();

                    StudentBase student = CreateStudentFromType(studentType, name, course, grades);
                    if (student != null)
                    {
                        students.Add(student);
                        StudentList.Items.Add($"{student.Name} - {student.GetCourseInfo()} - Средний балл: {student.CalculateAverageGrade():F2}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private StudentBase CreateStudentFromType(string studentType, string name, int course, List<double> grades)
        {
            // Создаем объект студента на основе типа, сохраненного в файле
            switch (studentType)
            {
                case nameof(StudentPOIT):
                    return new StudentPOIT(name, course, grades);
                case nameof(StudentNO):
                    return new StudentNO(name, course, grades);
                case nameof(StudentDO):
                    return new StudentDO(name, course, grades);
                case nameof(StudentIYA):
                    return new StudentIYA(name, course, grades);
                case nameof(StudentFK):
                    return new StudentFK(name, course, grades);
                default:
                    return null;
            }
        }
        private void ClearListButton_Click(object sender, RoutedEventArgs e)
{
    // Подтверждение действия пользователя перед очисткой списка
    var result = MessageBox.Show("Вы уверены, что хотите очистить весь список студентов? Это действие нельзя будет отменить.",
                                 "Подтверждение очистки",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Warning);

    if (result == MessageBoxResult.Yes)
    {
        // Очищаем список студентов и интерфейсное отображение
        students.Clear();
        StudentList.Items.Clear();

        // Очищаем файл данных
        try
        {
            File.WriteAllText(dataFilePath, string.Empty);
            MessageBox.Show("Список студентов успешно очищен!", "Очистка", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при очистке файла данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
    }
}
