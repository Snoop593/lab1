using System.Collections.Generic;

namespace l1
{

    public class Group
    {
        public string name { get; set; }
        public string[] header { get; set; }
        public List<Student> students { get; set; }
        public Dictionary<string, double> subjects { get; set; }
        public Dictionary<string, double> averageMarks { get; set; }


        public Group(List<string[]> data, string name = "ИП-41"){
            this.name = name;
            header = data[0];
            data.Remove(data[0]);

            subjects = new Dictionary<string, double>();
            averageMarks = new Dictionary<string, double>();
            students = new List<Student>();


            for (int i = 3; i < header.Length; i++) {
                subjects.Add(header[i], 0);
                averageMarks.Add(header[i], 0);
            }

            subjects.Add("Средний", 0);
            averageMarks.Add("Средний", 0);

            foreach (string[] line in data){
                Student student = new Student(line[0], line[1], line[2]);
                for (int i = 3; i < header.Length; i++) {
                    Subject subj = new Subject(header[i], double.Parse(line[i]));
                    student.AddSubj(subj);
                    subjects[header[i]] += double.Parse(line[i]);
                    averageMarks[header[i]] = subjects[header[i]] / (students.Count + 1);
                }

                subjects["Средний"] += student.averageMark;
                averageMarks["Средний"] = subjects["Средний"] / (students.Count + 1);
                students.Add(student);
            }
        }
    }
}
