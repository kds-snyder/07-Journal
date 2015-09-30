using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfJournal.Classes
{
    public class JournalEntry
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
