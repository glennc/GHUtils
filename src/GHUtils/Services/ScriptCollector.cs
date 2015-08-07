using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHUtils.Services
{
    public class ScriptCollector
    {
        public List<TagHelperContent> Scripts { get; set; }
        public ScriptCollector()
        {
            this.Scripts = new List<TagHelperContent>();
        }
    }
}
