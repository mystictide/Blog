using Blog.Core.Interface.Blog;
using Blog.Entity.Blog;
using Blog.Entity.Helpers;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Blog.Core.Repository.Blog
{
    public class SettingsRepository : Connection.DbConnection, ISettings
    {
        public ProcessResult Add(Settings entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Ayarlar başarıyla oluşturuldu.";
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
                ProcessResult pr = new ProcessResult();
                return pr;
        }

        public FilteredList<Settings> FilteredList(FilteredList<Settings> request)
        {
            try
            {
                FilteredList<Settings> result = new FilteredList<Settings>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Title like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Settings t {WhereClause}";

                string query = $@"
                SELECT *
                FROM Settings t 
                {WhereClause} 
                ORDER BY t.ID DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Settings>(query, param);
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

        public Settings Get(int ID)
        {
            var model = new Settings();
            try
            {
                string query = $@"
                SELECT *
                FROM Settings";
                using (var con = GetConnection)
                {
                    return con.QueryFirstOrDefault<Settings>(query);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return model;
            }        
        }

        public IEnumerable<Settings> GetAll()
        {
            try
            {
                string query = $@"
                Select *
                FROM Settings t";

                using (var connection = GetConnection)
                {
                    return connection.Query<Settings>(query);
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Settings entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@TitleTUR", entity.TitleTUR);
                param.Add("@TitleENG", entity.TitleENG);
                param.Add("@DescriptionTUR", entity.DescriptionTUR);
                param.Add("@DescriptionENG", entity.DescriptionENG);
                param.Add("@Instagram", entity.Instagram);
                param.Add("@Twitter", entity.Twitter);
                param.Add("@Facebook", entity.Facebook);
                param.Add("@YouTube", entity.YouTube);
                param.Add("@LinkedIn", entity.LinkedIn);

                string query = $@"
                update Settings set TitleTUR = @TitleTUR
                update Settings set TitleENG = @TitleENG
                update Settings set DescriptionTUR = @DescriptionTUR
                update Settings set DescriptionENG = @DescriptionENG
                update Settings set Instagram = @Instagram
                update Settings set Twitter = @Twitter
                update Settings set Facebook = @Facebook
                update Settings set YouTube = @YouTube
                update Settings set LinkedIn = @LinkedIn";
                using (var con = GetConnection)
                {
                    int res = con.Execute(query, param);
                    if (res > 0)
                    {
                        result.Message = "Ayarlar başarıyla güncellendi.";
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
