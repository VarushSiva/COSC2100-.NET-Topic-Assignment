/*
 Author:        Varush Sivalingam + Professor Kyle Chapman
 Created:       November 2nd, 2024
 Updated:       November 18th, 2024
 Description:   Learning log program that stores notes, state of wellness, quality
                as well as a filepath to recording.
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
    internal abstract class LogEntry
    {
        #region "Variable Declarations"
        // Static Variables
        protected static int count = 0;
        protected static DateTime firstEntry;
        protected static DateTime newestEntry;

        // Instance Variables
        protected int logID;
        protected DateTime logDate = DateTime.Now;
        protected int logWellness;
        protected int logQuality;
        protected string logNotes;

        // - a unique identifying number accessed via a property named Id;
        // - a date indicating when the audio was recorded accessed via a property named EntryDate;
        // - a number based on the “Wellness/Mood” ComboBox accessed via a property named Wellness;
        // - a number based on the “Quality” ComboBox access via a property named Quality;
        // - a piece of text from the “notes” TextBox accessed via a property named Notes; and
        // - a FileInfo object named RecordingFile, which will include the file location of the audio file.

        #endregion

        #region "Contructors"
        // There should be a default constructor as well as a parametrized constructor that will take four parameters: wellness, quality, notes, and FileInfo.

        // Increase count and then sets it to logID

        protected LogEntry(int wellnessValue, int qualityValue, string notesValue)
        {
            // Update the static variables.
            count++;
            if (count == 1)
            {
                firstEntry = logDate;
            }
            newestEntry = logDate;

            // Set Values for the instanced properties
            logID = count;
            Wellness = wellnessValue;
            Quality = qualityValue;
            Notes = notesValue;
        }
        #endregion

        #region "Properties"

        public int ID { get => logID; private set => logID = value;}

        public DateTime EntryDate { get => logDate; set => logDate = value; }

        public int Wellness { get => logWellness;   set => logWellness = value; }

        public int Quality { get => logQuality; set => logQuality = value; }

        public string Notes 
        { 
            get => logNotes; 
            set => logNotes = value;
        }

        // There will be static properties to access the total number of LogEntry objects created(named Count), the date of the first entry(named FirstEntry), and the date of the most recent entry(named NewestEntry).
        public static int Count => count;

        public static DateTime FirstEntry => firstEntry;

        public static DateTime NewestEntry => newestEntry;
        #endregion

        #region "Methods"
        public override string ToString()
        {
            return "Entry " + ID + " created at " + EntryDate.ToString();
        }
        #endregion

        internal static class LogEntryList
        {
            public static List<LogEntry> List { get; } = new List<LogEntry>();
        }
    }
}
