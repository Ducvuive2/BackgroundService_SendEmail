using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class PurchasingManagerRepository : IPurchasingManagerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PurchasingManagerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PurchasingManager>> GetAllPurchasingManagersAsync()
        {
            // Using LINQ to query for purchasing managers
            // This assumes you have the corresponding DbSets in your context for these entities
            var purchasingManagers = await (
                from contact in _dbContext.BusinessEntityContacts
                join contactType in _dbContext.ContactTypes on contact.ContactTypeID equals contactType.ContactTypeID
                join person in _dbContext.People on contact.PersonID equals person.BusinessEntityID
                where contactType.Name == "Purchasing Manager"
                orderby person.LastName, person.FirstName
                select new PurchasingManager
                {
                    BusinessEntityID = person.BusinessEntityID,
                    LastName = person.LastName,
                    FirstName = person.FirstName
                }
            ).ToListAsync();

            return purchasingManagers;
        }
    }
} 