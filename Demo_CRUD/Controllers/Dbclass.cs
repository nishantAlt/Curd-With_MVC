﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace Demo_CRUD.Controllers
{
    public class Dbclass
    {
        SqlConnection cnn=null;
        SqlCommand command;
        string query;
        SqlDataReader dataReader;
        string connectionString = "Data Source=DESKTOP-D84M7A1\\SQLEXPRESS;Initial Catalog=Practise;Integrated Security=True";

        public void connect()
        {
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                Console.WriteLine("Connected");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
        }

        public void disconnect()
        {
            if (cnn != null)
            {
                cnn.Close();
            }
        }
        public int insert(Bean obj)
        {
            connect();
            query = $"insert into demo values('{obj.Id}','{obj.Name}') ";
            SqlCommand cmd = new SqlCommand(query, cnn);
            int i=cmd.ExecuteNonQuery();
            disconnect();
            return i;
        }

        public List<Bean> selectAll()
        {
            connect();
            List<Bean> arr = new List<Bean>();
            query = "select * from demo";
            command = new SqlCommand(query, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                arr.Add(new Bean(dataReader.GetValue(0).ToString().Trim(), dataReader.GetValue(1).ToString().Trim()));
            }
            disconnect();
            return arr;
        }

        public Bean selectById(String id)
        {
            connect();
            query = $"select * from demo where id='{id}'";
            command = new SqlCommand(query, cnn);
            dataReader = command.ExecuteReader();
            Bean result = null;
            while (dataReader.Read())
            {
                result = new Bean(dataReader.GetValue(0).ToString().Trim(), dataReader.GetValue(1).ToString().Trim());
            }
            disconnect();
            return result;
        }
        public int insertCustom(Bean data)
        {
            connect();
            string param = null;
            string value = null;
                var last = data.GetType().GetProperties().Last();
                foreach (var prop in data.GetType().GetProperties())
                {
                    if(prop.Equals(last))
                    {
                        param = param + prop.Name;
                        value = value + "'" + prop.GetValue(data, null) + "'";
                    }
                    else
                    {
                        param = param + prop.Name + ",";
                        value = value + "'" + prop.GetValue(data, null) + "',";
                    }
                }
            query = $"insert into demo({param}) values({value})";
            SqlCommand cmd = new SqlCommand(query, cnn);
            int i=0;
            try
            {
                 i= cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: "+e);
            }
            disconnect();
            return i;

        }

        public int insertCustomAll(List<Bean> arr)
        {
            connect();
            string param = null;
            string value = null;
            int i = 0;
            foreach (Bean data in arr)
            {
                var last = data.GetType().GetProperties().Last();
                foreach (var prop in data.GetType().GetProperties())
                {
                    if (prop.Equals(last))
                    {
                        param = param + prop.Name;
                        value = value + "'" + prop.GetValue(data, null) + "'";
                    }
                    else
                    {
                        param = param + prop.Name + ",";
                        value = value + "'" + prop.GetValue(data, null) + "',";
                    }

                }
                query = $"insert into demo({param}) values({value})";
                SqlCommand cmd = new SqlCommand(query, cnn);

                try
                {
                    i = i + cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e);
                }
                param = "";
                value = "";
            }

            disconnect();
            return i;
        }
        

        

    }
}