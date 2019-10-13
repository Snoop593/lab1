using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace l1
{
    public class Task
    {
        public static void Run(string inputFile, string outputFile = null, string fileType = null) {
            List<string[]> data = ReadStudentsGroup(inputFile);
            Group group = new Group(data);
            Console.WriteLine(group);
            if (fileType != null && outputFile != null) {
                if (fileType.ToUpper() == "JSON"){
                    SaveStudentsGroupJSON(group, outputFile);
                }
                else if (fileType.ToUpper() == "XLSX"){
                    SaveStudentsGroupExcel(group, outputFile);
                }
            }
        }

        public static List<string[]> ReadStudentsGroup(string path){
            List<string[]> stringCSV = new List<string[]>();
            try{
                using (StreamReader reader = new StreamReader(path)){
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        string[] X = CSVParser.Split(line);
                        stringCSV.Add(X);
                    }
                }
            }
            catch{
                Console.WriteLine("Неверный формат файла или путь к нему.");
            }
            return stringCSV;
        }

        public static void SaveStudentsGroupJSON(Group group, string path)
        {
            string json = JsonConvert.SerializeObject(group);
            try{
                File.WriteAllText(path, json);
            }
            catch{
                System.Console.WriteLine("Неверное имя или путь к файлу.");
            }
        }

        public static void SaveStudentsGroupExcel(Group group, string path)
        {
            var newFile = new FileInfo(path);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile)){
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Group - " + DateTime.Now.ToString());
                worksheet.Cells[1, 1].Value = "Группа:";
                worksheet.Cells[1, 2].Value = group.name;
                for (int i = 0; i < group.header.Length; i++){
                    worksheet.Cells[2, i + 1].Value = group.header[i];
                }
                worksheet.Cells[2, group.header.Length + 1].Value = "Средний балл";
                int lastRow = 3;
                for (int i = 0, row = 3; i < group.students.Count; i++, row++){
                    int col = 1;
                    worksheet.Column(col).Width = 16;
                    worksheet.Cells[row, col].Value = group.students[i].name;
                    col++;
                    worksheet.Column(col).Width = 16;
                    worksheet.Cells[row, col].Value = group.students[i].surname;
                    col++;
                    worksheet.Column(col).Width = 18;
                    worksheet.Cells[row, col].Value = group.students[i].sursurname;
                    col++;
                    worksheet.Column(col).Width = 16;
                    for (int j = 0; j < group.students[i].subjects.Count; j++, col++){
                        worksheet.Cells[row, col].Value = group.students[i].subjects[j].mark;
                        worksheet.Column(col).Width = 16;
                    }
                    worksheet.Cells[row, col].Value = group.students[i].averageMark;
                    worksheet.Column(col).Width = 16;
                    lastRow = row;
                }
                lastRow++;
                worksheet.Cells[lastRow, 3].Value = "Средний по группе:";
                for (int i = 0; i < group.subjects.Count - 1; i++){
                    worksheet.Cells[lastRow, i + 4].Value = group.averageMarks[group.header[i + 3]];
                }
                worksheet.Cells[lastRow, group.subjects.Count + 3].Value = group.averageMarks["Средний"];
                try{
                    if (File.Exists(path)){
                        File.Delete(path);
                    }
                    xlPackage.Save();
                }
                catch{
                    System.Console.WriteLine("Неверное имя или путь к файлу.");
                }
            }
        }
    }
}
