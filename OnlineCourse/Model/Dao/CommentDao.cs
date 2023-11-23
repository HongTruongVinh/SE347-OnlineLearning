using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;
using PagedList;
using Model.ViewModel;


namespace Model.Dao
{
    public class CommentDao
    {

        public CommentDao()
        {

        }
        public bool Insert(Comment entity)
        {
            DataProvider.Ins.DB.Comments.Add(entity);
            DataProvider.Ins.DB.SaveChanges();
            return true;
        }
        public List<Comment> ListComment(long parentId, long productId)
        {
            return DataProvider.Ins.DB.Comments.Where(x => x.ParentID == parentId && x.ProductID == productId).ToList();
        }
       
    }
}
