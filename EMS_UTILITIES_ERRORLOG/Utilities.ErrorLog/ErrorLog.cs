using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
using Utilities.API;

public class ErrorLog
{
    private static string _serviceName;
    private static string _connectionString;
    //private static bool _returnExceptionError;
    //private static int _userID;

    public ErrorLog()
    { }

    /// <summary>
    /// Library for Creating an Text file to store Error Logs.
    /// </summary>
    /// <param name="serviceName">Prefix of the file that will be generated, ex. '<prefix> + DateTime.Now.ToString("yyyyMMdd") + ".txt"'.  </param>
    /// <param name="defaultDirectory">Default directory.</param>
    /// <param name="inetpubDirectory">Directory to create when the default directory cannot be created.</param>
    /// <param name="connectionString">Connection string of the database where the logs will be inserted.</param>
    public ErrorLog(string serviceName, string connectionString)
    {
        _serviceName = serviceName;
        _connectionString = connectionString;
    }

    /// <summary>
    /// Creates error log in a text file.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="method"></param>
    /// <param name="exception"></param>
    /// <param name="innerException"></param>
    public static void CreateErrorLog(string errorLogPath, string controller, string method, string exception, string innerException)
    {
        try
        {
            string filename = "EMS_" + _serviceName + "_ErrorLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            StringBuilder Output = new StringBuilder();
            Output.Append("Class: " + controller + " | Method:   " + method);
            Output.Append(Environment.NewLine);
            Output.Append("Time:     " + DateTime.Now.ToLongTimeString());
            Output.Append(Environment.NewLine);
            Output.Append("Message:  ");
            Output.Append(exception);
            if (!string.IsNullOrEmpty(innerException))
            {
                Output.Append(Environment.NewLine);
                Output.Append("Inner Exception:  ");
                Output.Append(innerException);
            }
            Output.Append(Environment.NewLine);
            Output.Append(Environment.NewLine);

            if (!Directory.Exists(errorLogPath))
            {
                System.IO.Directory.CreateDirectory(errorLogPath);
            }

            if (System.IO.File.Exists(errorLogPath + filename))
            {
                using TextWriter tr = System.IO.File.AppendText(errorLogPath + filename);
                tr.WriteLine(Output);
            }
            else
            {
                using TextWriter tw = System.IO.File.CreateText(errorLogPath + filename);
                tw.WriteLine(Output);
            }
        }
        catch {  }
    }

    /// <summary>
    /// Inserts the error log in the database using mysql.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="method"></param>
    /// <param name="exception"></param>
    /// <param name="innerException"></param>
    /// <returns></returns>
    public static string MySQLInsertErrorLog(string errorLogPath, string controller, string method, string exception,
        string innerException, int userID, bool returnExceptionError)
    {
        long Id = 0;
        string result = "";
        try
        {
            string query = @"INSERT INTO error_log (layer, class, method, error_message, inner_exception, user_id)
                            VALUES (@layer, @class, @method, @error_message, @inner_exception, @user_id);
                            SELECT LAST_INSERT_ID();";

            using MySqlConnection conn = new MySqlConnection(_connectionString);
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@layer", MySqlDbType.VarChar, 20).Value = "API";
            cmd.Parameters.Add("@class", MySqlDbType.VarChar, 100).Value = controller;
            cmd.Parameters.Add("@method", MySqlDbType.VarChar, 255).Value = method;
            cmd.Parameters.Add("@error_message", MySqlDbType.VarChar, 5000).Value = exception;
            cmd.Parameters.Add("@inner_exception", MySqlDbType.VarChar, 5000).Value = innerException;
            cmd.Parameters.Add("@user_id", MySqlDbType.Int32).Value = userID;

            conn.Open();

            Id = Convert.ToInt16(cmd.ExecuteScalar().ToString());
        }
        catch {  }

        try
        {
            string refNo;
            if (Id == 0)
            {
                refNo = Guid.NewGuid().ToString().Substring(0, 5);
            }
            else
            {
                refNo = "" + Id;
            }

            if (returnExceptionError)
            {
                StringBuilder Output = new StringBuilder();
                Output.Append("Class: " + controller + " | Method:   " + method);
                Output.Append(Environment.NewLine);
                Output.Append("Time:     " + DateTime.Now.ToLongTimeString());
                Output.Append(Environment.NewLine);
                Output.Append("Message:  ");
                Output.Append(exception);
                if (!string.IsNullOrEmpty(innerException))
                {
                    Output.Append(Environment.NewLine);
                    Output.Append("Inner Exception:  ");
                    Output.Append(innerException);
                }
                Output.Append(Environment.NewLine);
                Output.Append(Environment.NewLine);

                result = Output.ToString();
            }
            else
            {
                result = MessageUtilities.ERRMSG_EXCEPTION_WITH_REFNO_PREFIX + refNo + " (Service: " + _serviceName + ")";
            }

            CreateErrorLog(errorLogPath, controller, method + " | Reference # " + refNo, exception, innerException);
        }
        catch {  }

        return result;
    }
}