﻿using Blog.Core.Interface.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                string WhereClause = @" WHERE (t.Title like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Posts t {WhereClause}";

                string query = $@"
                SELECT *
                FROM Posts t 
                {WhereClause} 
                ORDER BY t.ID DESC 
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
                using (var con = GetConnection)
                {
                    return con.Get<Posts>(ID);
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