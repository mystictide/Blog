using Blog.Core.Interface.User;
using Blog.Entity.Helpers;
using Blog.Entity.User;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core.Repository.User
{
    public class UserRepository : Connection.DbConnection, IUsers
    {
        public ProcessResult Add(Users entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Kullanıcı başarıyla oluşturuldu.";
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

        public Users CheckAuth(string Mail, string Password)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Mail", Mail);
                param.Add("@Password", Password);

                string WhereClause = @" WHERE (t.Mail like '%' + @Mail + '%') AND Password = @Password";

                string query = $@"
                SELECT *
                FROM Users t 
                {WhereClause}";

                using (var connection = GetConnection)
                {
                    return connection.QueryFirstOrDefault<Users>(query, param);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Delete(int ID)
        {
            using (var con = GetConnection)
            {
                ProcessResult pr = new ProcessResult();
                try
                {
                    con.Delete<Users>(new Users() { ID = ID });
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

        public FilteredList<Users> FilteredList(FilteredList<Users> request)
        {
            try
            {
                FilteredList<Users> result = new FilteredList<Users>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Users t {WhereClause}";

                string query = $@"
                SELECT *
                FROM Users t 
                {WhereClause} 
                ORDER BY t.ID DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Users>(query, param);
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

        public Users Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<Users>(ID);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<Users> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<Users>();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Users entity)
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
                        result.Message = "Hesap bilgileri başarıyla güncellendi.";
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

        public bool CheckMail(string Mail)
        {
            string Query = @"Select * from Users where Mail=@Mail";
            DynamicParameters p = new DynamicParameters();
            p.Add("@Mail", Mail);
            using (var con = GetConnection)
            {
                return !(con.Query<Users>(Query, p).Count() > 0);
            }
        }
    }
}
