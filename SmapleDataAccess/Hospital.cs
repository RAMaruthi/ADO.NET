using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using SampleFrameworkApp;

namespace SampleDataAccessApp.Assignment
{
    class Patient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientAddress { get; set; }
        public string  PatientInsuren { get; set; }
        public int ProviderId { get; set; }
    }

    class Provider
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Specialization { get; set; }
    }

    interface IHospitalRepo
    {
        void AddNewPatient(Patient patient);
        void UpdatePatient(Patient patient,int id);
        void DeletePatient(int patientid);
        List<Patient> GetAllPatients();
        List<Provider> GetAllProviders();
    }

    class HospitalRepo : IHospitalRepo
    {
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3337;Integrated Security=True";
        const string STRPROVIDERID = "SELECT ProviderId,ProviderName FROM PROVIDER";
        const string STRADDNEWPATIENT = "INSERT INTO Patien(PatienName,PatienAddress,PatienInsu,ProviderId) VALUES (@name,@address,@insurence,@id)";
        const string STRCHECK= "SELECT * FROM patien WHERE PatienId = @id";
        const string STRUPDATE = "update patien set PatienName=@name,PatienAddress=@address,PatienInsu=@insurence,ProviderId=@ProvideId WHERE PatienId = @id";

        const string STRDELETE = "delete from patien WHERE PatienId = @id ";

        public void helperDisplayProviderId()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRPROVIDERID, con);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Provider ID: {reader[0]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public bool helperCheckId(int id)
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRCHECK, con);

            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return false;
        }





        public void displayProvider()
        {

            SqlConnection con = new SqlConnection();
        }
        public void AddNewPatient(Patient patient)
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRADDNEWPATIENT, con);

            cmd.Parameters.AddWithValue("@name", patient.PatientName);
            cmd.Parameters.AddWithValue("@address", patient.PatientAddress);
            cmd.Parameters.AddWithValue("@insurence", patient.PatientInsuren);
            cmd.Parameters.AddWithValue("@id", patient.ProviderId);


            try
            {
                con.Open();
                var rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine("Failed to add the patiet to the record");
                }
                Console.WriteLine("Patient added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void DeletePatient(int patientid)
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRDELETE, con);
            cmd.Parameters.AddWithValue("@id", patientid);
            try
            {
                con.Open();
                var rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine("Failed to add the patiet to the record");
                }
                Console.WriteLine("Patient Delete successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public List<Patient> GetAllPatients()
        {
            throw new NotImplementedException();
        }

        public List<Provider> GetAllProviders()
        {
            throw new NotImplementedException();
        }

        public void UpdatePatient(Patient patient,int id)
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRUPDATE, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", patient.PatientName);
            cmd.Parameters.AddWithValue("@address", patient.PatientAddress);
            cmd.Parameters.AddWithValue("@insurence", patient.PatientInsuren);
            cmd.Parameters.AddWithValue("@ProvideId", patient.ProviderId);
            try
            {
                con.Open();
                var rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    Console.WriteLine("Failed to add the patiet to the record");
                }
                Console.WriteLine("Patient Update successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }


    class UserMenu
    {
        public void start()
        {
            addingPatient();
            //Update();
           // Delete();

        }
        public void addingPatient()
        {
            HospitalRepo obj = new HospitalRepo();
            string name = Utilities.Prompt("Enter the Patient Name");
            string address = Utilities.Prompt("Enter the Patient Address");
            string Insurence = Utilities.Prompt("Enter the Insurance Public OR Private of the Patient");
            obj.helperDisplayProviderId();
            int id = Utilities.GetNumber("Enter the Provider ID from the above");
            Patient patient = new Patient { PatientName = name, PatientAddress = address, PatientInsuren=Insurence, ProviderId = id };
            obj.AddNewPatient(patient);
        }
        public void Update()
        {
            int id = Utilities.GetNumber("Enter The Id to Update Of Patient");
            HospitalRepo obj = new HospitalRepo();
            

            bool res= obj.helperCheckId(id);
            if (res == true)
            {
                string name = Utilities.Prompt("Enter the Patient Name To Update");
                string address = Utilities.Prompt("Enter the Patient Address To Update");
                string Insurence = Utilities.Prompt("Enter the Insurance Public OR Private of the Patient To Update");
                obj.helperDisplayProviderId();
                int newid = Utilities.GetNumber("Enter the Provider ID To Update");
                Patient patient = new Patient { PatientName = name, PatientAddress = address, PatientInsuren = Insurence, ProviderId = newid };
                obj.UpdatePatient(patient,id);
            }
            else
            {
                Console.WriteLine("The ID is Not Match");
            }


        }

        public void Delete()
        {
            HospitalRepo obj = new HospitalRepo();
            int id = Utilities.GetNumber("Enter The Id to Delete The Patient");
            bool res = obj.helperCheckId(id);
            if (res == true)
            {
                obj.DeletePatient(id);
            }
            else
            {
                Console.WriteLine("The Id Doesnot Matched");
            }

        }
    }
    class DomainApp
    {
        static void Main(string[] args)
        {
            UserMenu start = new UserMenu();
            start.start();
        }
    }
}