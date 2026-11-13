
// // using KodaStore.Api.Models;
// using Ewallet.Mvc.Models;
// using Microsoft.EntityFrameworkCore;

// namespace KodaStore.Api.Seeding;

// public static class UserSeedData
// {
    
//     private static IEnumerable<User> InitialUsers()
//     {
//         // *Data seed generate
//         for(int i = 1; i<=10; i++)
//         {
//             long id=i;
//             string userName = $"username{i:D3}";
//             string fullName = $"User Dummy {i}";
//             string email = $"userdummy{i}@dummy.com";
//             string phone="08967729"+i;
//             string? PasswordHash = $"password{i}";
//             UserStatusEnum status = UserStatusEnum.ACTIVE; 

//             yield return new User(id,userName,fullName,email,phone,PasswordHash,status);
//         }
//     }

//     public static void Seed(DbContext context)
//     {
//         foreach(var user in InitialUsers())
//         {
            
//             bool exists = context.Set<User>().Any(u=>u.Id == user.Id);

//             if (!exists)
//             {
//                 context.Set<User>().Add(user);
//             };

//         }

//         context.SaveChanges();
//     }

//     public static async Task SeedAsync(DbContext context, CancellationToken ct)
//     {
//         foreach (var user in InitialUsers())
//         {
//             bool exists = await context.Set<User>().AnyAsync(p => p.Id == user.Id, ct);

//             if (!exists)
//             {
//                 context.Set<User>().Add(user);
//             }
//         }

//         await context.SaveChangesAsync(ct);
//     }
// }