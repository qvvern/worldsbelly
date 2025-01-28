using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.Puppeteers.Puppeteers.Models
{
    public class Browsers
    {
        public List<Window> Windows { get; set; }

        public Browsers()
        {
            Windows = new List<Window>();
        }
    }
    public class Window
    {
        public BrowserContext Browser { get; set; }
        public List<Page> Pages { get; set; }

        public Window(BrowserContext browser)
        {
            Pages = new List<Page>();
            Browser = browser;
        }
    }
}
