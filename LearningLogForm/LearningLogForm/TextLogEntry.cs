/*
 Author:        Varush Sivalingam
 Created:       November 2nd, 2024
 Updated:       November 18th, 2024
 Description:   TextLog class inherits form Log Entry
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;


namespace LearningLogForm
{
    internal class TextLogEntry : LogEntry
    {
        public string Text { get; private set; }

        public TextLogEntry(int wellnessValue, int qualityValue, string notesValue, string text) : base(wellnessValue, qualityValue, notesValue)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("Text cannot be empty. ", nameof(text));
            }
            Text = text;
        }

        public override string ToString()
        {
            return base.ToString() + $" [Text: {Text}]";
        }
    }
}
