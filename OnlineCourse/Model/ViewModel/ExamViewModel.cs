using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ExamViewModel
    {
        public VideoExam Exam { get; set; }
        public Dictionary<int, bool> ChosenQuestion { get; set; }

    }
}
