using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Judy.Models
{
    public class Inquiry
    {
        /*
         CREATE TABLE "inquiries" (
	        "Id"	INTEGER NOT NULL UNIQUE,
	        "Message"	TEXT,
	        "PropertyId"	INTEGER NOT NULL,
	        "PersonId"	INTEGER NOT NULL,
	        PRIMARY KEY("Id")
        ); 
         */

        public int Id { get; set; }
        public string Message { get; set; }
        public int PropertyId { get; set; }
        public int PersonId { get; set; }
    }
}
