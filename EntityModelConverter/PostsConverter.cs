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
			return entity;
		}
    }
}
