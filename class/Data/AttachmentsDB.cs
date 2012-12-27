﻿//© 2004 - 2009 ActiveModules, Inc. All Rights Reserved
using System;
using System.Collections.Generic;
using System.Data;

using Microsoft.ApplicationBlocks.Data;
namespace DotNetNuke.Modules.ActiveForums.Data
{
	public class AttachController : Connection
	{
		public int Attach_Save(AttachInfo ai)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, dbPrefix + "Attachments_Save", ai.AttachID, ai.ContentId, ai.UserID, ai.Filename, ai.FileData, ai.ContentType, ai.FileSize, ai.AllowDownload, ai.DisplayInline, ai.ParentAttachId));
		}

		public List<AttachInfo> Attach_List(int TopicId, int ReplyId, int ContentId, int UserId)
		{
			var al = new List<AttachInfo>();
			using (IDataReader dr = SqlHelper.ExecuteReader(connectionString, dbPrefix + "Attachments_List", TopicId, ReplyId, ContentId, UserId))
			{
				while (dr.Read())
				{
					al.Add(FillInfo(dr));
				}
				dr.Close();
			}
			return al;
		}
		public List<AttachInfo> Attach_ListMyFiles(int UserId, int RowIndex, int MaxRows, string SortColumn, string Sort)
		{
			var al = new List<AttachInfo>();
			using (IDataReader dr = SqlHelper.ExecuteReader(connectionString, dbPrefix + "Attachments_ListMyFiles", UserId, RowIndex, MaxRows, SortColumn, Sort))
			{
				while (dr.Read())
				{
					al.Add(FillInfo(dr));
				}
				dr.Close();
			}
			return al;
		}
		public List<AttachInfo> Attach_ListAttachFiles(int UserId, string AttachIds)
		{
			var al = new List<AttachInfo>();
			using (IDataReader dr = SqlHelper.ExecuteReader(connectionString, dbPrefix + "Attachments_ListAttachFiles", UserId, AttachIds))
			{
				while (dr.Read())
				{
					al.Add(FillInfo(dr));
				}
				dr.Close();
			}
			return al;
		}
		public AttachInfo Attach_Get(int AttachID, int ContentId, int UserId, bool WithSecurity)
		{
			AttachInfo ai = null;
			using (IDataReader dr = SqlHelper.ExecuteReader(connectionString, dbPrefix + "Attachments_Get", AttachID, ContentId, UserId, WithSecurity))
			{
				while (dr.Read())
				{
					ai = FillInfo(dr);
				}
				dr.Close();
			}
			return ai;
		}
		public string GetAttachIds(int AuthorId, int ContentId)
		{
			string attachids = string.Empty;
			using (IDataReader dr = SqlHelper.ExecuteReader(connectionString, dbPrefix + "Attachments_ListForPost", AuthorId, ContentId))
			{
				while (dr.Read())
				{
					attachids += dr["AttachId"] + ";";
				}
				dr.Close();
			}
			return attachids;
		}
		public void SaveToContent(int ContentId, int AttachId, string FileURL, string FileName, bool DisplayLink, string ContentType)
		{
			SqlHelper.ExecuteNonQuery(connectionString, dbPrefix + "Attachments_SaveToContent", ContentId, AttachId, FileURL, FileName, DisplayLink, ContentType);
		}
		public void Attach_Delete(int AttachId, int ContentId)
		{
			SqlHelper.ExecuteNonQuery(connectionString, dbPrefix + "Attachments_Content_Delete", AttachId, ContentId);
		}
		public void Attach_Delete(int AttachId, int ContentId, int UserId)
		{
			SqlHelper.ExecuteNonQuery(connectionString, dbPrefix + "Attachments_Delete", AttachId, ContentId, UserId);
		}
		private AttachInfo FillInfo(IDataRecord dr)
		{
			var ai = new AttachInfo {AttachID = Convert.ToInt32(dr["AttachId"])};
		    try
			{
				if (dr["ContentId"] == null)
				{
					ai.ContentId = -1;
				}
				else
				{
					ai.ContentId = Convert.ToInt32(dr["ContentId"]);
				}
			}
			catch (Exception ex)
			{
				ai.ContentId = -1;
			}

			try
			{
				ai.PostID = Convert.ToInt32(dr["PostId"]);
			}
			catch (Exception ex)
			{
				ai.PostID = -1;
			}

			ai.UserID = Convert.ToInt32(dr["UserId"]);
			ai.Filename = Convert.ToString(dr["FileName"]);
			ai.AllowDownload = Convert.ToBoolean(dr["AllowDownload"]);
			ai.DisplayInline = Convert.ToBoolean(dr["DisplayInline"]);
			ai.ParentAttachId = Convert.ToInt32(dr["ParentAttachId"]);
			try
			{
				ai.FileUrl = dr["FileURL"].ToString();
			}
			catch (Exception ex)
			{
				ai.FileUrl = null;
			}
			try
			{
				ai.CanRead = dr["CanRead"].ToString();
			}
			catch (Exception ex)
			{
				ai.CanRead = string.Empty;
			}
			if (dr["FileData"] != DBNull.Value)
			{
				ai.FileData = (Array)(dr["FileData"]);
			}
			else
			{
				ai.FileData = null;
			}

			ai.ContentType = Convert.ToString(dr["ContentType"]);
			ai.FileSize = Convert.ToInt32(dr["FileSize"]);
			return ai;
		}

	}

}

