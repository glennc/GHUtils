using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using GHUtils.Services;

namespace GHUtils.TagHelpers
{
    // You may need to install the Microsoft.AspNet.Razor.Runtime package into your project
    [TargetElement("script", Attributes = "managed")]

    public class ScriptTag : TagHelper
    {
        private ScriptCollector _manager;

        public ScriptTag(ScriptCollector manager)
        {
            _manager = manager;
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.AllAttributes["managed"].Value.ToString() == "true")
            {
                var content = await context.GetChildContentAsync();
                _manager.Scripts.Add(content);
                output.SuppressOutput();
            }
        }
    }

    [TargetElement("render-managed-scripts")]
    public class ScriptRenderTag : TagHelper
    {
        private ScriptCollector _manager;

        public ScriptRenderTag(ScriptCollector manager)
        {
            _manager = manager;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            foreach (var scriptContent in _manager.Scripts)
            {
                output.Content.Append($"<script>{scriptContent.GetContent()}</script>");
            }
        }
    }
}
