using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleFrameworkApp;


namespace SmapleDataAccess
{
    static class Database
    {
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3337;Integrated Security=True";
        const string STRQUERY = "SELECT * FROM Emp";

        public static DataTable GetAllRecords()
        {
            SqlConnection sql = new SqlConnection();
            sql.ConnectionString = STRCONNECTION;

            SqlCommand cmd = sql.CreateCommand();
            cmd.CommandText = STRQUERY;
            try
            {
                sql.Open();
                var reader = cmd.ExecuteReader();
                DataTable table = new DataTable("EmpRecords");
                table.Load(reader);
                return table;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sql.State == ConnectionState.Open)
                {
                    sql.Close();
                }
            }
        }
    }
    class Connted
    {
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3337;Integrated Security=True";
        const string STRQUERY = "SELECT * FROM Emp";
        const string STRFIND = "select * from emp where empName=@name";
        const string STRINSERT = "insert into Emp values(@name,@Address,@salary,@deptId)";
        const string STRDEPT = "select * from tbldep";
        const string STRINSERTPROC = "insterEmp";
        static void Main(string[] args)
        {
            string name = Utilities.Prompt("ENter Name");
            string Address = Utilities.Prompt("Enter Address");
            int Salary = Utilities.GetNumber("Enter Salary");
            
           // DisplayofDept();
            int DeptId = Utilities.GetNumber("Enter DeptID");

            // AddNewReco(name, DeptId, Address, Salary);
            
            //Read();
            //GetRecords("Maruthi");
            // GetParameterRec("Maruthi");

            AddNewRecoUsingStoredProc(name, Address, Salary, DeptId);
            Display();

        }

        private static void AddNewRecoUsingStoredProc(string name, string Address, int Salary, int DeptId)
        {
            int empId = 0;
            SqlConnection sql = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRINSERTPROC, sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empName", name);
            cmd.Parameters.AddWithValue("@empAdd", Address);
            cmd.Parameters.AddWithValue("@empSalary", Salary);
            cmd.Parameters.AddWithValue("@depId", DeptId);
            cmd.Parameters.AddWithValue("@empId", empId);
            cmd.Parameters[4].Direction = ParameterDirection.Output;
            try
            {
                sql.Open();
                cmd.ExecuteNonQuery();
                empId = Convert.ToInt32(cmd.Parameters[4].Value);
                Console.WriteLine("The EmpId Of Newly added Employee is"+empId);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                sql.Close();
            }

        }

        private static void DisplayofDept()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRDEPT, con);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[1]+"  "+reader[0]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private static void AddNewReco(string name, int DeptId, string Address, int Salary)
        {
            SqlConnection sqlCon = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRINSERT, sqlCon);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@salary", Salary);
            cmd.Parameters.AddWithValue("@DeptId", DeptId);

            try
            {
                sqlCon.Open();
                var rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected!=1)
                {
                    throw new Exception("Failed to Add");
                }
                Console.WriteLine("Added SuccessFull");
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }

        }

        private static void GetParameterRec(string name)
        {

            SqlCommand cmd = new SqlCommand(STRFIND, new SqlConnection(STRCONNECTION));
            try
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Employee Name: {reader["EmpName"]}\nEmployee Address: {reader[2]}\n Employee salary: {reader[3]}\n\n");
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        private static void GetRecords(string v)
        {
            string StrQUERY = $"SELECT * from emp where empName like '%{v}%'";
            SqlConnection sql = new SqlConnection();
            sql.ConnectionString = STRCONNECTION;

            SqlCommand cmd = sql.CreateCommand();
            cmd.CommandText = StrQUERY;
            try
            {
                sql.Open();
                var reader = cmd.ExecuteReader();
                DataTable table = new DataTable("EmpRecords");
                table.Load(reader);

                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"Employee Name: {row["EmpName"]}\nEmployee Address: {row[2]}\n Employee salary: {row[3]}\n\n");
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sql.State == ConnectionState.Open)
                {
                    sql.Close();
                }
            }
        }

        private static void Display()
        {
            try
            {
                var table = Database.GetAllRecords();
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"Employee Name: {row["EmpName"]}\nEmployee Address: {row[2]}\n Employee salary: {row[3]}\n\n");
                }
                Console.WriteLine();
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        private static void Read()
        {
            SqlConnection sql = new SqlConnection();
            sql.ConnectionString = STRCONNECTION;

            SqlCommand command = sql.CreateCommand();
            command.CommandText = STRQUERY;
            try
            {
                sql.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine($"{dataReader["empName"]} from {dataReader[2]}");
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sql.State == System.Data.ConnectionState.Open)
                {
                    sql.Close();
                }
            }


        }
    }
}

