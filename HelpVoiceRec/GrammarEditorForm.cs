using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpVoiceRec
{
    public partial class GrammarEditorForm : Form
    {
        string fileName;

        public GrammarEditorForm(string fileName)
        {
            this.fileName = fileName;
            InitializeComponent();
            this.RefreshAnswerList();
            this.RefreshQuestionList();
        }

        private void SaveProgress()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                File.WriteAllText(fileName, "");
                File.WriteAllText(fileName, JsonConvert.SerializeObject(Logic.Instance.Collection));
            }
        }

        private void qstBtn_Click(object sender, EventArgs e)
        {
            Question questionForm = new Question(this);
            questionForm.Show();
        }

        private void ansBtn_Click(object sender, EventArgs e)
        {
            Answer answerForm = new Answer(this);
            answerForm.Show();
        }

        public void RefreshQuestionList()
        {
            listBox1.ClearSelected();
            listBox1.Items.Clear();
            foreach(QuestionCollection question in Logic.Instance.Collection.QuestionCollection)
            {
                listBox1.Items.Add(question);
            }

            listBox1.DisplayMember = "QuestionText";
            listBox1.Refresh();
            this.SaveProgress();
        }

        public void RefreshAnswerList()
        {
            listBox2.ClearSelected();
            listBox2.Items.Clear();

            foreach (AnswerCollection answer in Logic.Instance.Collection.AnswerCollection)
            {
                listBox2.Items.Add(answer);
            }

            listBox2.DisplayMember = "AnswerText";
            listBox2.Refresh();
            this.SaveProgress();
        }
    }
}
