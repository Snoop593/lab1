using System.Collections.Generic;

namespace l1
{
    public class Student
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string sursurname { get; set; }
        public double averageMark { get; set; }
        public List<Subject> subjects { get; set; }
        public Student(string name, string surname, string sursurname){
            this.name = name;
            this.surname = surname;
            this.sursurname = sursurname;
            subjects = new List<Subject>();
        }
        public double AverageMark(){
            double total = 0;
            foreach (Subject subj in subjects){
                total += subj.mark;
            }
            total /= subjects.Count;
            return total;
        }

        public void AddSubj(Subject subj){
            subjects.Add(subj);
            averageMark = AverageMark();
        }

    }
}
