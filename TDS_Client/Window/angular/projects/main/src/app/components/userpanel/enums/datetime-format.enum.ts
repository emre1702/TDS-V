export enum DateTimeFormatEnum {
	"EEEE, dd MMMM yyyy hh:mm:ss a" = "dddd, dd MMMM yyyy hh:mm:ss tt",	        // Tuesday, 22 August 2006 06:30:07 AM
	"EEEE, dd MMMM yyyy h:mm:ss a" = "dddd, dd MMMM yyyy h:mm:ss tt",	        // Tuesday, 22 August 2006 6:30:07 AM
    "EEEE, dd MMMM yyyy HH:mm:ss" = "dddd, dd MMMM yyyy HH:mm:ss",	            // Tuesday, 22 August 2006 06:30:07
    "EEE, dd MMM yyyy HH':'mm':'ss" = "ddd, dd MMM yyyy HH':'mm':'ss",	        // Tue, 22 Aug 2006 06:30:07

	"MM/dd/yyyy HH:mm:ss" = "MM/dd/yyyy HH:mm:ss" ,	                            // 08/22/2006 06:30:07
	"MM/dd/yyyy hh:mm:ss a" = "MM/dd/yyyy hh:mm:ss tt",	                        // 08/22/2006 06:30:07 AM
	"MM/dd/yyyy H:mm:ss" = "MM/dd/yyyy H:mm:ss",	                            // 08/22/2006 6:30:07
    "MM/dd/yyyy h:mm:ss a" = "MM/dd/yyyy h:mm:ss tt",	                        // 08/22/2006 6:30:07 AM

    "dd.MM.yyyy HH:mm:ss" = "MM/dd/yyyy HH:mm:ss" ,	                            // 22.08.2006 06:30:07
	"dd.MM.yyyy hh:mm:ss a" = "MM/dd/yyyy hh:mm:ss tt",	                        // 22.08.2006 06:30:07 AM
	"dd.MM.yyyy H:mm:ss" = "MM/dd/yyyy H:mm:ss",	                            // 22.08.2006 6:30:07
	"dd.MM.yyyy h:mm:ss a" = "MM/dd/yyyy h:mm:ss tt",	                        // 22.08.2006 6:30:07 AM

	"yyyy'-'MM'-'dd'T'HH':'mm':'ss" = "yyyy'-'MM'-'dd'T'HH':'mm':'ss",	        // 2006-08-22T06:30:07
    "yyyy'-'MM'-'dd HH':'mm':'ss" = "yyyy'-'MM'-'dd HH':'mm':'ss",	            // 2006-08-22 06:30:07

    "dd'-'MM'-'yyyy'T'HH':'mm':'ss" = "dd'-'MM'-'yyyy'T'HH':'mm':'ss",	        // 22-08-2006T06:30:07
    "dd'-'MM'-'yyyy HH':'mm':'ss" = "dd'-'MM'-'yyyy HH':'mm':'ss",	            // 22-08-2006 06:30:07
}
