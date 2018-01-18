using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpVoiceRec
{
    class Logic
    {
        /**
         * Singleton do przechowywania elementów kolekcji
         */

        private static Logic instance;

        private Logic() { }

        [JsonIgnore]
        public static Logic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logic();
                }
                return instance;
            }
        }

        public LogicCollection Collection
        {
            get
            {
                return logicCollection;
            }

            set
            {
                logicCollection = value;
            }
        }

        private LogicCollection logicCollection = new LogicCollection();
    }

    public class LogicCollection
    {
        public List<QuestionCollection> QuestionCollection
        {
            get
            {
                return questionLogic;
            }
            set
            {
                this.questionLogic = value;
            }
        }

        [JsonIgnore]
        public List<AnswerCollection> AnswerCollection
        {
            get
            {
                return answerLogic;
            }
            set
            {
                this.answerLogic = value;
            }
        }

        [JsonIgnore]
        public QuestionCollection CurrentQuestion
        {
            get
            {
                if (currentQuestion == null && questionLogic.Count > 0)
                    currentQuestion = questionLogic.First();

                return currentQuestion;
            }
            set
            {
                currentQuestion = value;
            }
        }

        public void ClearAll()
        {
            questionLogic.Clear();
            questionLogic = new List<HelpVoiceRec.QuestionCollection>();

            answerLogic.Clear();
            answerLogic = new List<HelpVoiceRec.AnswerCollection>();
        }

        private List<QuestionCollection> questionLogic = new List<QuestionCollection>();
        private List<AnswerCollection> answerLogic = new List<AnswerCollection>();
        private QuestionCollection currentQuestion = null;

    }

    public class QuestionCollection
    {
        public string ID = String.Empty;
        public List<AnswerCollection> possibleAnswers = new List<AnswerCollection>();
        public string QuestionText { get; set; }

        public string[] GetChoicesFromAnswers()
        {
            if (possibleAnswers.Count > 0)
            {
                string[] choices = new string[possibleAnswers.Count];

                for(var i=0; i<possibleAnswers.Count; ++i)
                {
                    choices[i] = possibleAnswers[i].AnswerText;
                }

                return choices;
            }
            else
                return new string[0];
        }

        public string GetChoicesString(string separator)
        {
            string value = "";
            var items = this.GetChoicesFromAnswers();

            foreach (var item in items)
            {
                value += item + " " + separator;
            }

            if(value.EndsWith(separator))
                value = value.Substring(0, value.Length - 1);

            return value;
        }
    }

    public class AnswerCollection
    {
        public QuestionCollection nextQuestion = null;
        public string AnswerText { get; set; }
    }
}
