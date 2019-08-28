using PhoneBookApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PhoneBookApp.Controllers
{
    public class SourceManager
    {
        public string Filter { get; set; }

        protected SqlConnection _connection;
        protected void Open()
        {
            _connection = new SqlConnection()
            {
                ConnectionString = "Data Source=NTINLTP2402897\\SQLEXPRESS;Initial Catalog=PhoneBook;User Id=test01;Password=test01;"
            };
            _connection.Open();
        }
        protected void Close()
        {
            _connection.Close();
        }

        public List<PersonModel> Get()
        {
            List<PersonModel> personList = new List<PersonModel>();
            Open();
            SqlCommand command = new SqlCommand()
            {
                CommandText = "SELECT *" +
                              "FROM [PhoneBook].[dbo].[People]",
                CommandType = CommandType.Text,
                Connection = _connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        personList.Add(new PersonModel(
                            Convert.ToInt32(reader.GetValue(0)),
                            Convert.ToString(reader.GetValue(1)),
                            Convert.ToString(reader.GetValue(2)),
                            Convert.ToString(reader.GetValue(3)),
                            Convert.ToString(reader.GetValue(4))
                            ));
                    }
                }
            }
            Close();
            return personList;

        }
        public List<PersonModel> GetPhoneBookList(string filter)
        {

            List<PersonModel> personList = new List<PersonModel>();
            Open();
            SqlCommand command = new SqlCommand()
            {
                CommandText = "GetPhoneBookList",
                CommandType = CommandType.StoredProcedure,
                Connection = _connection
            };
            SqlParameter ParameterFilter = new SqlParameter()
            {
                ParameterName = "@filter",
                Value = filter,
                DbType = DbType.String
            };
            command.Parameters.Add(ParameterFilter);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        personList.Add(new PersonModel(
                            Convert.ToInt32(reader.GetValue(0)),
                            Convert.ToString(reader.GetValue(1)),
                            Convert.ToString(reader.GetValue(2)),
                            Convert.ToString(reader.GetValue(3)),
                            Convert.ToString(reader.GetValue(4))
                            ));
                    }
                }
                Filter = filter;
            }
            Close();
            return personList;

        }
        public PersonModel GetById(long id)
        {
            PersonModel personModel = new PersonModel();
            Open();
            SqlCommand command = new SqlCommand()
            {
                CommandText = "SELECT * FROM [PhoneBook].[dbo].[People] WHERE ID = @Id",
                CommandType = CommandType.Text,
                Connection = _connection
            };
            SqlParameter ParameterID = new SqlParameter()
            {
                ParameterName = "@Id",
                Value = id,
                DbType = DbType.Int32
            };
            command.Parameters.Add(ParameterID);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        personModel.ID = Convert.ToInt32(reader.GetValue(0));
                        personModel.FirstName = Convert.ToString(reader.GetValue(1));
                        personModel.LastName = Convert.ToString(reader.GetValue(2));
                        personModel.Phone = Convert.ToString(reader.GetValue(3));
                        personModel.Email = Convert.ToString(reader.GetValue(4));
                    }
                }
            }
            return personModel;
        }
        public int RowsCounter(string filter)
        {
            int counter = 0;
            Open();
            SqlCommand command = new SqlCommand()
            {
                CommandText = "SELECT COUNT(*) " +
                              "FROM [PhoneBook].[dbo].[People] " +
                              "WHERE LastName Like @filter",
                CommandType = CommandType.Text,
                Connection = _connection
            };
            SqlParameter ParameterFilter = new SqlParameter()
            {
                ParameterName = "@filter",
                //  Value = "{filter}%",
                Value = filter,
                DbType = DbType.String
            };
            command.Parameters.Add(ParameterFilter);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        counter = Convert.ToInt16(reader.GetValue(0));
                    }
                }
            }
            Close();
            return counter;
        }
        public void Add(PersonModel personModel)
        {
            Open();
            SqlTransaction transaction = _connection.BeginTransaction();

            SqlCommand command = new SqlCommand()
            {
                CommandText = "INSERT INTO [PhoneBook].[dbo].[People] ( [FirstName], [LastName], [Phone], [Email])" +
                              "VALUES( @FirstName, @LastName, @Phone, @Email);" +
                              "SELECT SCOPE_IDENTITY(); ",
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            };
            SqlParameter ParameterFirstName = new SqlParameter()
            {
                ParameterName = "@FirstName",
                Value = personModel.FirstName,
                DbType = DbType.String
            };
            SqlParameter ParameterLastName = new SqlParameter()
            {
                ParameterName = "@LastName",
                Value = personModel.LastName.Replace(" ", ""),
                DbType = DbType.String
            };
            SqlParameter ParameterPhone = new SqlParameter()
            {
                ParameterName = "@Phone",
                Value = personModel.Phone.Replace(" ", ""),
                DbType = DbType.String
            };
            SqlParameter ParameterEmail = new SqlParameter()
            {
                ParameterName = "@Email",
                Value = personModel.Email.Replace(" ", ""),
                DbType = DbType.String
            };

            command.Parameters.Add(ParameterFirstName);
            command.Parameters.Add(ParameterLastName);
            command.Parameters.Add(ParameterPhone);
            command.Parameters.Add(ParameterEmail);

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                Close();
            }

        }
        public void Update(PersonModel personModel)
        {
            Open();
            SqlTransaction transaction = _connection.BeginTransaction();
            SqlCommand command = new SqlCommand()
            {
                CommandText = "UPDATE [dbo].[People] " +
                              "Set FirstName = @FirstName, " +
                              "LastName = @LastName, " +
                              "Phone = @Phone, " +
                              "Email = @Email " +
                              "WHERE ID = @Id; ",
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            };

            SqlParameter ParameterID = new SqlParameter()
            {
                ParameterName = "@Id",
                Value = personModel.ID,
                DbType = DbType.Int32
            };
            SqlParameter ParameterFirstName = new SqlParameter()
            {
                ParameterName = "@FirstName",
                Value = personModel.FirstName,
                DbType = DbType.String
            };
            SqlParameter ParameterLastName = new SqlParameter()
            {
                ParameterName = "@LastName",
                Value = personModel.LastName,
                DbType = DbType.String
            };
            SqlParameter ParameterPhone = new SqlParameter()
            {
                ParameterName = "@Phone",
                Value = personModel.Phone,
                DbType = DbType.String
            };
            SqlParameter ParameterEmail = new SqlParameter()
            {
                ParameterName = "@Email",
                Value = personModel.Email,
                DbType = DbType.String
            };


            command.Parameters.Add(ParameterID);
            command.Parameters.Add(ParameterFirstName);
            command.Parameters.Add(ParameterLastName);
            command.Parameters.Add(ParameterPhone);
            command.Parameters.Add(ParameterEmail);

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally
            {
                Close();
            }
        }
        public void Delete(long id)
        {
            Open();
            SqlTransaction transaction = _connection.BeginTransaction();
            SqlCommand command = new SqlCommand()
            {
                CommandText = "DELETE FROM [dbo].[People] " +
                              "WHERE ID = @Id; ",
                CommandType = CommandType.Text,
                Connection = _connection,
                Transaction = transaction
            };
            SqlParameter ParameterID = new SqlParameter()
            {
                ParameterName = "@Id",
                Value = id,
                DbType = DbType.Int32
            };
            command.Parameters.Add(ParameterID);
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally
            {
                Close();
            }
        }
    }
}