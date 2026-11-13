
// // using KodaStore.Api.Models;
// using Ewallet.Mvc.Models;
// using Microsoft.EntityFrameworkCore;

// namespace KodaStore.Api.Seeding;

// public static class WalletSeedData
// {
//     private const string DemoOrderNumber = "WAL-001";

    
//     private static IEnumerable<Wallet> InitialWallets()
//     {
        

       
//     }

//     public static void Seed(DbContext context)
//     {
//         if(context.Set<Wallet>().Any(o => o.WalletNumber == DemoOrderNumber)) return;

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