using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Webminux.Optician
{
    public class MEDLEMMER:Entity
    {
        /// <summary>
        /// Unique identifier (may not be primary key).
        /// </summary>
        [Key]
        public int? ID { get; set; }

        /// <summary>
        /// Member number (CustomerNo).
        /// </summary>
        public int? NUMMER { get; set; }

        /// <summary>
        /// Date the member was created.
        /// </summary>
        public DateTime? OPRETTET { get; set; }

        /// <summary>
        /// Date the member data was last changed.
        /// </summary>
        public DateTime? AENDRET { get; set; }

        /// <summary>
        /// First name of the member.
        /// </summary>
        public string FORNAVN { get; set; }

        /// <summary>
        /// Middle name of the member.
        /// </summary>
        public string MELLEMNAVN { get; set; }

        /// <summary>
        /// Last name (surname) of the member.
        /// </summary>
        public string EFTERNAVN { get; set; }

        /// <summary>
        /// Primary address line (Address).
        /// </summary>
        public string ADRESSE1 { get; set; }

        /// <summary>
        /// Secondary address line (optional).
        /// </summary>
        public string ADRESSE2 { get; set; }

        /// <summary>
        /// Private phone number.
        /// </summary>
        public string PRIVATTELEFON { get; set; }

        /// <summary>
        /// Work phone number.
        /// </summary>
        public string ARBEJDSTELEFON { get; set; }

        /// <summary>
        /// Mobile phone number (TelephoneFax).
        /// </summary>
        public string MOBILTELEFON { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        public DateTime? FOEDSELSDAG { get; set; }

        /// <summary>
        /// Notes about the member (stored as image/blob in database).
        /// </summary>
        public byte[] NOTER { get; set; }

        /// <summary>
        /// Possibly year of membership or joining.
        /// </summary>
        public short? K1AAR { get; set; }

        /// <summary>
        /// Does not wish to appear in the address book (flag).
        /// </summary>
        public short? DB_OENSKERIKKE { get; set; }

        /// <summary>
        /// Always send printed material (flag).
        /// </summary>
        public short? DB_SENDALTID { get; set; }

        /// <summary>
        /// Invalid address flag.
        /// </summary>
        public short? DB_UGYLDIG_ADRESSE { get; set; }

        /// <summary>
        /// Indicates if the family receives the magazine.
        /// </summary>
        public short? DB_FAMILIEN_FAAR_BLADET { get; set; }

        /// <summary>
        /// Indicates if the member is deceased.
        /// </summary>
        public short? DOED { get; set; }

        /// <summary>
        /// Wishes to receive a badge/pin (flag).
        /// </summary>
        public short? NAAL_OENSKES { get; set; }

        /// <summary>
        /// Should be updated in the WinFinans system (flag).
        /// </summary>
        public short? OPDATERWINFINANS { get; set; }

        /// <summary>
        /// Foreign key to country code (Country).
        /// </summary>
        public string FKLANDEKODE { get; set; }

        /// <summary>
        /// Foreign key to postal code (Postcode).
        /// </summary>
        public string FKPOSTNR { get; set; }

        /// <summary>
        /// Indicates if the member maintains their own profile data.
        /// </summary>
        public short? VEDLIGEHOLDER_EGNE_STAMDATA { get; set; }

        /// <summary>
        /// Web access code or password (Website).
        /// </summary>
        public string WEB_ADGANGSKODE { get; set; }

        /// <summary>
        /// Member title (foreign key to title table).
        /// </summary>
        public int? MPTITEL { get; set; }

        /// <summary>
        /// Last time the title was changed.
        /// </summary>
        public DateTime? MPTITEL_AENDRET { get; set; }

        /// <summary>
        /// Possibly handicap index or similar rating.
        /// </summary>
        public double? NUVHAC { get; set; }

        /// <summary>
        /// Has an alternative address (flag).
        /// </summary>
        public short? ALTERNATIV_ADRESSE { get; set; }

        /// <summary>
        /// Trial flag or test field (likely temporary/test purpose).
        /// </summary>
        public char? TRIAL675 { get; set; }
    }

}
