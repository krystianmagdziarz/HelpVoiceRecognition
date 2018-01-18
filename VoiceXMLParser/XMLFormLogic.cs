using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceXMLParser
{
    public class FormXML
    {
        private string _id = string.Empty;

        public List<ItemXML> ItemList { get; set; }
        public string ID { get { return _id; } set { _id = value; } }
        public PromptXML Prompt { get; set; }

        public FormXML()
        {
            this.ItemList = new List<ItemXML>();
        }
    }

    public class ItemXML
    {
        public string Answer { get; set; }
        public string NextID { get; set; }
        public FormXML Next { get; set; }
    }

    public class PromptXML
    {
        public string Prompt { get; set; }
    }
}
