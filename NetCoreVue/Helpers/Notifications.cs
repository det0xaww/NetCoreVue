using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreVue.Helpers
{
    public class Notifications
    {
        public string Group { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public Notifications(string group, string type, string title, string text)
        {
            Group = group;
            Type = type;
            Title = title;
            Text = text;
        }
    }
}
