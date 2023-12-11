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

        public List<CommentViewModel> ListCommentViewModel(long parentId, long productId)
        {
            var model = (from a in DataProvider.Ins.DB.Comments
                         join b in DataProvider.Ins.DB.Users
                             on a.UserID equals b.ID
                         where a.ParentID == parentId && a.ProductID == productId

                         select new
                         {
                             ID = a.ID,
                             CommentMsg = a.CommentMsg,
                             CommentDate = a.CommentDate,
                             ProductID = a.ProductID,
                             UserID = a.UserID,
                             FullName = b.Name,
                             ParentID = a.ParentID,
                             Rate = a.Rate,
                             LinkAvatar = b.LinkImage
                         }).AsEnumerable().Select(x => new CommentViewModel()
                         {
                             ID = x.ID,
                             CommentMsg = x.CommentMsg,
                             CommentDate = x.CommentDate,
                             ProductID = (long)x.ProductID,
                             UserID = (long)x.UserID,
                             FullName = x.FullName,
                             ParentID = (long)x.ParentID,
                             Rate = (int)x.Rate,
                             LinkAvatar = x.LinkAvatar
                         });
            return model.OrderByDescending(y => y.ID).ToList();
        }
    }
}
