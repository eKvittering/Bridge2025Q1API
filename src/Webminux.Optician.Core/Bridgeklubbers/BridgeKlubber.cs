using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bridgeklubbers
{
    public class BridgeKlub
    {
        /// <summary>
        /// Unique identifier (not necessarily a primary key).
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// Club number.
        /// </summary>
        public int? KLUBNUMMER { get; set; }

        /// <summary>
        /// Name of the club.
        /// </summary>
        public string NAVN { get; set; }

        /// <summary>
        /// Foreign key to the district ID.
        /// </summary>
        public int? FKDISTRIKTSID { get; set; }

        /// <summary>
        /// Date the club was created.
        /// </summary>
        public DateTime? OPRETTET { get; set; }

        /// <summary>
        /// Date the club data was last changed.
        /// </summary>
        public DateTime? AENDRET { get; set; }

        /// <summary>
        /// Type of the club (code/enum).
        /// </summary>
        public short? KLUBTYPE { get; set; }

        /// <summary>
        /// Start date of the club.
        /// </summary>
        public DateTime? STARTDATO { get; set; }

        /// <summary>
        /// Venue or place where the club meets.
        /// </summary>
        public string SPILLESTED { get; set; }

        /// <summary>
        /// Address line 1 of the club.
        /// </summary>
        public string ADRESSE1 { get; set; }

        /// <summary>
        /// Address line 2 of the club (optional).
        /// </summary>
        public string ADRESSE2 { get; set; }

        /// <summary>
        /// Telephone number.
        /// </summary>
        public string TELEFON { get; set; }

        /// <summary>
        /// Fax number.
        /// </summary>
        public string TELEFAX { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// Website URL.
        /// </summary>
        public string WWW { get; set; }

        /// <summary>
        /// Notes about the club (stored as binary/blob).
        /// </summary>
        public byte[] NOTER { get; set; }

        /// <summary>
        /// Season start, probably in MM-DD or similar short format.
        /// </summary>
        public string SAESONSTART { get; set; }

        /// <summary>
        /// Master point system used by the club.
        /// </summary>
        public short? MPORDNING { get; set; }

        /// <summary>
        /// Non-smoking flag (1 = non-smoking).
        /// </summary>
        public short? IKKERYGER { get; set; }

        /// <summary>
        /// Installation code (used for system registration).
        /// </summary>
        public string INSTALLATIONSKODE { get; set; }

        /// <summary>
        /// Security code for the club system.
        /// </summary>
        public string SIKKERHEDSKODE { get; set; }

        /// <summary>
        /// Indicates whether the club uses Bridge Central (BRUGERBC).
        /// </summary>
        public short? BRUGERBC { get; set; }

        /// <summary>
        /// Indicates whether to print club labels.
        /// </summary>
        public short? UDSKRIV_KLUBLABELS { get; set; }

        /// <summary>
        /// Indicates if the club is active.
        /// </summary>
        public short? AKTIV { get; set; }

        /// <summary>
        /// Indicates if the club receives the database.
        /// </summary>
        public short? MODTAGER_DB { get; set; }

        /// <summary>
        /// First year when the club had members.
        /// </summary>
        public int? FOERSTE_MEDLEMSAAR { get; set; }

        /// <summary>
        /// Half kilometer based membership fee? (likely fee calculation flag).
        /// </summary>
        public short? HALVT_KM_KONTINGENT { get; set; }

        /// <summary>
        /// Should be updated in the WinFinans system.
        /// </summary>
        public short? OPDATERWINFINANS { get; set; }

        /// <summary>
        /// Indicates if the club offers lessons or training.
        /// </summary>
        public short? TILBYDER_UNDERVISNING { get; set; }

        /// <summary>
        /// Foreign key to country code.
        /// </summary>
        public string FKLANDEKODE { get; set; }

        /// <summary>
        /// Foreign key to postal code.
        /// </summary>
        public string FKPOSTNR { get; set; }

        /// <summary>
        /// Export code used for external data handling.
        /// </summary>
        public int? EKSPORTKODE { get; set; }

        /// <summary>
        /// Username for export system.
        /// </summary>
        public string EKSPORTBRUGER { get; set; }

        /// <summary>
        /// Bank account number.
        /// </summary>
        public string BANKKONTO { get; set; }

        /// <summary>
        /// Transfer indicator (used in system sync).
        /// </summary>
        public short? OVERFOERES { get; set; }

        /// <summary>
        /// Local installation flag.
        /// </summary>
        public short? INSTLOKALT { get; set; }

        /// <summary>
        /// Excludes HCP system (Handicap system) flag.
        /// </summary>
        public short? FRAVAELGHAC { get; set; }

        /// <summary>
        /// FTP code (possibly for file transfer or account).
        /// </summary>
        public string FTPKODE { get; set; }

        /// <summary>
        /// Percentage of Danish Bridge involvement.
        /// </summary>
        public int? DANSK_BRIDGE_PROCENT { get; set; }

        /// <summary>
        /// Number of members or events involving Danish Bridge.
        /// </summary>
        public int? DANSK_BRIDGE_ANTAL { get; set; }
    }

}
