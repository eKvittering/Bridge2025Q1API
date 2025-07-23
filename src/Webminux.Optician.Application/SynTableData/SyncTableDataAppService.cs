using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public async Task<bool> ImportData()
        {
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
                {
                    var userTypeId = _userTypeRepository.GetAll().Where(x => x.Name == OpticianConsts.UserTypes.Employee).FirstOrDefault().Id;
                    var userId = 5;//AbpSession.UserId.Value;
                    
                    //using (var unitOfWork = _unitOfWorkManager.Begin())
                    //{
                   
                    var clientClubs = await _BRIDGEKLUBBERRepository.GetAllListAsync();
                    var clientMembers = await _MEDLEMMERRepository.GetAllListAsync();
                    var clientClubMembers = await _MEDLEMSKABERRepository.GetAllListAsync();

                    foreach (var club in clientClubs)
                    {
                        try
                        {
                            var existingTenant = _tenantRepository
                            .GetAll().Where(t => t.TenancyName.ToLower() == club.NAVN.ToLower()).FirstOrDefault();
                            if (existingTenant == null)
                            {
                              
                                var newTenant = MapBridgeKlubberToTenant(club, userId);

                                using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions { IsTransactional = false }))
                                {
                                    await _tenantRepository.InsertAsync(newTenant);
                                    await _unitOfWorkManager.Current.SaveChangesAsync();
                                    await uow.CompleteAsync();
                                }

                            }
                        }
                        catch (Exception e)
                        {

                            throw;
                        }

                    }


                    foreach (var item in clientClubMembers)
                    {
                        var clientTenant = clientClubs.FirstOrDefault(x => x.ID == item.FKBRIDGEKLUBID);
                        if (clientTenant == null)
                        {
                            continue;
                        }

                        var clientMember = clientMembers.FirstOrDefault(x => x.ID == item.FKMEDLEMSID);
                        if (clientMember == null)
                        {
                            continue;
                        }

                        var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.TenancyName == clientTenant.NAVN);
                        if (tenant == null)
                        {
                            continue;
                        }


                        var existingMember = await _customerRepository
                           .FirstOrDefaultAsync(t => t.CustomerNo.ToLower() == clientMember.NUMMER.ToString().ToLower());

                        if (existingMember == null)
                        {
                            var clientCustomer = MapToCustomer(clientMember, userId, tenant.Id);

                            // var input = _objectMapper.Map<CreateCustomerDto>(economicCustomer);
                            //input.Password = "Dummy@123"; // Default password for new users
                            //input.EmailAddress = !string.IsNullOrWhiteSpace(input.EmailAddress)
                            //    ? input.EmailAddress
                            //    : $"{input.CustomerNo + args.TenantId}@e-conomic.com";
                            //input.UserName = input.EmailAddress;

                            User newUser = null; // Variable to hold the new user

                            // Check if a user with the same email already exists
                            var userDbo = _userManager.Users.FirstOrDefault(x => x.EmailAddress == clientCustomer.EMAIL);

                            if (userDbo == null)
                            {
                                // User doesn't exist, create a new user
                                //newUser = _objectMapper.Map<User>(input);
                                newUser.UserTypeId = userTypeId;
                                newUser.Password = "Secure$888";
                                newUser.EmailAddress = clientCustomer.EMAIL;
                                newUser.UserName = clientCustomer.EMAIL;
                                newUser.CreationTime = DateTime.UtcNow;
                                newUser.CreatorUserId = userId;

                                newUser.Name = clientCustomer.FORNAVN;
                                newUser.Surname = clientCustomer.EFTERNAVN;
                                newUser.TenantId = tenant.Id;
                                newUser.IsActive = true;

                                // Create and save the new user
                                await _userManager.CreateAsync(newUser, newUser.Password);
                                await _unitOfWorkManager.Current.SaveChangesAsync();

                                // Fetch the newly created user
                                newUser = _userManager.Users.OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            else
                            {
                                // If user exists, assign the existing user to newUser
                                newUser = userDbo;
                            }

                            // Check if the customer is already associated with the user
                            //if (_customerRepository.FirstOrDefault(x => x.UserId == newUser.Id) == null)
                            //{
                            //    // Create and insert the new customer
                            //    var customer = Customer.Create(args.TenantId,
                            //        clientCustomer.CustomerNo,
                            //        clientCustomer.Address ?? "",
                            //        clientCustomer.Postcode ?? "",
                            //        clientCustomer.TownCity ?? "",
                            //        clientCustomer.Country ?? "",
                            //        clientCustomer.TelephoneFax ?? "",
                            //        clientCustomer.Website ?? "",
                            //        clientCustomer.Currency ?? "",
                            //        newUser.Id, // Associate the user with the customer
                            //        newUser.Id,
                            //        1,
                            //        null,
                            //        false,
                            //        string.Empty);

                            //    _customerRepository.Insert(customer);
                            //}

                            clientCustomer.UserId = newUser.Id;
                            await _customerRepository.InsertAsync(clientCustomer);
                            await _unitOfWorkManager.Current.SaveChangesAsync();



                            var userTenant = new UserTenant();
                            userTenant.TenantId = tenant.Id;
                            userTenant.UserId = newUser.Id;

                            await _userTenantRepository.InsertAsync(userTenant);
                            await _unitOfWorkManager.Current.SaveChangesAsync();

                        }


                    }


                    //    await unitOfWork.CompleteAsync();
                    //}
                }

                return true;
            }
            catch (Exception e)
            {

                throw;
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
