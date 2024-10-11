using System.Collections.Generic;

namespace StudUchet.Classes
{
    public abstract class StudentBase : IStudent
    {
        public string Name { get; set; }
        public int Course { get; set; }
        public List<double> Grades { get; set; }

        public StudentBase(string name, int course, List<double> grades)
        {
            Name = name;
            Course = course;
            Grades = grades;
        }

        public double CalculateAverageGrade()
        {
            if (Grades == null || Grades.Count == 0) return 0;
            double total = 0;
            foreach (var grade in Grades)
            {
                total += grade;
            }
            return total / Grades.Count;
        }

        public abstract string GetCourseInfo(); // Реализация будет в классах специализаций
    }
}
