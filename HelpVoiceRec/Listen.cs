using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using DBConnector;

namespace HelpVoiceRec
{
    class Listen
    {
        OwnCommands ownCommands;
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        Choices choices = new Choices();
        GrammarBuilder grammarBuilder = new GrammarBuilder();
        Speak speak;
        Form1 form1;

        public Listen(Form1 form1)
        {
            TimeSpan ts = new TimeSpan(0);
            engine.EndSilenceTimeout = ts;
            engine.InitialSilenceTimeout = ts;
            engine.LoadGrammarCompleted += Engine_LoadGrammarCompleted;
            engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Engine_SpeechRecognized);
            engine.SpeechDetected += Engine_SpeechDetected;
            engine.SpeechRecognitionRejected += Engine_SpeechRecognitionRejected;
            engine.SetInputToDefaultAudioDevice();

            engine.AudioStateChanged += Engine_AudioStateChanged;

            this.speak = form1.GetCurrentSpeak();
            this.form1 = form1;

            ownCommands = new OwnCommands();
        }

        private void Engine_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            Console.WriteLine("Nadawanie");
        }

        public void AddChoices(string[] value)
        {
            if (value.Count() > 0)
            {
                engine.UnloadAllGrammars();
                choices = new Choices();
                choices.Add(value);
                grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(choices);
                Grammar grammar = new Grammar(grammarBuilder);
                engine.LoadGrammarAsync(grammar);
            }
            else
            {
                this.form1.StopRecognition();
            }
        }

        private void Engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            ownCommands.SaveActivity(this.form1.GetCallerID(), Logic.Instance.Collection.CurrentQuestion.QuestionText, e.Result.Text);

            var newQuestion = Logic.Instance.Collection.CurrentQuestion.possibleAnswers.Find(x => x.AnswerText == e.Result.Text);

            if (newQuestion != null && newQuestion.nextQuestion != null)
            {
                Logic.Instance.Collection.CurrentQuestion = newQuestion.nextQuestion;
                speak.SpeakIt(Logic.Instance.Collection.CurrentQuestion.QuestionText);

                var choices = Logic.Instance.Collection.CurrentQuestion.GetChoicesFromAnswers();

                if(choices.Count() > 0)
                {
                    AddChoices(choices);
                    this.form1.setPossibleAnswers(Logic.Instance.Collection.CurrentQuestion.GetChoicesString("|"));
                }
                else
                {
                    this.form1.setPossibleAnswers("");
                    this.form1.StopRecognition();
                }
            }
            else
            {
                engine.UnloadAllGrammars();
                form1.setPossibleAnswers("");
            }
            Console.WriteLine(e.Result.Text);
        }

        private void Engine_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            Console.WriteLine("Wykryto szum");
        }

        private void Engine_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
        {
            Console.WriteLine("Wczytano gramtyke");
        }

        private void Engine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Nie rozpoznano");
        }

        public void StartListen()
        {
            this.form1.setPossibleAnswers(Logic.Instance.Collection.CurrentQuestion.GetChoicesString("|"));
            engine.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopListen()
        {
            engine.RecognizeAsyncStop();
        }
    }
}
