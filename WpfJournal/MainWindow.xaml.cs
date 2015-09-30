using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfJournal.Classes;

namespace WpfJournal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Journal currentJournal;
        private string journalTitle = "My Journal";
        private int maxEntryId = 0; // ID to put in journal entries
        private int displayedEntryId = 0; 

        public MainWindow()
        {
            InitializeComponent();
            // Create new instance of journal class
            currentJournal = new Journal(journalTitle);
            // Put journal title in window title bar
            this.Title = journalTitle;
            // Specify journal entries as item source for data grid
            dataGrid_JournalEntries.ItemsSource = currentJournal.Entries;
        }

        // Publish button clicked: add journal entry
        private void button_publish_Click(object sender, RoutedEventArgs e)
        {
            JournalEntry entry = new JournalEntry();

            entry.Date = DateTime.Now;
            entry.Title = textBox_title.Text;
            ++maxEntryId;
            entry.Id = maxEntryId;
            entry.Text = textBox_entry.Text;

            currentJournal.Entries.Add(entry);
        }

        // Delete button clicked: delete entry
        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            int entryID = 0;
            try
            {
                // Check that row is selected in grid
                if (dataGrid_JournalEntries.SelectedCells.Count > 0)
                {

                    // Get the entry ID of the selected row, by
                    //  creating a new instance of the JournalEntry class,
                    //   and copying the selected items collection to it 
                    //    (selected items collection must be cast)
/*
                    var selectedEntry = dataGrid_JournalEntries.SelectedItems;
                    foreach (var item in selectedEntry)
                    {
                        var rowEntry = item as JournalEntry;
                        entryID = rowEntry.Id;
                    }
*/
                    JournalEntry rowEntry = new JournalEntry();
                    if (dataGrid_JournalEntries.SelectedItem != null &&
                         dataGrid_JournalEntries.SelectedItem is JournalEntry)
                    {
                        rowEntry = (JournalEntry)dataGrid_JournalEntries.SelectedItems[0];
                        entryID = rowEntry.Id;

                        // Remove the journal entry according to the entry ID
                        currentJournal.Entries.Remove
                            (currentJournal.Entries.Single(i => i.Id == entryID));

                    }
                    else
                    {
                        MessageBox.Show("The entry to delete is empty");
                    }
                }
                else
                {
                    MessageBox.Show("Please select an entry to delete");
                }
            }
            catch (Exception except)
            {
                if (entryID > 0)
                {
                    MessageBox.Show("An error occurred when deleting entry ID "
                             + entryID + ": " + except.Message);
                }
                else
                {
                    MessageBox.Show("An error occurred when deleting entry: "
                             + except.Message);
                }
            }
        }

        // Entry selection changed in grid: display entry at bottom
        private void dataGrid_JournalEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_JournalEntries.SelectedItem != null &&
                     dataGrid_JournalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry rowEntry = new JournalEntry();
                rowEntry = (JournalEntry)dataGrid_JournalEntries.SelectedItems[0];
                textBox_title.Text = rowEntry.Title;
                textBox_entry.Text = rowEntry.Text;
                displayedEntryId = rowEntry.Id;
            }
       }
    }
}
