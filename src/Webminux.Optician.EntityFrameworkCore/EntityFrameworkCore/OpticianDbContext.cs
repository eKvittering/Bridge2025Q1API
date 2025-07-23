using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.Activities;
using Webminux.Optician.Authorization.Roles;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Bookings;
using Webminux.Optician.Brands;
using Webminux.Optician.Categories;
using Webminux.Optician.Chat;
using Webminux.Optician.Companies;
using Webminux.Optician.Core;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Core.Notes;
using Webminux.Optician.Core.SubCustomers;
using Webminux.Optician.Core.Tasks;
using Webminux.Optician.Customers;
using Webminux.Optician.CustomFields;
using Webminux.Optician.EyeTools;
using Webminux.Optician.Faults;
using Webminux.Optician.MenuItems;
using Webminux.Optician.MultiTenancy;
using Webminux.Optician.Orders;
using Webminux.Optician.PageConfigs;
using Webminux.Optician.Products;
using Webminux.Optician.Rooms;
using Webminux.Optician.Sites;
using Webminux.Optician.Suppliers;
using Webminux.Optician.Tickets;

namespace Webminux.Optician.EntityFrameworkCore
{
    public class OpticianDbContext : AbpZeroDbContext<Tenant, Role, User, OpticianDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<ActivityArt> ActivityArts { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityResponsible> ActivityResponsibles { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<EyeTool> EyeTools { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<CustomerGroup> CustomerGroups { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EconomicSyncHistory> EconomicSyncHistories { get; set; }
        public DbSet<ActivityTask> ActivityTasks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProductGroup> productGroups { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductResponsibleGroup> ProductResponsibleGroups { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }
        public DbSet<EntityFieldMapping> EntityFieldMappings { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Fault> Faults { get; set; }
        public DbSet<FaultFile> FaultFiles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingEmployee> BookingEmployees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<TenantMedia> TenantMedias { get; set; }
        public DbSet<BookingActivityType> BookingActivityTypes { get; set; }
        public DbSet<ChatNotification> chatNotifications { get; set; }
        public DbSet<MenuItem> menuItems { get; set; }
        public DbSet<Webminux.Optician.ProductItem.ProductItem> ProductItems { get; set; }
        public DbSet<PageConfig> PageConfigs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SubCustomer> SubCustomers { get; set; }
        public DbSet<Ticket> tickets { get; set; }
        public DbSet<TicketUser> TicketsUsers { get; set; }
        public DbSet<EmployeeGroup> EmployeeGroup { get; set; }
        public DbSet<Site> sites { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }

        public DbSet<Webminux.Optician.PackageType.PackageType> PackageType { get; set; }
        public DbSet<Webminux.Optician.Package.Pacakge> Packages { get; set; }
        public DbSet<Webminux.Optician.SubPackage.SubPackage> SubPackages { get; set; }
        public DbSet<SyncHistoryDetail> SyncHistoryDetails { get; set; }
        public DbSet<UserTenant> UserTenants { get; set; }

        //Clients data tables
        public DbSet<MEDLEMMER> MEDLEMMER { get; set; }
        public DbSet<MEDLEMSKABER> MEDLEMSKABER { get; set; }
        public DbSet<BRIDGEKLUBBER> BRIDGEKLUBBER { get; set; }

        //End


        public OpticianDbContext(DbContextOptions<OpticianDbContext> options)
            : base(options)
        {

        }

    }
}

