using System;
using CatsProj.DB;
using CatsDataEntity;
using MySql.Data;
using SqlSugar;
using System.Collections.Generic;
using Cats.DataEntiry;

namespace CatsProj.DAL.Providers
{
    public class PostsProvider
    {
		public tbl_posts getPost(string postId)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			tbl_posts result = db.Queryable<tbl_posts>().Where(o => o.postsID == postId).First();
			return result;
		}

        public IList<tbl_posts> getPosts(string userId,int from,int count,int orderby=0)//0 means order by readed times,1 means order by createdate
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			List<tbl_posts> result = new List<tbl_posts>();
			if (orderby == 0)
			{
				result = db.Queryable<tbl_posts>().Where(o => o.postsMaker == userId)
										   .OrderBy(o=>o.postsReaded,OrderByType.Desc).Take(from + count).Skip(from)
										   .ToList();
			}
			else
			{
				result = db.Queryable<tbl_posts>().Where(o => o.postsMaker == userId)
                                           .OrderBy(o => o.postsMakeDate, OrderByType.Desc).Take(from + count).Skip(from)
                                           .ToList();
			}
			return result;
		}

		public IList<PostsPics> getPosts(int from,int count,int orderby)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			List<PostsPics> result = new List<PostsPics>();
			try
			{
				if (orderby == 0)
				{
					result = db.Queryable<tbl_posts, tbl_postsPics, tbl_user>((po, pp, ur) => new object[] {
										JoinType.Left,po.postsID==pp.postsID,
										JoinType.Left,po.postsMaker==ur.openid})
											.Select((po, pp, ur) => new PostsPics
											{
												postsID = po.postsID,
												postsContent = po.postsContent,
												postsPics = pp.picPath,
												postsMaker = ur.nickName,
												postsLoved = po.postsLoved,
												postsReaded = po.postsReaded,
												postsStatus = po.postsStatus,
												postsMakeDate = po.postsMakeDate,
												postsPicCount = po.postsPicCount,
												postsReported = po.postsReported,
												postsCollected = po.postsCollected
					}).Where(pp=>pp.picIndex==SqlFunc.Subqueryable<tbl_postsPics>().Where(o => o.picIndex == 0).Select(o => o.picIndex))
											   .OrderBy(po => po.postsReaded, OrderByType.Desc).Take(from + count).Skip(from)
											   .ToList();
				}
				else
				{
					result = db.Queryable<tbl_posts, tbl_postsPics, tbl_user>((po, pp, ur) => new object[] {
										JoinType.Left,po.postsID==pp.postsID,
										JoinType.Left,po.postsMaker==ur.openid})
											.Select((po, pp, ur) => new PostsPics
											{
												postsID = po.postsID,
												postsContent = po.postsContent,
												postsPics = pp.picPath,
												postsMaker = ur.nickName,
												postsLoved = po.postsLoved,
												postsReaded = po.postsReaded,
												postsStatus = po.postsStatus,
												postsMakeDate = po.postsMakeDate,
												postsPicCount = po.postsPicCount,
												postsReported = po.postsReported,
												postsCollected = po.postsCollected,
						picIndex=pp.picIndex
						                        
					}).Where(pp => pp.picIndex == SqlFunc.Subqueryable<tbl_postsPics>().Where(o => o.picIndex == 0).Select(o => o.picIndex))
											   .OrderBy(po => po.postsMakeDate, OrderByType.Desc).Take(from + count).Skip(from)
											   .ToList();
				}
				return result;
			}
            catch(Exception e)
			{
				return result;
			}
		}

        public void savePosts(tbl_posts posts)
		{
			SqlSugarClient db = SqlSugarInstance.newInstance();
			db.Insertable<tbl_posts>(posts).ExecuteCommand();
		}
        
        
    }
}
