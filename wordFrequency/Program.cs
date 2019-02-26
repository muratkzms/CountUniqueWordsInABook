//Created by Murat Kızmış
//Software Engineer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;

namespace SimpleTermFrequencyAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            downloadFile();
            XmlDocument kitap = new XmlDocument();
            XmlNode root = kitap.CreateElement("words");
            kitap.AppendChild(root);
            
            string filename = @"d:\file.txt";
            string inputString = File.ReadAllText(filename);
            
            inputString = inputString.ToLower();
                        
            string[] stripChars = { ";", ",", ".", "-", "_", "^", "(", ")", "[", "]",
                        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "\n", "\t", "\r", "!", "\""};
            foreach (string character in stripChars)
            {
                inputString = inputString.Replace(character, "");
            }
            
            List<string> wordList = inputString.Split(' ').ToList();

            string[] stopwords = new string[] { "and", "the", "she", "for", "this", "you", "but" };
            foreach (string word in stopwords)
            {
                while (wordList.Contains(word))
                {
                    wordList.Remove(word);
                }
            }
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach (string word in wordList)
            {
                if (word.Length >= 3)
                {
                    if (dictionary.ContainsKey(word))
                    {
                        // zaten kelime varsa 1 arttır
                        dictionary[word]++;
                    }
                    else
                    {
                        // yoksa 1 e eşitle
                        dictionary[word] = 1;
                    }
                }
            }
            // sıralı index
            var sortedDict = (from entry in dictionary orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                        
            int count = 1;
            Console.WriteLine("---- Most Frequent Terms in the File: " + filename + " ----");
            Console.WriteLine();
            foreach (KeyValuePair<string, int> pair in sortedDict)
            {
                // Ekrana yaz sıralı listeyi
                Console.WriteLine(count + "\t"+"Count: "+pair.Value+"\t" + pair.Key);


                XmlNode Woord = kitap.CreateElement("word");

                //XmlAttribute ID = kitap.CreateAttribute("ID");
                XmlAttribute Text1 = kitap.CreateAttribute("text");
                XmlAttribute Count1 = kitap.CreateAttribute("count");

                //ID.Value = Sayi.ToString();
                Text1.Value = pair.Key;
                Count1.Value = pair.Value.ToString();

                Woord.Attributes.Append(Text1);
                Woord.Attributes.Append(Count1);
                root.AppendChild(Woord);

                count++;

                //if (count > 100)
                //{
                //    break;
                //}
            }
            kitap.Save(@"D:\Files\XML\kitap.xml");
            Console.WriteLine("XML File is Created (Murat Kızmış)");
            Console.ReadKey();

        }

        public static void downloadFile()
        {
            WebClient wc = new WebClient();
        wc.DownloadFile("http://www.gutenberg.org/files/2701/2701-0.txt", @"d:\file.txt");
        }

    } 

}