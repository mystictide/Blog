using Blog.Core.Interface.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Blog.Core.Repository.Blog
{
    public class CategoryRepository : Connection.DbConnection, ICategories
    {
        public ProcessResult Add(Categories entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Kategori başarıyla oluşturuldu.";
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
                    con.Delete<Categories>(new Categories() { ID = ID });
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

        public FilteredList<Categories> FilteredList(FilteredList<Categories> request)
        {
            try
            {
                FilteredList<Categories> result = new FilteredList<Categories>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Categories t {WhereClause}";

                string query = $@"
                SELECT *
                FROM Categories t 
                {WhereClause} 
                ORDER BY t.ID DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Categories>(query, param);
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

        public Categories Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<Categories>(ID);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<Categories> GetAll()
        {
            try
            {
                string query = $@"
                Select *
                ,(select COUNT(ID) from PostCategoryJunk Where CategoryID = t.ID)PostCount
                FROM Categories t";

                using (var connection = GetConnection)
                {
                    return connection.Query<Categories>(query);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Categories entity)
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
                        result.Message = "Kategori başarıyla güncellendi.";
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
