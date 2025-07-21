using System;
using Abp.MultiTenancy;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Companies;

namespace Webminux.Optician.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public virtual Company Company { get; set; }

        // Club Number
        public int? Klubnummer { get; set; }

        // Name
        //public string? Navn { get; set; }

        // District ID (Foreign Key)
        public int? FKDistriktsId { get; set; }

        // Created Date
      //  public DateTime? Oprettet { get; set; }

        // Modified Date
       // public DateTime? Aendret { get; set; }

        // Club Type
        public short? Klubtype { get; set; }

        // Start Date
        public DateTime? Startdato { get; set; }

        // Venue
        public string? Spillested { get; set; }

        // Address 1
        public string? Adresse1 { get; set; }

        // Address 2
        public string? Adresse2 { get; set; }

        // Phone
        public string? Telefon { get; set; }

        // Fax
        public string? Telefax { get; set; }

        // Email
        public string? Email { get; set; }

        // Website
        public string? Www { get; set; }

        // Notes
        public byte[]? Noter { get; set; }

        // Season Start
        public string? Saesonstart { get; set; }

        // MP Arrangement
        public short? Mpordning { get; set; }

        // Non-Smoker
        public short? Ikkeryger { get; set; }

        // Installation Code
        public string? Installationskode { get; set; }

        // Security Code
        public string? Sikkerhedskode { get; set; }

        // Uses BridgeCentral
        public short? BrugerBC { get; set; }

        // Print Club Labels
        public short? Udskriv_klubLabels { get; set; }

        // Is Active
        //public short? Aktiv { get; set; }

        // Receives DB
        public short? Modtager_DB { get; set; }

        // First Membership Year
        public int? Foerste_medlemsaAr { get; set; }

        // Half KM Fee
        public short? Halvt_KM_Kontingent { get; set; }

        // Update WinFinans
        public short? OpdaterWinFinans { get; set; }

        // Offers Teaching
        public short? Tilbyder_undervisning { get; set; }

        // Country Code (Foreign Key)
        public string? FKLandekode { get; set; }

        // Postal Code (Foreign Key)
        public string? FKPostnr { get; set; }

        // Export Code
        public int? Eksportkode { get; set; }

        // Export User
        public string? Eksportbruger { get; set; }

        // Bank Account
        public string? Bankkonto { get; set; }

        // Is Transferred
        public short? Overfoeres { get; set; }

        // Local Installation
        public short? Instlokalt { get; set; }

        // Exclude HAC
        public short? FravaelgHAC { get; set; }

        // FTP Code
        public string? FTPKode { get; set; }

        // Danish Bridge Percentage
        public int? Dansk_bridge_procent { get; set; }

        // Danish Bridge Count
        public int? Dansk_bridge_antal { get; set; }
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
