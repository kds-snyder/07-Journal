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
        private int displayedEntryId = 0; // Displayed entry's ID
        private int displayedEntryIndex = 0; // Displayed entry's ndex in list of journal entries
        private string labelEntryIdPrefix = "Entry ID: ";
        private bool displayedEntryChanged = false;

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

        // Add button clicked: add new journal entry, using
        //  displayed entry data
        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            addEntry();

       }

        // Update button clicked: update entry data              
        private void button_update_Click(object sender, RoutedEventArgs e)
        {
            if (displayedEntryId > 0)
            {
                updateEntry();
            }

         }

        // Delete button clicked: delete entry
        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check that row is selected in grid,
                //  and that entry is not empty
                if (dataGrid_JournalEntries.SelectedCells.Count > 0)
                {
                    if (displayedEntryId > 0)
                    {
                        // Prompt for confirmation before deleting
                        MessageBoxResult deleteConfirm =
                            MessageBox.Show
                             ("Are you sure you want to delete entry ID " 
                               + displayedEntryId + "?",
                               "Confirm deletion", MessageBoxButton.YesNo);
                        if (deleteConfirm == MessageBoxResult.Yes)
                        {
                            // Remove the journal entry that corresponds to 
                            //  the displayed entry index
                            currentJournal.Entries.RemoveAt(displayedEntryIndex);

                            // Clear the displayed entry data
                            clearDisplayedData();
                        }                       
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
                MessageBox.Show("An error occurred when deleting entry: "
                             + except.Message);                
            }
        }

        // Entry selection changed in grid: display entry data
        // If the displayed data has changed, offer chance to save it
        private void dataGrid_JournalEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          // Check that non-empty row has been selected
          if (dataGrid_JournalEntries.SelectedItem != null &&
                     dataGrid_JournalEntries.SelectedItem is JournalEntry)
           {
                // Get selected row
                JournalEntry rowEntry = new JournalEntry();
                rowEntry = (JournalEntry)dataGrid_JournalEntries.SelectedItem;

                // Before displaying selected row, 
                //   save displayed data if it has changed
                if (displayedEntryChanged)
                {
                    SaveChanges();
                }

                // Display entry data of selected row
                textBox_title.Text = rowEntry.Title;
                textBox_entry.Text = rowEntry.Text;

                // Update displayed entry info
                updateDisplayedInfo(rowEntry);
            }
       }

        // Displayed entry title has changed: 
        //   set indication that displayed entry has changed
        private void textBox_title_TextChanged(object sender, TextChangedEventArgs e)
        {
            displayedEntryChanged = true;
        }

        // Displayed entry text has changed: 
        //   set indication that displayed entry has changed
        private void textBox_entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            displayedEntryChanged = true;
        }

        // Offer chance to save displayed entry data, 
        //  updating if it is an existing entry, otherwise adding it        
        private void SaveChanges()
        {
            if (displayedEntryId > 0)
            {
                MessageBoxResult saveChangesResult =
                  MessageBox.Show("Entry ID " + displayedEntryId +
                  " has changed; update it before displaying selected entry?",
                       "Update changed entry?", MessageBoxButton.YesNo);
                if (saveChangesResult == MessageBoxResult.Yes)
                {
                    updateEntry();
                }
            }
            else
            {
                MessageBoxResult saveChangesResult = MessageBox.Show
               ("Entry data has changed; save as new entry before displaying selected entry?",
                     "Save new entry?", MessageBoxButton.YesNo);
                if (saveChangesResult == MessageBoxResult.Yes)
                {
                    addEntry();
                }
            }
        }   

        // Add a new entry with the displayed data
        private void addEntry()
        {
            try
            {
                // Create a new entry with all the data
                JournalEntry entry = new JournalEntry();
                entry.Date = DateTime.Now;
                entry.Title = textBox_title.Text;
                ++maxEntryId;
                entry.Id = maxEntryId;
                entry.Text = textBox_entry.Text;

                // Add the new entry
                currentJournal.Entries.Add(entry);

                // Update displayed entry info
                updateDisplayedInfo(entry);
            }
            catch (Exception except)
            {
                MessageBox.Show("An error occurred when adding entry: "
                             + except.Message);
            }
        }

        // Update entry, using displayed entry data
        private void updateEntry()
        {
            try
            {
                // Update date/time, title, and text
                currentJournal.Entries[displayedEntryIndex].Date = DateTime.Now;
                currentJournal.Entries[displayedEntryIndex].Title =
                                                    textBox_title.Text;
                currentJournal.Entries[displayedEntryIndex].Text =
                                                    textBox_entry.Text;

                // Refresh the data grid
                dataGrid_JournalEntries.Items.Refresh();
            }
            catch (Exception except)
            {
                MessageBox.Show("An error occurred when updating entry: "
                             + except.Message);

            }
        }

        // Update displayed entry ID and index
        private void updateDisplayedInfo (JournalEntry row)
        {
            try
            {
                // Display the entry ID
                label_entryId.Content = labelEntryIdPrefix +
                    Environment.NewLine + row.Id;

                // Save ID and index of displayed entry
                displayedEntryId = row.Id;
                displayedEntryIndex =
                  currentJournal.Entries.IndexOf
                    (currentJournal.Entries.Single(i => i.Id == displayedEntryId));

                // Reset indication that displayed entry has changed
                displayedEntryChanged = false;
            }
            catch (Exception except)
            {
                MessageBox.Show
                    ("An error occurred when updating displayed entry information: "
                     + except.Message);
            }

        }

        // Clear displayed entry data
        private void clearDisplayedData()
        {
            // Clear displayed text and title
            textBox_title.Text = "";
            textBox_entry.Text = "";
            
            // Clear the displayed entry ID
            label_entryId.Content = "";

            // Reset ID and index of displayed entry
            displayedEntryId = 0;
            displayedEntryIndex =0;

            // Reset indication that displayed entry has changed
            displayedEntryChanged = false;
        }


    }
}
