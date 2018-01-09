using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using Newtonsoft.Json;

namespace HelpVoiceRec
{
    public partial class Form1 : Form
    {
        GrammarEditorForm grammarEditorForm;
        Listen listen;
        Speak speak;

        public Form1()
        {
            InitializeComponent();

            this.speak = new Speak(this.speakText);
            this.listen = new Listen(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            speak.SpeakIt("Witaj w Help Voice Recognition");
            speak.SpeakIt("Program umożliwia symulacje automatu obsługującego zgłoszenia o wypadkach");
            speak.SpeakIt("Proszę wybrać plik XML z zawartą gramatyką");
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            speak.SpeakIt(Logic.Instance.Collection.CurrentQuestion.QuestionText);

            listen.StartListen();
            stopBtn.Enabled = true;
            startBtn.Enabled = false;
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            this.StopRecognition();
        }

        private void loadxmlBtn_Click(object sender, EventArgs e)
        {
            this.LoadContent(false);
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Systemy dialogowe\n\nKrystian MAGDZIARZ\nMikołaj MOTYSEK\nI6B1S4", "O aplikacji", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void odpPisemneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (odpPisemneToolStripMenuItem.Checked)
            {
                odpPisemneToolStripMenuItem.Checked = false;
                odpBox.Visible = false;
            }
            else
            {
                odpPisemneToolStripMenuItem.Checked = true;
                odpBox.Visible = true;
            }
        }

        private void OdpInput_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                Console.WriteLine("Wprowadzono odpowiedz");
            }
        }

        private void odpInput_TextChanged(object sender, EventArgs e)
        {
        }

        private void nowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logic.Instance.Collection.ClearAll();
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog1.FileName != "")
            {
                grammarEditorForm = new GrammarEditorForm(saveFileDialog1.FileName);
                grammarEditorForm.Show();
            }
        }

        private bool LoadContent(Boolean showEditor)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Pliki słownika|*.hvrd";
            openFileDialog1.Title = "Wybierz plik słownika";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Logic.Instance.Collection.ClearAll();

                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                try
                {
                    Logic.Instance.Collection = JsonConvert.DeserializeObject<LogicCollection>(sr.ReadToEnd());
                    sr.Close();

                    if (Logic.Instance.Collection.QuestionCollection.Count > 0)
                    {
                        Logic.Instance.Collection.AnswerCollection.Reverse();
                        Logic.Instance.Collection.QuestionCollection.Reverse();

                        Console.WriteLine(Logic.Instance.Collection.CurrentQuestion.QuestionText + Logic.Instance.Collection.CurrentQuestion.possibleAnswers.Count.ToString());

                        var choices = Logic.Instance.Collection.CurrentQuestion.GetChoicesFromAnswers();
                        if (choices.Count() > 0)
                        {
                            startBtn.Enabled = true;
                            loadxmlBtn.Enabled = false;

                            listen.AddChoices(choices);
                        }
                        else
                        {
                            MessageBox.Show("Pierwsze pytanie nie posiada odpowiedzi", "Błąd wczytywania", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Nie wczytano żadnych pytań (0)", "Błąd wczytywania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                } catch (Exception e)
                {
                    MessageBox.Show("Nie wczytano żadnych pytań", "Błąd wczytywania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(e.ToString());
                    return false;
                }

                if (showEditor)
                {
                    grammarEditorForm = new GrammarEditorForm(openFileDialog1.FileName);
                    grammarEditorForm.Show();
                }

                return true;
            }

            return false;
        }

        private void wczytajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadContent(true);
        }

        private void odpInput_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                var newQuestion = Logic.Instance.Collection.CurrentQuestion.possibleAnswers.Find(x => x.AnswerText == odpInput.Text);

                if (newQuestion.nextQuestion != null)
                {
                    Logic.Instance.Collection.CurrentQuestion = newQuestion.nextQuestion;
                    speak.SpeakIt(newQuestion.nextQuestion.QuestionText);
                }

                odpInput.Clear();
            }
        }

        public Speak GetCurrentSpeak()
        {
            return speak;
        }

        public void StopRecognition()
        {
            speak.SpeakIt("Zakończono działanie");

            listen.StopListen();
            startBtn.Enabled = true;
            stopBtn.Enabled = false;
        }
    }
}
