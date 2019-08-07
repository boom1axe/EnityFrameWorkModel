using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnityFrameWorkModel
{
    class Program
    {
        static void Main(string[] args)
        {
            APEntities context = new APEntities();
            context.Database.Log = Console.WriteLine;

            Vendor ibm =
                (from v in context.Vendors
                 where v.VendorName == "IBM"
                 select v).SingleOrDefault();
            if (ibm != null)
            {
                Console.WriteLine(ibm.VendorID);
            }

            Console.ReadKey();

            int numVendors =
                (from v in context.Vendors
                 select v).Count();
            Console.WriteLine("Count is: " + numVendors);

            Console.ReadKey();

            //get all CA state vendors 
            List<Vendor> allCAVendors =
                (from v in context.Vendors
                where v.VendorState == "CA"
                orderby v.VendorName ascending
                select v).ToList();

            foreach (Vendor currVendor in allCAVendors)
            {
                Console.WriteLine(currVendor.VendorName);
            }

            //get vendor name and location
            //Name, City, State
            var vendorLocations =
                (from v in context.Vendors
                select new
                {
                    v.VendorName,
                    v.VendorState,
                    v.VendorCity
                }).ToList();

            // Ucomment to see all Vendor Locations
            //foreach (var venLoc in vendorLocations)
            //{
            //    Console.WriteLine($"{venLoc.VendorName} is at \n" +
            //        $" {venLoc.VendorCity} : {venLoc.VendorState} \n\n");
            //}

            //Get all Vendors and their Invoices
            List<Vendor> vendors =
                //(from v in context.Vendors
                // join inv in context.Invoices on
                //    v.VendorID equals inv.VendorID
                // select v).ToList();

                //left join
                //Gets all Vendors and any invoices they may have
                (from v in context.Vendors
                                    .Include(nameof(Vendor.Invoices))
                 select v).ToList();

            foreach (Vendor v in vendors)
            {
                Console.WriteLine(v.VendorName);
                foreach (Invoice inv in v.Invoices)
                {
                    Console.WriteLine($"\t{inv.InvoiceNumber}");
                }
            }

            Console.ReadKey();
        }
    }
}
