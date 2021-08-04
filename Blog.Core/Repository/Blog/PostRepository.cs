using Blog.Core.Interface.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core.Repository.Blog
{
    public class PostRepository : Connection.DbConnection, IPosts
    {
        public ProcessResult Add(Posts entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Yazı başarıyla oluşturuldu.";
                    result.State = ProcessState.Success;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
                LogsRepository.CreateLog(ex);
            }
            return result;
        }

        public ProcessResult Delete(int ID)
        {
            using (var con = GetConnection)
            {
                ProcessResult pr = new ProcessResult();
                try
                {
                    con.Delete<Posts>(new Posts() { ID = ID });
                    pr.ReturnID = 0;
                    pr.Message = "Başarılı";
                    pr.State = ProcessState.Success;
                }
                catch (Exception ex)
                {
                    pr.ReturnID = 0;
                    pr.Message = "Başarısız";
                    pr.State = ProcessState.Error;
                    LogsRepository.CreateLog(ex);
                }
                return pr;
            }
        }

        public FilteredList<Posts> FilteredList(FilteredList<Posts> request)
        {
            try
            {
                FilteredList<Posts> result = new FilteredList<Posts>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.TitleTUR like '%' + @Keyword + '%')";

                if (request.filterModel.UserID > 0)
                {
                    param.Add("@UserID", request.filterModel.UserID);
                    WhereClause = @" WHERE UserID = @UserID AND (t.TitleTUR like '%' + @Keyword + '%')";
                }            

                string query_count = $@"  Select Count(t.ID) from Posts t {WhereClause}";

                string query = $@"
                SELECT *
                ,(select Name from Users where ID = t.UserID) Author
                ,(select Name from Users where ID = t.ContributorID) ContributingAuthor
                ,(select Bio from Users where ID = t.UserID) Bio
                FROM Posts t 
                {WhereClause} 
                ORDER BY t.Date DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Posts>(query, param);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Posts Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                ,(select Name from Users where ID = t.UserID) Author
                ,(select Name from Users where ID = t.ContributorID) ContributingAuthor
                FROM Posts t
                WHERE ID = @ID";

                string junkquery = $@"
                SELECT CategoryID
                FROM PostCategoryJunk
                WHERE PostID = @ID";

                using (var connection = GetConnection)
                {
                    var model = connection.QueryFirstOrDefault<Posts>(query, param);
                    model.Categories = connection.Query<int>(junkquery, param).ToList();
                    return model;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Posts GetPostforView(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                ,(select Name from Users where ID = t.UserID) Author
                ,(select Name from Users where ID = t.ContributorID) ContributingAuthor
                FROM Posts t
                WHERE ID = @ID";

                string junkquery = $@"
                SELECT CategoryID
                ,(select Name from Categories where ID = t.CategoryID) as Name
                ,(select ID from Categories where ID = t.CategoryID) as ID
                FROM PostCategoryJunk t
                WHERE PostID = @ID";

                using (var connection = GetConnection)
                {
                    var model = connection.QueryFirstOrDefault<Posts>(query, param);
                    model.Category = connection.Query<Categories>(junkquery, param).ToList();
                    return model;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<Posts> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<Posts>();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public FilteredList<PostCategoryJunk> PostsbyCategory(FilteredList<PostCategoryJunk> request, int CategoryID)
        {
            try
            {
                FilteredList<PostCategoryJunk> result = new FilteredList<PostCategoryJunk>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PageSize", request.filter.pageSize);
                param.Add("@CategoryID", CategoryID);
                string Where = @" WHERE CategoryID = @CategoryID";

                string query_count = $@"  Select Count(t.ID) from Posts t";

                string query = $@"
                SELECT *
                ,(select Name from Users where ID = c.UserID)Author
                ,(select Name from Users where ID = c.ContributorID)ContributingAuthor
                FROM PostCategoryJunk t
                LEFT JOIN Posts c ON c.ID = t.PostID 
                {Where}
                ORDER BY c.Date DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<PostCategoryJunk, Posts, PostCategoryJunk>(query,
                        (p, c) =>
                        {
                            p.Post = c;
                            return p;
                        },
                        splitOn: "ID",
                        param: param).ToList();
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Posts entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    bool res = con.Update(entity);
                    if (res == true)
                    {
                        result.ReturnID = entity.ID;
                        result.Message = "Yazı başarıyla güncellendi.";
                        result.State = ProcessState.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
                LogsRepository.CreateLog(ex);
            }
            return result;
        }
    }
}
