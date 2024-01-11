using Model.Dao;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCourse.Common
{
    public class UserAnswer
    {
        public ExamQuestion Question { get;  }
        public List<QuestionAnswer> Answers { get;  }

        public int ChosenAnswerId { get;  }
        public bool IsTrueAnwser { get;  }

        public UserAnswer(ExamQuestion question, List<QuestionAnswer> answers, int chosenAnswerId) 
        {
            IsTrueAnwser = false;
            IsTrueAnwser = new ExamQuestionDao().isTrueAnswer(question.ID, chosenAnswerId);
            ChosenAnswerId = chosenAnswerId;

            Question = question;
            Answers = answers;
        }

    }
}