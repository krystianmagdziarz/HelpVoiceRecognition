using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpVoiceRec
{
    public partial class Question : Form
    {
        GrammarEditorForm grammarEditorForm;

        public Question(GrammarEditorForm grammarEditorForm)
        {
            this.grammarEditorForm = grammarEditorForm; 
            InitializeComponent();
            this.LoadComponents();
        }

        private void LoadComponents()
        {
            listBox1.ClearSelected();
            listBox1.Items.Clear();

            foreach (AnswerCollection answer in Logic.Instance.Collection.AnswerCollection)
            {
                listBox1.Items.Add(answer);
            }

            listBox1.DisplayMember = "AnswerText";
            listBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QuestionCollection question = new QuestionCollection() { QuestionText = richTextBox1.Text };


            foreach(AnswerCollection element in listBox1.SelectedItems)
            {
                    question.possibleAnswers.Add(element);
            }

            if (richTextBox1.Text.Count() > 0)
            {
                Logic.Instance.Collection.QuestionCollection.Add(question);
                grammarEditorForm.RefreshQuestionList();
            }

            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}
