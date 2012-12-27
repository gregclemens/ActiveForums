//� 2004 - 2008 ActiveModules, Inc. All Rights Reserved
//ORIGINAL LINE: Imports System.Web.HttpContext

using System.Data;

using System.Web;
namespace DotNetNuke.Modules.ActiveForums
{
	public class emoticons
	{
		public string LoadEmoticons(EditorTypes Type, int ModuleId, string ImagePath)
		{
			return RegisterEmotIcons(ModuleId, ImagePath, Type);
		}

		public string RegisterEmotIcons(int ModuleId, string ImagePath, EditorTypes InsertType)
		{
			string strHost = Common.Globals.AddHTTP(Common.Globals.GetDomainName(HttpContext.Current.Request)) + "/";
			var sb = new System.Text.StringBuilder();
			IDataReader dr = DataProvider.Instance().Filters_GetEmoticons(ModuleId);
			sb.Append("<div id=\"emotions\" class=\"afemoticons\"><div id=\"emotions\" style=\"width:100%; height:100%;align:center;\">");
			int i = 0;
			while (dr.Read())
			{
				string sEmotPath = ImagePath + dr["Replace"];
				string sInsert;
				if (InsertType == EditorTypes.TEXTBOX)
				{
					sInsert = dr["Find"].ToString();
				}
				else
				{
					sInsert = "<img src=\\'" + sEmotPath + "\\' />";
				}
				//sb.Append("<div class=""afEmot"" style=""width:16px;height:16px;""><img class=""afEmot"" src=""" & sEmotPath & """ width=""20"" height=""20"" title=""" & dr("Find").ToString & """ unselectable=""on"" style=""cursor:hand;"" onclick=""insertEmoticon('" & sInsert & "')"" />")
				sb.Append("<span class=\"afEmot\" style=\"width:20px;height:20px;cursor:hand;\" unselectable=\"on\" onclick=\"amaf_insertHTML('" + sInsert + "')\"><img onmousedown=\"return false;\" src=\"" + sEmotPath + "\" width=\"20\" height=\"20\" title=\"" + dr["Find"] + "\" /></span>");
				i += 1;
				if (i % 2 == 0)
				{
					sb.Append("<br />");
				}
			}
			dr.Close();
			sb.Append("</div></div>");


			return sb.ToString();

		}
	}
}

