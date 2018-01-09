using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Drawing;


namespace HelpVoiceRec
{
    public class Speak
    {
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        PromptBuilder promptBuilder = new PromptBuilder();

        Label speakText;

        public Speak(Label speakText)
        {
            this.speakText = speakText;
            speechSynthesizer.SpeakProgress += SpeechSynthesizer_SpeakProgress;
            speechSynthesizer.SpeakCompleted += SpeechSynthesizer_SpeakCompleted;
        }

        private void SpeechSynthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            speakText.ForeColor = Color.Black;
            speakText.Text = "...";
        }

        private void SpeechSynthesizer_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            speakText.ForeColor = Color.Black;
            speakText.Text = e.Text;

            if (e.Text == "Help" || e.Text == "Voice" || e.Text == "Recognition")
                speakText.ForeColor = Color.Red;
        }

        public void SpeakIt(string Text)
        {
            promptBuilder.ClearContent();
            promptBuilder.AppendText(Text);
            speechSynthesizer.SpeakAsync(promptBuilder);
        }
    }
}
