using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace web.Service
{
    public static class DBHelper
    {
        public static void InsertStudentClass(int studentId, int classId, int confirmed)
        {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["BHCSConnection"].ConnectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = $"insert into  bhcs.class_student (classid, studentId, confirmed)  values ({classId}, {studentId},  {confirmed})";
                command.ExecuteNonQuery();
            }
        }
    }
}