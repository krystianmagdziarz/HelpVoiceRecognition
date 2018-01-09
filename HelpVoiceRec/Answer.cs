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
    public partial class Answer : Form
    {
        GrammarEditorForm grammarEditorForm;

        public Answer(GrammarEditorForm grammarEditorForm)
        {
            this.grammarEditorForm = grammarEditorForm;
            InitializeComponent();
            this.LoadComponents();
        }

        private void LoadComponents()
        {
            listBox1.Items.Clear();
            foreach(QuestionCollection question in Logic.Instance.Collection.QuestionCollection)
            {
                listBox1.Items.Add(question);
            }

            listBox1.DisplayMember = "QuestionText";
            listBox1.Refresh();

            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnswerCollection answerCollection = new AnswerCollection();

            if (nullBox.Checked && listBox1.SelectedIndex == 0)
                answerCollection.nextQuestion = null;
            else if (listBox1.SelectedIndex >= 0)
                answerCollection.nextQuestion = (QuestionCollection)listBox1.SelectedItem;

            answerCollection.AnswerText = textBox1.Text;
            Logic.Instance.Collection.AnswerCollection.Add(answerCollection);
            grammarEditorForm.RefreshAnswerList();
            this.Close();
        }
    }
}
