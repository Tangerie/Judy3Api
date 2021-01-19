using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SQLite;
using Judy.Models;
using Newtonsoft.Json;

namespace Judy.Modules
{
    public class Storage
    {

        public static Storage Instance { get; private set; }

        public static void CreateInstance(string db)
        {
            Instance = new Storage(db);
        }

        SQLiteConnection dbCon;
        public string DatabaseLocation { get; private set; }

        public Storage(string db)
        {
            dbCon = new SQLiteConnection($"Data Source={db}");
            DatabaseLocation = db;
        }

        public string GetVersion()
        {
            SQLiteCommand cmd = new SQLiteCommand("SELECT SQLITE_VERSION()", dbCon);
            dbCon.Open();
            string ver = cmd.ExecuteScalar().ToString();
            cmd.Dispose();
            dbCon.Close();
            return ver;
        }

        
        public Property CreateProperty(Property p)
        {
            dbCon.Open();

            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO properties (Address, ResponseMessage, InquiryIds) VALUES (@addr, @res, @inq)", dbCon);
            cmd.Parameters.AddWithValue("@addr", p.Address);
            cmd.Parameters.AddWithValue("@res", p.ResponseMessage);
            cmd.Parameters.AddWithValue("@inq", JsonConvert.SerializeObject(p.InquiryIds));

            try
            {
                cmd.ExecuteNonQuery();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            cmd.Dispose();

            p.Id = (int)dbCon.LastInsertRowId;

            dbCon.Close();
            return p;
        }

        //Returns a property from the data
        //Creates an Id automatically
        public Property CreateProperty(string addr, string responseMessage)
        {
            Property p = new Property
            {
                Address = addr,
                ResponseMessage = responseMessage,
                InquiryIds = new List<int>()
            };

            return CreateProperty(p);
        }

        public Property GetProperty(int id)
        {
            dbCon.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM properties WHERE id = @id", dbCon);
            cmd.Parameters.AddWithValue("@id", id);

            Property p = null;
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                p = new Property();
                p.Id = reader.GetInt32(0);
                p.Address = reader.GetString(1);
                p.IsActive = reader.GetBoolean(2);
                p.ResponseMessage = reader.GetString(3);
                p.InquiryIds = JsonConvert.DeserializeObject<List<int>>(reader.GetString(4));
                break;
            }

            cmd.Dispose();
            dbCon.Close();

            return p;
        }

        public List<Property> GetProperties()
        {
            List<Property> properties = new List<Property>();
            dbCon.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM properties", dbCon);
            
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Property p = new Property();
                p.Id = reader.GetInt32(0);
                p.Address = reader.GetString(1);
                p.IsActive = reader.GetBoolean(2);
                p.ResponseMessage = reader.GetString(3);
                p.InquiryIds = JsonConvert.DeserializeObject<List<int>>(reader.GetString(4));
                properties.Add(p);
            }

            cmd.Dispose();
            dbCon.Close();
            return properties;
        }



        public Person CreatePerson(Person p)
        {
            dbCon.Open();

            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO people (Name, Phone, Email, InquiryIds) VALUES (@name, @phone, @email, @inq)", dbCon);
            cmd.Parameters.AddWithValue("@name", p.Name);
            cmd.Parameters.AddWithValue("@phone", p.Phone);
            cmd.Parameters.AddWithValue("@email", p.Email);
            cmd.Parameters.AddWithValue("@inq", JsonConvert.SerializeObject(p.InquiryIds));

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            cmd.Dispose();

            p.Id = (int)dbCon.LastInsertRowId;

            dbCon.Close();
            return p;
        }

        public Person CreatePerson(string name, string phone, string email)
        {
            Person p = new Person
            {
                Name = name,
                Phone = phone,
                Email = email,
                InquiryIds = new List<int>()
            };

            return CreatePerson(p);
        }

        public Person GetPerson(int id)
        {
            dbCon.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM people WHERE id = @id", dbCon);
            cmd.Parameters.AddWithValue("@id", id);

            Person p = null;
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                p = new Person();
                p.Id = reader.GetInt32(0);
                p.Name = reader.GetString(1);
                p.Phone = reader.GetString(2);
                p.Email = reader.GetString(3);
                p.InquiryIds = JsonConvert.DeserializeObject<List<int>>(reader.GetString(4));
                break;
            }

            cmd.Dispose();
            dbCon.Close();
            return p;
        }




        public List<Person> GetPeople()
        {
            List<Person> people = new List<Person>();
            dbCon.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM people", dbCon);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Person p = new Person();
                p.Id = reader.GetInt32(0);
                p.Name = reader.GetString(1);
                p.Phone = reader.GetString(2);
                p.Email = reader.GetString(3);
                p.InquiryIds = JsonConvert.DeserializeObject<List<int>>(reader.GetString(4));
                people.Add(p);
            }

            cmd.Dispose();
            dbCon.Close();
            return people;
        }

        public Inquiry CreateInquiry(Inquiry i)
        {
            if (GetPerson(i.PersonId) == null) {
                return null;
            }

            if (GetProperty(i.PropertyId) == null)
            {
                return null;
            }

            dbCon.Open();

            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO people (Message, PropertyId, PersonId) VALUES (@msg, @propId, @perId)", dbCon);
            cmd.Parameters.AddWithValue("@msg", i.Message);
            cmd.Parameters.AddWithValue("@propId", i.PropertyId);
            cmd.Parameters.AddWithValue("@perId", i.PersonId);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            cmd.Dispose();

            i.Id = (int)dbCon.LastInsertRowId;

            dbCon.Close();
            return null;
        }

        public Inquiry CreateInquiry(string msg, int propId, int perId)
        {
            Inquiry i = new Inquiry()
            {
                Message = msg,
                PropertyId = propId,
                PersonId = perId
            };
            return CreateInquiry(i);
        }

        public Inquiry GetInquiry(int id)
        {
            dbCon.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM inquries WHERE id = @id", dbCon);
            cmd.Parameters.AddWithValue("@id", id);

            Inquiry i = null;
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                i = new Inquiry();
                i.Id = reader.GetInt32(0);
                i.Message = reader.GetString(1);
                i.PropertyId = reader.GetInt32(2);
                i.PersonId = reader.GetInt32(3);
                break;
            }

            cmd.Dispose();
            dbCon.Close();
            return i;
        }

    }
}
