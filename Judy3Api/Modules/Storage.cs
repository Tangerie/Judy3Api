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

        public string DatabaseLocation { get; private set; }

        public Storage(string db)
        {
            DatabaseLocation = db;
        }

        private SQLiteConnection GetConnection()
        {
            var con = new SQLiteConnection($"Data Source={DatabaseLocation}");
            con.Open();
            return con;
        }


        public string GetVersion()
        {
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT SQLITE_VERSION()", con))
                {
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        
        public Property CreateProperty(Property p)
        {
            using (var con = GetConnection())
            {
                using (var cmd = new SQLiteCommand("INSERT INTO properties (Address, ResponseMessage, InquiryIds) VALUES (@addr, @res, @inq)", con))
                {
                    cmd.Parameters.AddWithValue("@addr", p.Address);
                    cmd.Parameters.AddWithValue("@res", p.ResponseMessage);
                    cmd.Parameters.AddWithValue("@inq", JsonConvert.SerializeObject(p.InquiryIds));

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    p.Id = (int)con.LastInsertRowId;
                    return p;
                }
            }
        }
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
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM properties WHERE id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Property p = new Property();
                            p.Id = reader.GetInt32(0);
                            p.Address = reader.GetString(1);
                            p.IsActive = reader.GetBoolean(2);
                            p.ResponseMessage = reader.GetString(3);
                            p.InquiryIds = JsonConvert.DeserializeObject<List<int>>(reader.GetString(4));
                            return p;
                        }
                        return null;
                    }
                }
            }
        }
        public List<Property> GetProperties()
        {
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM properties", con))
                {
                    List<Property> properties = new List<Property>();

                    using (var reader = cmd.ExecuteReader())
                    {

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
                        return properties;
                    }
                }
            }
        }
        public bool UpdateProperty(Property p)
        {
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("UPDATE properties SET Address = @addr, ResponseMessage = @res, InquiryIds = @inq, IsActive = @active WHERE Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@addr", p.Address);
                    cmd.Parameters.AddWithValue("@res", p.ResponseMessage);
                    cmd.Parameters.AddWithValue("@inq", JsonConvert.SerializeObject(p.InquiryIds));
                    cmd.Parameters.AddWithValue("@active", p.IsActive);
                    cmd.Parameters.AddWithValue("@id", p.Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public Person CreatePerson(Person p)
        {
            using (var con = GetConnection())
            {
                using (var cmd = new SQLiteCommand("INSERT INTO people (Name, Phone, Email, InquiryIds) VALUES (@name, @phone, @email, @inq)", con))
                {
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

                    p.Id = (int)con.LastInsertRowId;
                    return p;
                }
            }
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
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM people WHERE id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Person p = new Person();
                            p.Id = reader.GetInt32(0);
                            p.Name = reader.GetString(1);
                            p.Phone = reader.GetString(2);
                            p.Email = reader.GetString(3);
                            p.InquiryIds = JsonConvert.DeserializeObject<List<int>>(reader.GetString(4));
                            return p;
                        }
                        return null;
                    }
                }
            }

        }
        public List<Person> GetPeople()
        {
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM people", con))
                {
                    List<Person> people = new List<Person>();
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
                    return people;
                }
            }
        }
        public bool UpdatePerson(Person p)
        {
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("UPDATE properties SET Name = @name, Phone = @phone, Email = @email, InquiryIds = @inq WHERE Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@name", p.Name);
                    cmd.Parameters.AddWithValue("@phone", p.Phone);
                    cmd.Parameters.AddWithValue("@email", p.Email);
                    cmd.Parameters.AddWithValue("@inq", JsonConvert.SerializeObject(p.InquiryIds));
                    cmd.Parameters.AddWithValue("@id", p.Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
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

            using (var con = GetConnection())
            {
                using (var cmd = new SQLiteCommand("INSERT INTO people (Message, PropertyId, PersonId) VALUES (@msg, @propId, @perId)", con))
                {
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

                    i.Id = (int)con.LastInsertRowId;
                    return i;
                }
            }
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
            using (var con = GetConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM inquiries WHERE id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Inquiry i = new Inquiry();
                            i.Id = reader.GetInt32(0);
                            i.Message = reader.GetString(1);
                            i.PropertyId = reader.GetInt32(2);
                            i.PersonId = reader.GetInt32(3);
                            return i;
                        }
                        return null;
                    }
                }
            }
        }
    }
}
