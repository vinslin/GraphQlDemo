using GraphQlDemo.Models;
using Microsoft.EntityFrameworkCore;


namespace GraphQlDemo.GraphQL.Query
{
    public class Query
    {
        private readonly EmployeesDbContext db;

        public Query(EmployeesDbContext dbContext) {
            db = dbContext; //contruictor type dependecy inhjection
        }
        
         [GraphQLName("GetAllUsers")]
        public async Task<IEnumerable<User>> GetUsers([Service] EmployeesDbContext db)
        {//directly in the method signature
            return await db.Users.Include(x => x.AddressTables).Include(x => x.Posts).ToListAsync();
        }

        [GraphQLName("GetUser")]
        public async Task<User?> GetUser(int id, [Service] EmployeesDbContext db) { 
             return await db.Users.Where(u => u.Id == id).Include(x=>x.AddressTables).Include(x=>x.Posts).FirstOrDefaultAsync();
        }

        [GraphQLName("GetEmployees")]
        public async Task<IEnumerable<Employee>> GetEmployees([Service] EmployeesDbContext dd) {
            return await dd.Employees.ToListAsync();
        }

        [GraphQLName("GetEmployee")]
        public async Task<Employee> GetEmployee(Guid id) {
            return await db.Employees.FirstOrDefaultAsync(e => e.Id == id);
        
        }
    }
}
