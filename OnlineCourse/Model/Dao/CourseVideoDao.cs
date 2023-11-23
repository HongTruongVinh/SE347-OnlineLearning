using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CourseVideoDao
    {
        public CourseVideoDao() { }

        public CourseVideo GetVideo(int videoId)
        {
            return DataProvider.Ins.DB.CourseVideos.Where(x => x.ID == videoId).SingleOrDefault();
        }

        public List<CourseVideo> GetListVideoInfor(int productId)
        {
            List<CourseVideo> courseVideos = new List<CourseVideo>();

            courseVideos = DataProvider.Ins.DB.CourseVideos.Where(x => x.productID == productId).ToList();

            return courseVideos;
        }

        public bool AddCourseVideo(CourseVideo courseVideo)
        {
            try
            {
                DataProvider.Ins.DB.CourseVideos.Add(courseVideo);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool UpdateCourseVideo(CourseVideo entity)
        {
            try
            {
                var video = DataProvider.Ins.DB.CourseVideos.Find(entity.ID);
                video.Title = entity.Title;
                video.DateUpdate = DateTime.Now;
                video.Link = entity.Link;

                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCourseVideo(int courseVideoId)
        {
            try
            {
                DataProvider.Ins.DB.CourseVideos.Remove(DataProvider.Ins.DB.CourseVideos.Where(x => x.ID == courseVideoId).SingleOrDefault());
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
