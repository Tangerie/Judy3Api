using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Judy.Models
{
    /*
    CREATE TABLE "properties" (
	    "Id"	INTEGER NOT NULL UNIQUE,
	    "Address"	TEXT NOT NULL,
	    "IsActive"	INTEGER NOT NULL DEFAULT 1,
	    "ResponseMessage"	TEXT NOT NULL,
	    "InquiryIds"	TEXT,
	    PRIMARY KEY("Id")
    );
     */
    public class Property
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string ResponseMessage { get; set; }

        public List<int> InquiryIds { get; set; }
    }
}
