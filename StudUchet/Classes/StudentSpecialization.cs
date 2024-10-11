using System.Collections.Generic;

namespace StudUchet.Classes
{
    public class StudentPOIT : StudentBase
    {
        public StudentPOIT(string name, int course, List<double> grades)
            : base(name, course, grades) { }

        public override string GetCourseInfo()
        {
            return $"Специальность: ПОИТ, Курс: {Course}, Всего курсов: 4";
        }
    }
    public class StudentFK : StudentBase
    {
        public StudentFK(string name, int course, List<double> grades)
            : base(name, course, grades) { }

        public override string GetCourseInfo()
        {
            return $"Специальность: ФК, Курс: {Course}, Всего курсов: 3";
        }
    }

    public class StudentIYA : StudentBase
    {
        public StudentIYA(string name, int course, List<double> grades)
            : base(name, course, grades) { }

        public override string GetCourseInfo()
        {
            return $"Специальность: ИЯ, Курс: {Course}, Всего курсов: 4";
        }
    }

    public class StudentNO : StudentBase
    {
        public StudentNO(string name, int course, List<double> grades)
            : base(name, course, grades) { }

        public override string GetCourseInfo()
        {
            return $"Специальность: НО, Курс: {Course}, Всего курсов: 3";
        }
    }

    public class StudentDO: StudentBase
    {
        public StudentDO(string name, int course, List<double> grades)
            : base(name, course, grades) { }

        public override string GetCourseInfo()
        {
            return $"Специальность: ДО, Курс: {Course}, Всего курсов: 3";
        }
    }
}