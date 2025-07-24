using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Webminux.Optician;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Companies;
using Webminux.Optician.Core;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.MultiTenancy;
using static AutoMapper.Internal.ExpressionFactory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Webminux.Optician.Authorization.Roles.StaticRoleNames;

namespace Webminux.Optician.SynTableData
{
    /// <summary>
    /// This is an application service class for <see cref="SyncTableDataAppService"/> entity.
    /// </summary>
    //[AbpAuthorize]
    public class SyncTableDataAppService : OpticianAppServiceBase, ISyncTableDataAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<BRIDGEKLUBBER> _BRIDGEKLUBBERRepository;
        private readonly IRepository<MEDLEMMER> _MEDLEMMERRepository;
        private readonly IRepository<MEDLEMSKABER> _MEDLEMSKABERRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<UserTenant> _userTenantRepository;
        internal readonly UserManager _userManager;
        internal readonly IRepository<UserType, int> _userTypeRepository;
        private readonly string _conn = "Data Source=mssql3.unoeuro.com;Initial Catalog=crm_beckit_dk_db_1;User Id=crm_beckit_dk;Password=y2eBt39pafx4; connect timeout=10000";

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncTableDataAppService"/> class.
        /// </summary>
        public SyncTableDataAppService(IUnitOfWorkManager unitOfWorkManager,
            IRepository<MEDLEMMER> MEDLEMMERRepository, IRepository<MEDLEMSKABER> MEDLEMSKABERRepository,
        IRepository<BRIDGEKLUBBER> BRIDGEKLUBBERRepository, IRepository<Tenant> tenantRepository,
        IRepository<Customer> customerRepository, IRepository<UserTenant> userTenantRepository,
        UserManager userManager, IRepository<UserType, int> userTypeRepository
        )
        {
            _unitOfWorkManager = unitOfWorkManager;
            _BRIDGEKLUBBERRepository = BRIDGEKLUBBERRepository;
            _MEDLEMMERRepository = MEDLEMMERRepository;
            _MEDLEMSKABERRepository = MEDLEMSKABERRepository;
            _tenantRepository = tenantRepository;
            _customerRepository = customerRepository;
            _userTenantRepository = userTenantRepository;
            _userManager = userManager;
            _userTypeRepository = userTypeRepository;
        }
        /// <summary>
        /// This is an application service class for Import Data for Clubs and Members.
        /// </summary>
        public async Task<bool> ImportCustomersData()
        {
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
                {
                    var userTypeId = _userTypeRepository.GetAll().Where(x => x.Name == OpticianConsts.UserTypes.Employee).FirstOrDefault().Id;
                    var userId = AbpSession.UserId ?? 1;

                    var clientClubs = await _BRIDGEKLUBBERRepository.GetAllListAsync();
                    var clientMembers = await _MEDLEMMERRepository.GetAllListAsync();
                    var clientClubMembers = await _MEDLEMSKABERRepository.GetAllListAsync();


                    foreach (var item in clientClubMembers)
                    {
                        var clientTenant = clientClubs.FirstOrDefault(x => x.ID == item.FKBRIDGEKLUBID);
                        if (clientTenant == null)
                        {
                            continue;
                        }

                        var member = clientMembers.FirstOrDefault(x => x.ID == item.FKMEDLEMSID);
                        if (member == null)
                        {
                            continue;
                        }

                        var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.TenancyName == clientTenant.NAVN);
                        if (tenant == null)
                        {
                            continue;
                        }


                        var existingMember = await _customerRepository
                           .FirstOrDefaultAsync(t => t.CustomerNo.ToLower() == member.NUMMER.ToString().ToLower());

                        if (existingMember == null)
                        {
                            User newUser = null; // Variable to hold the new user

                            // Check if a user with the same email already exists
                            var userDbo = _userManager.Users.FirstOrDefault(x => x.EmailAddress == member.EMAIL);

                            if (userDbo == null)
                            {
                                await InsertUserRecord(userTypeId, userId, member, tenant);
                                // Fetch the newly created user
                                newUser = _userManager.Users.OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            else
                            {
                                // If user exists, assign the existing user to newUser
                                newUser = userDbo;
                            }

                            await InsertCustomerRecord(userId, member, tenant);
                            await InsertUserTenantRecord(tenant, newUser);

                        }


                    }
                }

                return true;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        private async Task InsertUserRecord(int userTypeId, long userId, MEDLEMMER member, Tenant tenant)
        {
            using (var connection = new SqlConnection(_conn))
            {
                await connection.OpenAsync();

                var insertCommand = new SqlCommand(@"
                                                INSERT INTO AbpUsers (
                                                    UserTypeId, Password, EmailAddress, UserName, CreationTime, CreatorUserId,
                                                    Name, Surname, TenantId, IsActive,IsDeleted,AccessFailedCount,IsLockoutEnabled,
                                                   IsPhoneNumberConfirmed,IsTwoFactorEnabled,IsEmailConfirmed,NormalizedUserName ,NormalizedEmailAddress,
                                                    CanAnswerPhoneCalls,CanOpenCloseShop,IsReceptionAllowed,IsResponsibleForStocks,IsAdmin
                                                )
                                                VALUES (
                                                    @UserTypeId, @Password, @EmailAddress, @UserName, @CreationTime, @CreatorUserId,
                                                    @Name, @Surname, @TenantId, @IsActive,@IsDeleted,@AccessFailedCount,@IsLockoutEnabled,
                                                       @IsPhoneNumberConfirmed,@IsTwoFactorEnabled,@IsEmailConfirmed,@NormalizedUserName,@NormalizedEmailAddress,@CanAnswerPhoneCalls
                                                    ,@CanOpenCloseShop,@IsReceptionAllowed,@IsResponsibleForStocks,@IsAdmin
                                                )", connection);

                // Add parameters
                insertCommand.Parameters.AddWithValue("@UserTypeId", userTypeId);
                insertCommand.Parameters.AddWithValue("@Password", "AQAAAAEAACcQAAAAEOAjE4XA68JRcZDqTtjwDoMhTRgyy9dookaozUJ3hXTWsMFHhN6GrzDv2GKwh01wuQ=="); // 🔐 Consider hashing
                insertCommand.Parameters.AddWithValue("@EmailAddress", member.EMAIL ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UserName", member.EMAIL ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CreationTime", DateTime.UtcNow);
                insertCommand.Parameters.AddWithValue("@CreatorUserId", userId);
                insertCommand.Parameters.AddWithValue("@Name", member.FORNAVN ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Surname", member.EFTERNAVN ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TenantId", tenant.Id);
                insertCommand.Parameters.AddWithValue("@IsActive", true);
                insertCommand.Parameters.AddWithValue("@IsDeleted", false);
                insertCommand.Parameters.AddWithValue("@AccessFailedCount", 0);
                insertCommand.Parameters.AddWithValue("@IsLockoutEnabled", 0);
                insertCommand.Parameters.AddWithValue("@IsPhoneNumberConfirmed", 0);
                insertCommand.Parameters.AddWithValue("@IsTwoFactorEnabled", 0);
                insertCommand.Parameters.AddWithValue("@IsEmailConfirmed", 1);
                insertCommand.Parameters.AddWithValue("@NormalizedUserName", member.FORNAVN);
                insertCommand.Parameters.AddWithValue("@NormalizedEmailAddress", member.EMAIL);
                insertCommand.Parameters.AddWithValue("@CanAnswerPhoneCalls", 0);
                insertCommand.Parameters.AddWithValue("@CanOpenCloseShop", 0);
                insertCommand.Parameters.AddWithValue("@IsReceptionAllowed", 0);
                insertCommand.Parameters.AddWithValue("@IsResponsibleForStocks", 0);
                insertCommand.Parameters.AddWithValue("@IsAdmin", false);

                await insertCommand.ExecuteNonQueryAsync();

            }
        }

        private async Task InsertCustomerRecord(long userId, MEDLEMMER member, Tenant tenant)
        {
            using (var connection = new SqlConnection(_conn))
            {
                await connection.OpenAsync();

                var insertCommand = new SqlCommand(@"
                                   INSERT INTO Customers (
                                        TenantId, CustomerNo, Address, Postcode, TownCity, Country, TelephoneFax, Website,
                                        Currency, Site, IsSync, IsSubCustomer, NUMMER, OPRETTET, AENDRET, FORNAVN,
                                        MELLEMNAVN, EFTERNAVN, ADRESSE1, ADRESSE2, PRIVATTELEFON, ARBEJDSTELEFON,
                                        MOBILTELEFON, EMAIL, FOEDSELSDAG, NOTER, K1AAR, DB_OENSKERIKKE, DB_SENDALTID,
                                        DB_UGYLDIG_ADRESSE, DB_FAMILIEN_FAAR_BLADET, DOED, NAAL_OENSKES, OPDATERWINFINANS,
                                        FKLANDEKODE, FKPOSTNR, VEDLIGEHOLDER_EGNE_STAMDATA, WEB_ADGANGSKODE, MPTITEL,
                                        MPTITEL_AENDRET, NUVHAC, ALTERNATIV_ADRESSE, TRIAL675, UserId
                                    )
                                    VALUES (
                                        @TenantId, @CustomerNo, @Address, @Postcode, @TownCity, @Country, @TelephoneFax, @Website,
                                        @Currency, @Site, @IsSync, @IsSubCustomer, @NUMMER, @OPRETTET, @AENDRET, @FORNAVN,
                                        @MELLEMNAVN, @EFTERNAVN, @ADRESSE1, @ADRESSE2, @PRIVATTELEFON, @ARBEJDSTELEFON,
                                        @MOBILTELEFON, @EMAIL, @FOEDSELSDAG, @NOTER, @K1AAR, @DB_OENSKERIKKE, @DB_SENDALTID,
                                        @DB_UGYLDIG_ADRESSE, @DB_FAMILIEN_FAAR_BLADET, @DOED, @NAAL_OENSKES, @OPDATERWINFINANS,
                                        @FKLANDEKODE, @FKPOSTNR, @VEDLIGEHOLDER_EGNE_STAMDATA, @WEB_ADGANGSKODE, @MPTITEL,
                                        @MPTITEL_AENDRET, @NUVHAC, @ALTERNATIV_ADRESSE, @TRIAL675, @UserId
);
                                ", connection);

                insertCommand.Parameters.AddWithValue("@TenantId", tenant.Id);
                insertCommand.Parameters.AddWithValue("@CustomerNo", member.NUMMER?.ToString() ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address", member.ADRESSE1 ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Postcode", member.FKPOSTNR ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TownCity", member.ADRESSE2 ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Country", member.FKLANDEKODE ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TelephoneFax",
                    member.MOBILTELEFON ?? member.ARBEJDSTELEFON ?? member.PRIVATTELEFON ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Website", (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Currency", (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Site", (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSync", false);
                insertCommand.Parameters.AddWithValue("@IsSubCustomer", false);

                // Custom fields
                insertCommand.Parameters.AddWithValue("@NUMMER", member.NUMMER ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@OPRETTET", member.OPRETTET ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AENDRET", member.AENDRET ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@FORNAVN", member.FORNAVN ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MELLEMNAVN", member.MELLEMNAVN ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EFTERNAVN", member.EFTERNAVN ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ADRESSE1", member.ADRESSE1 ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ADRESSE2", member.ADRESSE2 ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PRIVATTELEFON", member.PRIVATTELEFON ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ARBEJDSTELEFON", member.ARBEJDSTELEFON ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MOBILTELEFON", member.MOBILTELEFON ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EMAIL", member.EMAIL ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@FOEDSELSDAG", member.FOEDSELSDAG ?? (object)DBNull.Value);

                // Binary data (e.g., NOTER)
                byte[] noterBytes = new byte[0];
                insertCommand.Parameters.Add("@NOTER", SqlDbType.VarBinary).Value = (object?)noterBytes ?? DBNull.Value;

                insertCommand.Parameters.AddWithValue("@K1AAR", member.K1AAR ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DB_OENSKERIKKE", member.DB_OENSKERIKKE ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DB_SENDALTID", member.DB_SENDALTID ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DB_UGYLDIG_ADRESSE", member.DB_UGYLDIG_ADRESSE ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DB_FAMILIEN_FAAR_BLADET", member.DB_FAMILIEN_FAAR_BLADET ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DOED", member.DOED ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@NAAL_OENSKES", member.NAAL_OENSKES ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@OPDATERWINFINANS", member.OPDATERWINFINANS ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@FKLANDEKODE", member.FKLANDEKODE ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@FKPOSTNR", member.FKPOSTNR ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@VEDLIGEHOLDER_EGNE_STAMDATA", member.VEDLIGEHOLDER_EGNE_STAMDATA ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@WEB_ADGANGSKODE", member.WEB_ADGANGSKODE ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MPTITEL", member.MPTITEL ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MPTITEL_AENDRET", member.MPTITEL_AENDRET ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@NUVHAC", member.NUVHAC ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ALTERNATIV_ADRESSE", member.ALTERNATIV_ADRESSE ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@TRIAL675", member.TRIAL675 ?? (object)DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UserId", userId);

                await insertCommand.ExecuteNonQueryAsync();
            }
        }

        private async Task InsertUserTenantRecord(Tenant tenant, User newUser)
        {
            using (var connection = new SqlConnection(_conn))
            {
                await connection.OpenAsync();

                var insertCommand = new SqlCommand(@"
                                INSERT INTO UserTenants (UserId, TenantId)
                                VALUES (@UserId, @TenantId)
                            ", connection);

                insertCommand.Parameters.AddWithValue("@UserId", newUser.Id);
                insertCommand.Parameters.AddWithValue("@TenantId", tenant.Id);

                await insertCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> ImportClubsData()
        {
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
                {
                    var userTypeId = _userTypeRepository.GetAll().Where(x => x.Name == OpticianConsts.UserTypes.Employee).FirstOrDefault().Id;
                    var userId = AbpSession.UserId ?? 1;

                    var clientClubs = await _BRIDGEKLUBBERRepository.GetAllListAsync();
                    foreach (var club in clientClubs)
                    {
                        await InsertTenantIfNotExistsAsync(club, userId, _conn);
                    }
                }

                return true;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        private async Task InsertTenantIfNotExistsAsync(BRIDGEKLUBBER club, long userId, string connectionString)
        {
            byte[] binaryData = new byte[] { 0x01, 0x02, 0x03 };
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM AbpTenants WHERE LOWER(TenancyName) = LOWER(@TenancyName)
                    )
                    BEGIN
                        INSERT INTO AbpTenants (
                            IsActive, IsDeleted, Klubnummer, FKDistriktsId, Name, TenancyName,
                            CreationTime, CreatorUserId, LastModificationTime, Klubtype, Startdato,
                            Spillested, Adresse1, Adresse2, Telefon, Telefax, Email, Www, Noter,
                            Saesonstart, Mpordning, Ikkeryger, Installationskode, Sikkerhedskode,
                            BrugerBC, Udskriv_klubLabels, Modtager_DB, Foerste_medlemsaAr,
                            Halvt_KM_Kontingent, OpdaterWinFinans, Tilbyder_undervisning,
                            FKLandekode, FKPostnr, Eksportkode, Eksportbruger, Bankkonto,
                            Overfoeres, Instlokalt, FravaelgHAC, FTPKode, Dansk_bridge_procent,
                            Dansk_bridge_antal
                        )
                        VALUES (
                            @IsActive, @IsDeleted, @Klubnummer, @FKDistriktsId, @Name, @TenancyName,
                            @CreationTime, @CreatorUserId, @LastModificationTime, @Klubtype, @Startdato,
                            @Spillested, @Adresse1, @Adresse2, @Telefon, @Telefax, @Email, @Www, @Noter,
                            @Saesonstart, @Mpordning, @Ikkeryger, @Installationskode, @Sikkerhedskode,
                            @BrugerBC, @Udskriv_klubLabels, @Modtager_DB, @Foerste_medlemsaAr,
                            @Halvt_KM_Kontingent, @OpdaterWinFinans, @Tilbyder_undervisning,
                            @FKLandekode, @FKPostnr, @Eksportkode, @Eksportbruger, @Bankkonto,
                            @Overfoeres, @Instlokalt, @FravaelgHAC, @FTPKode, @Dansk_bridge_procent,
                            @Dansk_bridge_antal
                        )
                    END", connection);

                // Set parameters
                command.Parameters.AddWithValue("@IsActive", true);
                command.Parameters.AddWithValue("@IsDeleted", false);
                command.Parameters.AddWithValue("@Klubnummer", club.KLUBNUMMER ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FKDistriktsId", club.FKDISTRIKTSID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Name", club.NAVN ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TenancyName", club.NAVN ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreationTime", club.OPRETTET ?? DateTime.UtcNow);
                command.Parameters.AddWithValue("@CreatorUserId", userId);
                command.Parameters.AddWithValue("@LastModificationTime", (object?)club.AENDRET ?? DBNull.Value);
                command.Parameters.AddWithValue("@Klubtype", (object?)club.KLUBTYPE ?? DBNull.Value);
                command.Parameters.AddWithValue("@Startdato", (object?)club.STARTDATO ?? DBNull.Value);
                command.Parameters.AddWithValue("@Spillested", club.SPILLESTED ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Adresse1", club.ADRESSE1 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Adresse2", club.ADRESSE2 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Telefon", club.TELEFON ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Telefax", club.TELEFAX ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", club.EMAIL ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Www", club.WWW ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Noter", binaryData);
                command.Parameters.AddWithValue("@Saesonstart", club.SAESONSTART ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Mpordning", (object?)club.MPORDNING ?? DBNull.Value);
                command.Parameters.AddWithValue("@Ikkeryger", (object?)club.IKKERYGER ?? DBNull.Value);
                command.Parameters.AddWithValue("@Installationskode", club.INSTALLATIONSKODE ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Sikkerhedskode", club.SIKKERHEDSKODE ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BrugerBC", (object?)club.BRUGERBC ?? DBNull.Value);
                command.Parameters.AddWithValue("@Udskriv_klubLabels", (object?)club.UDSKRIV_KLUBLABELS ?? DBNull.Value);
                command.Parameters.AddWithValue("@Modtager_DB", (object?)club.MODTAGER_DB ?? DBNull.Value);
                command.Parameters.AddWithValue("@Foerste_medlemsaAr", (object?)club.FOERSTE_MEDLEMSAAR ?? DBNull.Value);
                command.Parameters.AddWithValue("@Halvt_KM_Kontingent", (object?)club.HALVT_KM_KONTINGENT ?? DBNull.Value);
                command.Parameters.AddWithValue("@OpdaterWinFinans", (object?)club.OPDATERWINFINANS ?? DBNull.Value);
                command.Parameters.AddWithValue("@Tilbyder_undervisning", (object?)club.TILBYDER_UNDERVISNING ?? DBNull.Value);
                command.Parameters.AddWithValue("@FKLandekode", club.FKLANDEKODE ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FKPostnr", club.FKPOSTNR ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Eksportkode", club.EKSPORTKODE ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Eksportbruger", club.EKSPORTBRUGER ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Bankkonto", club.BANKKONTO ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Overfoeres", (object?)club.OVERFOERES ?? DBNull.Value);
                command.Parameters.AddWithValue("@Instlokalt", (object?)club.INSTLOKALT ?? DBNull.Value);
                command.Parameters.AddWithValue("@FravaelgHAC", (object?)club.FRAVAELGHAC ?? DBNull.Value);
                command.Parameters.AddWithValue("@FTPKode", club.FTPKODE ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Dansk_bridge_procent", (object?)club.DANSK_BRIDGE_PROCENT ?? DBNull.Value);
                command.Parameters.AddWithValue("@Dansk_bridge_antal", (object?)club.DANSK_BRIDGE_ANTAL ?? DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
        }


        public static Tenant MapBridgeKlubberToTenant(BRIDGEKLUBBER bridgeKlubber, long userId)
        {
            var tenant = new Tenant();
            tenant.IsActive = true;
            tenant.IsDeleted = false;

            // Map the Club Number
            tenant.Klubnummer = bridgeKlubber.KLUBNUMMER;

            // Map the District ID (Foreign Key)
            tenant.FKDistriktsId = bridgeKlubber.FKDISTRIKTSID;

            // Map the Club Name
            tenant.Name = bridgeKlubber.NAVN;
            tenant.TenancyName = bridgeKlubber.NAVN;

            // Map the Created Date
            tenant.CreationTime = bridgeKlubber.OPRETTET ?? DateTime.UtcNow;
            tenant.CreatorUserId = userId;

            // Map the Modified Date
            tenant.LastModificationTime = bridgeKlubber.AENDRET;

            // Map the Club Type
            tenant.Klubtype = bridgeKlubber.KLUBTYPE;

            // Map the Start Date
            tenant.Startdato = bridgeKlubber.STARTDATO;

            // Map the Venue
            tenant.Spillested = bridgeKlubber.SPILLESTED;

            // Map the Address 1
            tenant.Adresse1 = bridgeKlubber.ADRESSE1;

            // Map the Address 2 (optional)
            tenant.Adresse2 = bridgeKlubber.ADRESSE2;

            // Map the Phone
            tenant.Telefon = bridgeKlubber.TELEFON;

            // Map the Fax
            tenant.Telefax = bridgeKlubber.TELEFAX;

            // Map the Email
            tenant.Email = bridgeKlubber.EMAIL;

            // Map the Website URL
            tenant.Www = bridgeKlubber.WWW;

            // Map the Notes (binary data)
            tenant.Noter = bridgeKlubber.NOTER;

            // Map the Season Start (string format)
            tenant.Saesonstart = bridgeKlubber.SAESONSTART;

            // Map the MP Arrangement (short)
            tenant.Mpordning = bridgeKlubber.MPORDNING;

            // Map the Non-Smoker Flag (1 = non-smoking)
            tenant.Ikkeryger = bridgeKlubber.IKKERYGER;

            // Map the Installation Code
            tenant.Installationskode = bridgeKlubber.INSTALLATIONSKODE;

            // Map the Security Code
            tenant.Sikkerhedskode = bridgeKlubber.SIKKERHEDSKODE;

            // Map the BridgeCentral User Flag
            tenant.BrugerBC = bridgeKlubber.BRUGERBC;

            // Map the Print Club Labels Flag
            tenant.Udskriv_klubLabels = bridgeKlubber.UDSKRIV_KLUBLABELS;

            // Map the Active Flag (optional, since it's commented out in Tenant class)
            // tenant.Aktiv = bridgeKlubber.AKTIV;

            // Map the Receives DB Flag
            tenant.Modtager_DB = bridgeKlubber.MODTAGER_DB;

            // Map the First Membership Year
            tenant.Foerste_medlemsaAr = bridgeKlubber.FOERSTE_MEDLEMSAAR;

            // Map the Half KM Fee Flag
            tenant.Halvt_KM_Kontingent = bridgeKlubber.HALVT_KM_KONTINGENT;

            // Map the Update WinFinans Flag
            tenant.OpdaterWinFinans = bridgeKlubber.OPDATERWINFINANS;

            // Map the Offers Teaching Flag
            tenant.Tilbyder_undervisning = bridgeKlubber.TILBYDER_UNDERVISNING;

            // Map the Country Code (Foreign Key)
            tenant.FKLandekode = bridgeKlubber.FKLANDEKODE;

            // Map the Postal Code (Foreign Key)
            tenant.FKPostnr = bridgeKlubber.FKPOSTNR;

            // Map the Export Code
            tenant.Eksportkode = bridgeKlubber.EKSPORTKODE;

            // Map the Export User
            tenant.Eksportbruger = bridgeKlubber.EKSPORTBRUGER;

            // Map the Bank Account
            tenant.Bankkonto = bridgeKlubber.BANKKONTO;

            // Map the Is Transferred Flag
            tenant.Overfoeres = bridgeKlubber.OVERFOERES;

            // Map the Local Installation Flag
            tenant.Instlokalt = bridgeKlubber.INSTLOKALT;

            // Map the Exclude HAC Flag
            tenant.FravaelgHAC = bridgeKlubber.FRAVAELGHAC;

            // Map the FTP Code
            tenant.FTPKode = bridgeKlubber.FTPKODE;

            // Map the Danish Bridge Percentage
            tenant.Dansk_bridge_procent = bridgeKlubber.DANSK_BRIDGE_PROCENT;

            // Map the Danish Bridge Count
            tenant.Dansk_bridge_antal = bridgeKlubber.DANSK_BRIDGE_ANTAL;

            return tenant;
        }

        public static Customer MapToCustomer(MEDLEMMER member, long userId, int tenantId)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member), "The MEDLEMMER object cannot be null.");
            }

            // Create a new Customer object
            var customer = new Customer();

            // Set required fields first
            customer.TenantId = tenantId;
            customer.CustomerNo = member.NUMMER?.ToString() ?? string.Empty;
            customer.Address = member.ADRESSE1; // Primary address line
            customer.Postcode = member.FKPOSTNR; // Postal code
            customer.TownCity = member.ADRESSE2 ?? string.Empty; // Secondary address line or empty
            customer.Country = member.FKLANDEKODE; // Country code
            customer.TelephoneFax = member.MOBILTELEFON ?? member.ARBEJDSTELEFON ?? member.PRIVATTELEFON; // Prioritize mobile, work, then private phone
            customer.Website = string.Empty; // Add if needed
            customer.Currency = string.Empty; // Add currency info if available
            customer.Site = string.Empty; // Add site information if available
            customer.IsSync = false; // Set based on your business logic
            customer.IsSubCustomer = false; // Set based on your business logic

            // Set client-specific properties
            customer.NUMMER = member.NUMMER;
            customer.OPRETTET = member.OPRETTET;
            customer.AENDRET = member.AENDRET;
            customer.FORNAVN = member.FORNAVN;
            customer.MELLEMNAVN = member.MELLEMNAVN;
            customer.EFTERNAVN = member.EFTERNAVN;
            customer.ADRESSE1 = member.ADRESSE1;
            customer.ADRESSE2 = member.ADRESSE2;
            customer.PRIVATTELEFON = member.PRIVATTELEFON;
            customer.ARBEJDSTELEFON = member.ARBEJDSTELEFON;
            customer.MOBILTELEFON = member.MOBILTELEFON;
            customer.EMAIL = member.EMAIL;
            customer.FOEDSELSDAG = member.FOEDSELSDAG;
            customer.NOTER = member.NOTER;
            customer.K1AAR = member.K1AAR;
            customer.DB_OENSKERIKKE = member.DB_OENSKERIKKE;
            customer.DB_SENDALTID = member.DB_SENDALTID;
            customer.DB_UGYLDIG_ADRESSE = member.DB_UGYLDIG_ADRESSE;
            customer.DB_FAMILIEN_FAAR_BLADET = member.DB_FAMILIEN_FAAR_BLADET;
            customer.DOED = member.DOED;
            customer.NAAL_OENSKES = member.NAAL_OENSKES;
            customer.OPDATERWINFINANS = member.OPDATERWINFINANS;
            customer.FKLANDEKODE = member.FKLANDEKODE;
            customer.FKPOSTNR = member.FKPOSTNR;
            customer.VEDLIGEHOLDER_EGNE_STAMDATA = member.VEDLIGEHOLDER_EGNE_STAMDATA;
            customer.WEB_ADGANGSKODE = member.WEB_ADGANGSKODE;
            customer.MPTITEL = member.MPTITEL;
            customer.MPTITEL_AENDRET = member.MPTITEL_AENDRET;
            customer.NUVHAC = member.NUVHAC;
            customer.ALTERNATIV_ADRESSE = member.ALTERNATIV_ADRESSE;
            customer.TRIAL675 = member.TRIAL675;

            // Set relationship properties
            customer.UserId = userId; // Link the current user (can be passed in or obtained)
            //customer.ResponsibleEmployeeId = responsibleEmployeeId; // Responsible employee (can be null)
            customer.SubCustomers = new List<Customer>(); // Initialize empty list of sub-customers if needed
            customer.Notes = new List<Note>(); // Initialize empty list of notes if needed

            return customer;
        }
    }
}
