using GraphQlDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQlDemo.GraphQL.Mutation
{
    public class Mutation
    {
        
        [GraphQLName("AddUser")]
        [GraphQLDescription("Add the user into the db")]
        public async Task<User> AddUser(string name, string email, [Service] EmployeesDbContext db)
        {
            var user = new User
            {
                Name = name,
                Email = email
            };

            var response= db.Users.Add(user);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("Failed to save the db changes ", ex)
                        .SetCode("Db_Exception")
                        .Build()
                    );
            }
            return user;
        }

        [GraphQLName("DeleteUser")]
        [GraphQLDescription("Delete the user ")]
        public async Task<User> DeleteUser(int id, [Service] EmployeesDbContext db) {
            var user = db.Users.Find(id);

            if (user != null)
            {
                db.Users.Remove(user);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("Failed to save the db changes ", ex)
                            .SetCode("Db_Exception")
                            .Build()
                        );
                }
            }
            else {
                throw new GraphQLException(
                    ErrorBuilder.New()
                    .SetMessage("no user in this id")
                    .SetCode("No_User").Build()
                    );
            }
                return user!;
        }

        [GraphQLName("UpdateUser")]
        [GraphQLDescription("Update the user")]
        public async Task<User> UpdateUser(int id, string name, string email, [Service] EmployeesDbContext db) {
            var user = await db.Users.FindAsync(id);
            if (user != null) {
                user.Name = name;
                user.Email = email;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("Failed to save the db changes ", ex)
                            .SetCode("Db_Exception")
                            .Build()
                        );
                }
            }
            return user!;
        }

        [GraphQLName("AddPosts")]
        public async Task<Post> AddPost(string title, string content, int userId, [Service] EmployeesDbContext db) {
            var post = new Post
            {
                Title = title,
                Content = content,
                UserId = userId
            };
            db.Posts.Add(post);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("Failed to save the db changes ", ex)
                        .SetCode("Db_Exception")
                        .Build()
                    );
            }
            return post;
        } 
        [GraphQLName("AddAdress")]
        public async Task<AddressTable> AddAdress(int pincode, int userId, [Service] EmployeesDbContext db) {
            var user = await db.Users.FindAsync(userId);
            if (user == null) {
                throw new GraphQLException(
                ErrorBuilder.New()
                  .SetMessage("User not found so cant set address")
                  .SetCode("USER_NOT_FOUND")
                  .Build()
          );
            }
            var adress = new AddressTable
            {
                Pincode= pincode,
                UserId = userId
            };
            db.AddressTables.Add(adress);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex){
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("Failed to save the db changes ",ex)
                        .SetCode("Db_Exception")
                        .Build()
                    );
            }
            return adress;
        } 
    }
}
