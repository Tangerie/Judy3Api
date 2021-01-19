using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Judy.Models
{
    public class Person
    {
        /*
        CREATE TABLE "people" (
	        "Id"	INTEGER NOT NULL UNIQUE,
	        "Name"	TEXT,
	        "Phone"	TEXT,
	        "Email"	TEXT,
	        "InquiryIds"	TEXT,
	        PRIMARY KEY("Id")
        );*/
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public List<int> InquiryIds { get; set; }
    }
}
