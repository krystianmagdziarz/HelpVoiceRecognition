using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceXMLParser;

namespace HelpVoiceRec
{
    public class Helper
    {

        public LogicCollection TranslateFromXMLToLogicCollection(List<FormXML> formList)
        {
            LogicCollection logicCollection = new LogicCollection();

            foreach(FormXML form in formList)
            {
                logicCollection.QuestionCollection.Add(FillForm(form));
            }

            return logicCollection;
        }

        private QuestionCollection FillForm(FormXML form)
        {
            QuestionCollection questionCollection = new QuestionCollection();
            questionCollection.QuestionText = form.Prompt.Prompt;

            foreach (ItemXML itemxml in form.ItemList)
            {
                AnswerCollection answerCollection = new AnswerCollection();
                answerCollection.AnswerText = itemxml.Answer;

                if (itemxml.Next != null)
                    answerCollection.nextQuestion = FillForm(itemxml.Next);
                else
                    answerCollection.nextQuestion = null;

                questionCollection.possibleAnswers.Add(answerCollection);
            }

            return questionCollection;
        }

    }
}
