/*
 Author:        Varush Sivalingam
 Created:       November 2nd, 2024
 Updated:       November 18th, 2024
 Description:   AudioLog class inherits form Log Entry
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
    internal class AudioLogEntry : LogEntry
    {
        public FileInfo RecordingFile { get; private set; }

        public AudioLogEntry(int wellnessValue, int qualityValue, string notesValue, FileInfo recordingFile) : base(wellnessValue, qualityValue, notesValue)
        {
            if (!recordingFile.Exists)
            {
                throw new ArgumentException("Recording file does not exist.", nameof(recordingFile));
            }
            RecordingFile = recordingFile;
        }

        public override string ToString()
        {
            return base.ToString() + $" [Audio Recording: {RecordingFile.FullName}]";
        }
    }
}
