using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace VoiceXMLParser
{
    public class XMLParser
    {
        public List<FormXML> formsList { get; set; }
        public StreamReader sr { get; set; }

        public XMLParser(StreamReader sr)
        {
            this.formsList = new List<FormXML>();
            this.sr = sr;
            this.LoadFile();
        }

        private void LoadFile()
        {
            var is_form = false;
            var is_item = false;
            var is_prompt = false;
            var go_to_next = 0;

            FormXML form = new FormXML();
            PromptXML prompt = new PromptXML();
            ItemXML item = new ItemXML();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            using (XmlReader reader = XmlReader.Create(this.sr, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "item":
                                    is_item = true;
                                    item = new ItemXML();
                                    break;
                                case "prompt":
                                    is_prompt = true;
                                    prompt = new PromptXML();
                                    break;
                                case "form":
                                    is_form = true;
                                    form = new FormXML();
                                    form.ID = reader.GetAttribute("id");
                                    break;
                                case "goto":
                                    var next = reader.GetAttribute("next");

                                    if (form.ItemList.Count > go_to_next)
                                        form.ItemList[go_to_next].NextID = this.GetElementId(next);

                                    ++go_to_next;
                                    break;
                            }
                            break;
                        case XmlNodeType.Text:
                            if (is_item)
                                item.Answer = RemoveWhiteSpaces(reader.Value);
                            else if (is_prompt)
                                prompt.Prompt = RemoveWhiteSpaces(reader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            switch (reader.Name)
                            {
                                case "item":
                                    is_item = false;
                                    form.ItemList.Add(item);
                                    break;
                                case "prompt":
                                    is_prompt = false;
                                    form.Prompt = prompt;
                                    break;
                                case "form":
                                    is_form = false;
                                    go_to_next = 0;
                                    formsList.Add(form);
                                    break;
                            }
                            break;
                    }

                }
            }

            this.AssiociateAll();
        }

        private string RemoveWhiteSpaces(string s)
        {
            return Regex.Replace(s, @"\t|\n|\r", "");
        }

        private string GetElementId(string s)
        {
            return s.Replace("#", "");
        }

        public void AssiociateAll()
        {
            foreach(FormXML form in formsList)
            {
                foreach(ItemXML item in form.ItemList)
                {
                    try
                    {
                        item.Next = formsList.Find(x => x.ID == item.NextID);
                    }
                    catch (Exception e)
                    {
                        item.Next = null;
                    }
                }
            }
        }
    }

    public class FormXML
    {
        private string _id = string.Empty;

        public List<ItemXML> ItemList { get; set; } 
        public string ID { get { return _id; } set { _id = value;  } }
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
