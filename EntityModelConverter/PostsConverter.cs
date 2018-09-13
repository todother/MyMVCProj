using System;
using Cats.DataEntiry;
using CatsDataEntity;
using CatsPrj.Model;

namespace EntityModelConverter
{
    public class PostsConverter
    {
        public static PostsModel postsEntityToModel(PostsPics entity)
		{
			PostsModel posts = new PostsModel();
			posts.postsID = entity.postsID;
			posts.postsContent = entity.postsContent;
			posts.postsMaker = entity.postsMaker;
			posts.postsLoved = entity.postsLoved;
			posts.postsMakeDate = entity.postsMakeDate;
			posts.postsPicCount = entity.postsPicCount;
			posts.postsReaded = entity.postsReaded;
			posts.postsPics = entity.postsPics;
			posts.makerName = entity.makerName;
			posts.picsSimpPath = entity.picSimpPath;
			posts.whenPosts = dateDiff(entity.postsMakeDate.GetValueOrDefault());
			posts.makerPhoto = entity.makerPhoto;
			posts.makerID = entity.openId;
            posts.postsLocation = entity.postsLocation;
            posts.latitude = entity.latitude;
            posts.longitude = entity.longitude;
            posts.postsType = entity.postsType;
            posts.postsStatus = entity.postsStatus.Value;
            posts.ifOfficial = entity.ifOfficial;
            posts.picsRate = entity.picsRate;
            posts.ifUserLoved = entity.ifUserLoved>0;
			return posts;
		}

        public static tbl_posts postsModelToEntity(PostsModel model)
		{
			tbl_posts entity = new tbl_posts();
			entity.postsCollected = 0;
			entity.postsContent = model.postsContent;
			entity.postsID = model.postsID;
			entity.postsLoved = 0;
			entity.postsMakeDate = DateTime.Now;
			entity.postsMaker = model.postsMaker;
			entity.postsPicCount = model.postsPicCount;
			entity.postsReaded = 0;
			entity.postsStatus = 0;//0 means OK, 1 means unauthorized
			entity.postsReported = 0;
            entity.latitude = model.latitude;
            entity.longitude = model.longitude;
            entity.postsLocation = model.postsLocation;
            entity.postsType = model.postsType;
            entity.ifOfficial = model.ifOfficial;
			return entity;
		}

        private static string dateDiff(DateTime postsDate)
		{
			DateTime dtNow = DateTime.Now;
			TimeSpan timeSpanNow = new TimeSpan(dtNow.Ticks);
			TimeSpan timeSpanPosts = new TimeSpan(postsDate.Ticks);
			TimeSpan timeSpan = timeSpanNow.Subtract(timeSpanPosts).Duration();
            if(timeSpan.Days>0)
			{
				return timeSpan.Days + "天前";
			}
			else
			{
				if(timeSpan.Hours>0)
				{
					return timeSpan.Hours + "小时前";
				}
				else
				{
					if(timeSpan.Minutes>0)
					{
						return timeSpan.Minutes + "分钟前";
					}
					else
					{
						return "刚刚";
					}
				}
			}
		}
    }
}
