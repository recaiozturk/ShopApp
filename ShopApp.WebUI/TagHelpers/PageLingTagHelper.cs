using Microsoft.AspNetCore.Razor.TagHelpers;
using ShopApp.WebUI.Models;
using System.Text;

namespace ShopApp.WebUI.TagHelpers
{

    [HtmlTargetElement("div",Attributes ="page-model")]
    public class PageLingTagHelper:TagHelper
    {
        public PageInfo PageModel { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<ul class='pagination'>");

            for (int i = 1; i <= PageModel.TotalPages(); i++)
            {
                stringBuilder.AppendFormat("<li class='page-item {0}'>", i == PageModel.CurrentPage ? "active" : "");

                if (string.IsNullOrEmpty(PageModel.CurrentCategory))
                {
                    stringBuilder.AppendFormat("<a class='page-link' href='/products?page={0}'>{0}</a>", i);
                }
                else
                {
                    stringBuilder.AppendFormat("<a class='page-link' href='/products/{0}?page={1}'>{1}</a>", PageModel.CurrentCategory, i);
                }
                stringBuilder.Append("</li>");
            }

            output.Content.SetHtmlContent(stringBuilder.ToString());

            base.Process(context, output);

           
        }
    }
}
