using System;
using System.Collections.Generic;
using Cats.DataEntiry;
using CatsDataEntity;
using CatsPrj.Model;
using CatsProj.DAL.Providers;
using EntityModelConverter;

namespace CatsProj.BLL.Handlers
{
    public class PostsHandler
    {
        public void savePosts(string postsMaker,string postsContent,int picsCount,string postsId)
		{
			PostsModel model = new PostsModel();
			model.postsMaker = postsMaker;
			model.postsContent = postsContent;
			model.postsID = postsId;
			model.postsMakeDate = DateTime.Now;
			model.postsPicCount = picsCount;
            
			tbl_posts posts =  PostsConverter.postsModelToEntity(model);
			PostsProvider provider = new PostsProvider();
			provider.savePosts(posts);
		}

        public List<PostsModel> getPosts(int from,int count)
		{
			PostsProvider provider = new PostsProvider();
			IList<PostsPics> pics = new List<PostsPics>();
			pics = provider.getPosts(from, count, 1);
			List<PostsModel> result = new List<PostsModel>();

            foreach(var item in pics)
			{
				result.Add(PostsConverter.postsEntityToModel(item));
			}
			return result;
		}
    }
}
