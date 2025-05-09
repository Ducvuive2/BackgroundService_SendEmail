using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Employee
    {
        public int BusinessEntityID { get; set; }
        public string NationalIDNumber { get; set; } = string.Empty;
        public string LoginID { get; set; } = string.Empty;
        
        // We won't use SqlHierarchyId since EF Core doesn't support it directly
        [NotMapped]
        public byte[]? OrganizationNode { get; set; }
        
        public short? OrganizationLevel { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string MaritalStatus { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public bool SalariedFlag { get; set; }
        public short VacationHours { get; set; }
        public short SickLeaveHours { get; set; }
        public bool CurrentFlag { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}