using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;

namespace Dnevnik
{

    [Table(Name = "Lessons")]
    public class Lesson
    {
        [Column(IsPrimaryKey = true)]
        public int LessonID; //1-48
        [Column]
        public string Name;
        [Column]
        public string Notes;
    }
    [Table(Name = "HomeworkPoints")]
    public class HomeworkPoint
    {
        [Column(IsPrimaryKey = true)]
        public int HomeworkPointID; 
        [Column]
        public int LessonID;
        [Column]
        public string Description;
        [Column]
        public bool isDone;

    }
    class ConnectionWithDataBase
    {
        public DataContext db;
        public Table<Lesson> Lessons;
        public Table<HomeworkPoint> HomeworkPoints;
        public int ChoosenLessonID;
        public ConnectionWithDataBase()
        {
            db = new DataContext("C:\\lessons\\lessonsdatabase.mdf");
            Lessons = db.GetTable<Lesson>();
            HomeworkPoints = db.GetTable<HomeworkPoint>();
            ChoosenLessonID = 0;
        }
        public string[] GetAllLessons()
        {
            string[] result = new string[48];
            int index = 0;
            foreach(Lesson les in Lessons)
            {
                result[index] = les.Name;
                ++index;
            }
            return result;
        }

        public struct HomeworkPointInfo
        {
            public string Name;
            public bool IsDone;
        }
        public HomeworkPointInfo[] GetLessonHomeworkPoints()
        {
            HomeworkPointInfo[] result = new HomeworkPointInfo[5];
            int index = 0;
            foreach(HomeworkPoint hw in HomeworkPoints)
            {
                if(hw.LessonID == ChoosenLessonID)
                {
                    HomeworkPointInfo hwpi = new HomeworkPointInfo();
                    hwpi.Name = hw.Description;
                    hwpi.IsDone = hw.isDone;
                    result[index] = hwpi;
                    ++index;
                }
            }
            return result;
        }
        public void ChangeNote(string note)
        {
            foreach(Lesson les in Lessons)
            {
                if (les.LessonID == ChoosenLessonID)
                {
                    les.Notes = note;
                }
            }
            db.SubmitChanges();
        }
        public string GetNote()
        {
            foreach(Lesson les in Lessons)
            {
                if (les.LessonID == ChoosenLessonID)
                {
                    return les.Notes;
                }
            }
            return "";
        }
        private void DeleteHW(HomeworkPoint hw)
        {
            hw.Description = "";
            hw.isDone = false;
        }
        public void SubmitLessonsChanges(string[] changes)
        {
            int index = 0;
            foreach(Lesson les in Lessons)
            {
                if (les.Name != changes[index])
                {
                    foreach(HomeworkPoint hw in HomeworkPoints)
                    {
                        if (hw.LessonID == (index+1))
                        {
                            DeleteHW(hw);
                        }
                    }
                    les.Notes = "";
                }
                les.Name = changes[index];
                ++index;
            }
            db.SubmitChanges();
        }
        public void SubmitHomeworkPointChanges(HomeworkPointInfo[] hwpi) //TODO
        {
            int hwpindex = (ChoosenLessonID - 1) * 5 + 1;
            foreach (HomeworkPoint les in HomeworkPoints)
            {
                
            }
            db.SubmitChanges();
        }
    }

}
