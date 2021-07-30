using Blog.Core.Interface.User;
using Blog.Entity.Helpers;
using Blog.Entity.User;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Blog.Core.Repository.User
{
    public class ContactRepository : Connection.DbConnection, IContact
    {
        public ProcessResult Add(Contact entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Mesaj başarıyla oluşturuldu.";
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
                    con.Delete<Contact>(new Contact() { ID = ID });
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

        public FilteredList<Contact> FilteredList(FilteredList<Contact> request)
        {
            try
            {
                FilteredList<Contact> result = new FilteredList<Contact>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Contact t {WhereClause}";

                string query = $@"
                SELECT *
                FROM Contact t 
                {WhereClause} 
                ORDER BY t.ID DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Contact>(query, param);
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

        public Contact Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<Contact>(ID);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<Contact> GetAll()
        {
            try
            {
                string query = $@"
                Select *
                FROM Contact t";

                using (var connection = GetConnection)
                {
                    return connection.Query<Contact>(query);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Contact entity)
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
                        result.Message = "Mesaj başarıyla güncellendi.";
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
