using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WpfJournal.Classes
{
    public class Journal
    {
        public Journal(string journalTitle)
        {
            Title = journalTitle;
            Entries = new ObservableCollection<JournalEntry>();
        }
        public string Title { get; private set; }

        public ObservableCollection<JournalEntry> Entries { get; set; }
    }
}
