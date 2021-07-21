using Blog.Core.Interface.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Blog.Core.Repository.Blog
{
    public class PostCategoryJunkRepository : Connection.DbConnection, IPostCategoryJunk
    {
        public ProcessResult Add(PostCategoryJunk entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "x başarıyla oluşturuldu.";
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

        public ProcessResult Add(List<PostCategoryJunk> InsertList)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(InsertList);
                    result.State = ProcessState.Success;
                    result.Message = "Başarılı.";
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
                    con.Delete<PostCategoryJunk>(new PostCategoryJunk() { ID = ID });
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

        public ProcessResult DeleteAllbyPost(int PostID)
        {
            ProcessResult pr = new ProcessResult();
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@PostID", PostID);

                string query = $@"
                DELETE
                FROM PostCategoryJunk 
                WHERE PostID = @PostID";

                using (var con = GetConnection)
                {
                    con.Query<PostCategoryJunk>(query, param);
                    pr.ReturnID = 0;
                    pr.Message = "Başarılı";
                    pr.State = ProcessState.Success;
                }
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

        public FilteredList<PostCategoryJunk> FilteredList(FilteredList<PostCategoryJunk> request)
        {
            try
            {
                FilteredList<PostCategoryJunk> result = new FilteredList<PostCategoryJunk>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from PostCategoryJunk t {WhereClause}";

                string query = $@"
                SELECT *
                FROM PostCategoryJunk t 
                {WhereClause} 
                ORDER BY t.ID DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<PostCategoryJunk>(query, param);
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

        public PostCategoryJunk Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<PostCategoryJunk>(ID);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<PostCategoryJunk> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<PostCategoryJunk>();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(PostCategoryJunk entity)
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
                        result.Message = "x başarıyla güncellendi.";
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
