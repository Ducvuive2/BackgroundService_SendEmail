using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            // Ensure SqlServerTypes are initialized
            SqlServerTypesInitializer.Initialize();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<PurchasingManager> PurchasingManagers { get; set; }
        public DbSet<BusinessEntityContact> BusinessEntityContacts { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Employee entity to map to HumanResources.Employee table in AdventureWorks
            modelBuilder.Entity<Employee>()
                .ToTable("Employee", "HumanResources")
                .HasKey(e => e.BusinessEntityID);

            // Configure property mappings
            modelBuilder.Entity<Employee>()
                .Property(e => e.BusinessEntityID)
                .HasColumnName("BusinessEntityID")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.NationalIDNumber)
                .HasColumnName("NationalIDNumber")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.LoginID)
                .HasColumnName("LoginID")
                .IsRequired();

            // Ignore the SqlHierarchyId property as EF Core can't map it directly
            modelBuilder.Entity<Employee>()
                .Ignore(e => e.OrganizationNode);

            modelBuilder.Entity<Employee>()
                .Property(e => e.OrganizationLevel)
                .HasColumnName("OrganizationLevel");

            modelBuilder.Entity<Employee>()
                .Property(e => e.JobTitle)
                .HasColumnName("JobTitle")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.BirthDate)
                .HasColumnName("BirthDate")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.MaritalStatus)
                .HasColumnName("MaritalStatus")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Gender)
                .HasColumnName("Gender")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.HireDate)
                .HasColumnName("HireDate")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.SalariedFlag)
                .HasColumnName("SalariedFlag")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.VacationHours)
                .HasColumnName("VacationHours")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.SickLeaveHours)
                .HasColumnName("SickLeaveHours")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.CurrentFlag)
                .HasColumnName("CurrentFlag")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.rowguid)
                .HasColumnName("rowguid")
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired();

            // Configure PurchasingManager entity - this will be used for query projections
            modelBuilder.Entity<PurchasingManager>()
                .HasNoKey();

            // Configure Person entity
            modelBuilder.Entity<Person>()
                .ToTable("Person", "Person")
                .HasKey(p => p.BusinessEntityID);

            // Configure ContactType entity
            modelBuilder.Entity<ContactType>()
                .ToTable("ContactType", "Person")
                .HasKey(c => c.ContactTypeID);

            // Configure BusinessEntityContact entity
            modelBuilder.Entity<BusinessEntityContact>()
                .ToTable("BusinessEntityContact", "Person")
                .HasKey(b => new { b.BusinessEntityID, b.PersonID, b.ContactTypeID });
        }
    }

    public static class SqlServerTypesInitializer
    {
        public static void Initialize()
        {
            // Ensure the SQL Server types assembly is loaded
            try
            {
                // Register the SQL Server types
                SqlHierarchyId.Null.ToString();
            }
            catch (Exception)
            {
                // Log any issues with loading SqlServerTypes
            }
        }
    }
}