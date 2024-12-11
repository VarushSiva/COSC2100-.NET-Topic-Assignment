/*
 Author:        Varush Sivalingam
 Created:       November 2nd, 2024
 Updated:       November 18th, 2024
 Description:   Learning Log to Save, Delete, Play recorded files.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LearningLogForm.LogEntry;

namespace LearningLogForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isRecording = false;
        FileInfo recordingFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!isRecording)
            {
                labelRecordText.Content = "St_op";
                isRecording = true;
                RecordWav.StartRecording();
                buttonSave.IsEnabled = false;
                buttonPlay.IsEnabled = false;
                buttonDelete.IsEnabled = false;
                UpdateStatus("Recording Started at " + DateTime.Now.ToString("yyyyMMdd") + ".");
            }
            else
            {
                labelRecordText.Content = "_Record";
                isRecording = false;
                recordingFile = RecordWav.EndRecording();
                buttonSave.IsEnabled = true;
                buttonPlay.IsEnabled = true;
                buttonDelete.IsEnabled = true;
                UpdateStatus("Recording completed and saved to " + recordingFile.FullName);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate ComboBox selection
                if (comboWellness.SelectedIndex == -1 || comboQuality.SelectedIndex == -1)
                {
                    throw new InvalidOperationException("You must select a Wellness/Mood and Quality before saving");
                }

                // Validate Notes
                if (string.IsNullOrWhiteSpace(textNotes.Text))
                {
                    throw new InvalidOperationException("Notes cannot be empty!");
                }
                int wellnessValue = comboWellness.SelectedIndex + 1;
                int qualityValue = comboQuality.SelectedIndex + 1;
                LogEntry newEntry = new AudioLogEntry(wellnessValue, qualityValue, textNotes.Text, recordingFile);
                LogEntryList.List.Add(newEntry);
                UpdateSummary();
                UpdateList();
                UpdateStatus(newEntry.ToString());
            }
            // Auto complete helped with exception handling
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
            
        }

        private void buttonSaveText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate ComboBox selection
                if (comboWellnessText.SelectedIndex == -1 || comboQualityText.SelectedIndex == -1)
                {
                    throw new InvalidOperationException("You must select a Wellness/Mood and Quality before saving");
                }

                // Validate Notes
                if (string.IsNullOrWhiteSpace(textEntry.Text))
                {
                    throw new InvalidOperationException("Text cannot be empty!");
                }
                int wellnessValue = comboWellnessText.SelectedIndex + 1;
                int qualityValue = comboQualityText.SelectedIndex + 1;
                LogEntry newEntry = new TextLogEntry(wellnessValue, qualityValue, textNotes.Text, textEntry.Text);
                LogEntryList.List.Add(newEntry);
                UpdateSummary();
                UpdateList();
                UpdateStatus(newEntry.ToString());

                buttonDeleteText.IsEnabled = true;
            }
            // Auto complete helped with exception handling
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }

        }

        private void UpdateStatus(string status)
        {
            statusState.Content = status;
        }

        private void textNotes_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            var player = new SoundPlayer(recordingFile.FullName);
            player.Play();
            UpdateStatus("Playing " + recordingFile.FullName + ".");
        }

        private void TabChanged(object sender, RoutedEventArgs e)
        {
            if (tabController.SelectedItem == tabEntry)
            {
                // Update the Entry tab fields
                UpdateStatus("Viewing Entry");
            }
            if (tabController.SelectedItem == tabSummary)
            {
                // Update the summary tab fields
                labelTotalEntries.Content = LogEntry.Count.ToString();
                // Check if Entry is set or uninitialized - Thanks to auto complete!
                labelFirstEntry.Content = LogEntry.FirstEntry != default ? LogEntry.FirstEntry.ToString() : "No Entries";
                labelNewestEntry.Content = LogEntry.NewestEntry != default ? LogEntry.NewestEntry.ToString() : "No Entries";
                UpdateStatus("Viewing Summary");
            }
            if (tabController.SelectedItem == tabList)
            {
                // Update the List tab fields
                listViewEntries.ItemsSource = LogEntryList.List;
                UpdateStatus("Viewing List of Entries");
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (recordingFile != null && recordingFile.Exists)
            {
                // I was trying to write something else and then autocomplete helped me finish this, did my research later to understand what this was.
                var entryToRemove = LogEntryList.List.OfType<AudioLogEntry>().FirstOrDefault(entry => entry.RecordingFile.FullName == recordingFile.FullName);

                if (entryToRemove != null)
                {
                    LogEntryList.List.Remove(entryToRemove);
                    recordingFile.Delete();
                    UpdateSummary();
                    UpdateList();
                    UpdateStatus("Recording Deleted.");
                }

                buttonPlay.IsEnabled = false;
                buttonDelete.IsEnabled = false;
                buttonSave.IsEnabled = false;
            }
        }

        private void buttonDeleteText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Thanks to autocomplete
                var entryToRemove = LogEntryList.List.OfType<TextLogEntry>().OrderByDescending(entry => entry.EntryDate).FirstOrDefault();

                if (entryToRemove != null)
                {
                    LogEntryList.List.Remove(entryToRemove);
                    UpdateSummary();
                    UpdateList();
                    UpdateStatus("Text Entry Deleted.");
                }

                buttonDeleteText.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void UpdateSummary()
        {
            labelTotalEntries.Content = LogEntryList.List.Count.ToString();
            if (LogEntryList.List.Any())
            {
                labelFirstEntry.Content = LogEntryList.List.Min(entry => entry.EntryDate).ToString();
                labelNewestEntry.Content = LogEntryList.List.Max(entry => entry.EntryDate).ToString();
            }
            else
            {
                labelFirstEntry.Content = "No Entries";
                labelNewestEntry.Content = "No Entries";
            }
            
        }

        private void UpdateList()
        {
            listViewEntries.ItemsSource = null;
            listViewEntries.ItemsSource = LogEntryList.List;
        }
    }
}
