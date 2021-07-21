using Dapper.Contrib.Extensions;
using Blog.Core.Interface;
using System;
using System.Diagnostics;
using System.Linq;

namespace Blog.Core.Repository
{
    public class LogsRepository : Connection.DbConnection, ILogs
    {
        public int Add(Entity.Logs entity)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return (int)con.Insert(entity);
                }
            }
            catch (Exception exception)
            {
                string a = exception.Message;
                return 0;
            }
        }

        public static void CreateLog(Exception ex, int UserId = 0)
        {
            try
            {
                var st = new StackTrace(ex, true);
                if (st != null)
                {
                    st.GetFrames().Where(k => k.GetFileLineNumber() > 0).ToList().ForEach(k =>
                    {
                        new LogsRepository().Add(new Entity.Logs()
                        {
                            CreatedDate = DateTime.Now,
                            UserID = UserId,
                            Message = ex.Message,
                            Source = ex.Source + " | " + k,
                            Line = k.GetFileLineNumber()
                        });
                    });
                }
                else
                {
                    new LogsRepository().Add(new Entity.Logs()
                    {
                        CreatedDate = DateTime.Now,
                        UserID = UserId,
                        Message = ex.Message,
                        Source = ex.Source,
                        Line = 0
                    });
                }
            }
            catch (Exception exception)
            {
                new LogsRepository().Add(new Entity.Logs()
                {
                    CreatedDate = DateTime.Now,
                    UserID = 0,
                    Message = exception.Message,
                    Source = exception.Source + " - Error logging",
                    Line = 0
                });
            }


        }
    }
}