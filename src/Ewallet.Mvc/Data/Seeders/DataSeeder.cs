// using Ewallet.Mvc.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;

// namespace Ewallet.Mvc.Data;

// public static class DataSeeder
// {
//     private static DateTime Utc (int day, int hour, int minute =0) => new(2026,11,day,hour,minute,0,DateTimeKind.Utc);

//     public static void Seed(this ModelBuilder modelBuilder)
//     {
        
//     }

//     private static void SeedUsersAndWallets(ModelBuilder modelBuilder)
//     {
//         var users = new[]
//         {
//             (Id: 1L,  Username: "andi.pratama",     FullName: "Andi Pratama"),
//             (Id: 2L,  Username: "budi.santoso",     FullName: "Budi Santoso"),
//             (Id: 3L,  Username: "citra.lestari",    FullName: "Citra Lestari"),
//             (Id: 4L,  Username: "dewi.anggraini",   FullName: "Dewi Anggraini"),
//             (Id: 5L,  Username: "eko.wijaya",       FullName: "Eko Wijaya"),
//             (Id: 6L,  Username: "fitri.handayani",  FullName: "Fitri Handayani"),
//             (Id: 7L,  Username: "galih.permana",    FullName: "Galih Permana"),
//             (Id: 8L,  Username: "hana.putri",       FullName: "Hana Putri"),
//             (Id: 9L,  Username: "indra.kusuma",     FullName: "Indra Kusuma"),
//             (Id: 10L, Username: "joko.susilo",      FullName: "Joko Susilo"),
//         };

//         modelBuilder.Entity<User>().HasData(users.Select(u => new
//         {
//             u.Id,u.Username,u.FullName,
//             Email=$"{u.Username}@example.com",
//             Phone=$"08123123{u.Id:D2}",
//             Status=UserStatusEnum.ACTIVE,
//             CreatedAtUtc = Utc(1,7)
//         }));

//         var financialBalances = new Dictionary<long, decimal>
//         {
//             {1,650_000m}, {2,350_000},{3,750_000m},{4,175_000},{5,75_000m}
//         };

//         modelBuilder.Entity<Wallet>().HasData(users.Select(u => new
//         {
//             Id=u.Id,
//             UserId=u.Id,
//             WalletNumber=$"{u.Id:D8}",
//             Balance=financialBalances.GetValueOrDefault(u.Id,0m),
//             Status=WalletStatusEnum.ACTIVE,
//             CreatedAtUtc=Utc(1,7)
//         }));
//     }


//     private static void SeedPaymentMethods(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<PaymentMethod>().HasData(
//             new{Id=1,Code="BANK_TRANSFER",Name="Bank Transfer",ProviderName="Simulasi Bank",IsActive=true,CreatedUtc=Utc(1,7)},
//             new{Id=2,Code="QRIS",Name="QRIS",ProviderName="Qris",IsActive=true,CreatedUtc=Utc(1,7)},
//             new{Id=3,Code="VIRTUAL_ACCOUNT",Name="Virtual Account",ProviderName="Simulasi VA",IsActive=false,CreatedUtc=Utc(1,7)}
//         );
//     }


//     private static void SeedTransactions(ModelBuilder modelBuilder)
//     {
//         const TransactionTypeEnum TopUp = TransactionTypeEnum.TOPUP;
//         const TransactionTypeEnum Transfer = TransactionTypeEnum.TRANSFER;
//         const TransactionStatusEnum Success = TransactionStatusEnum.SUCCESS;

//         modelBuilder.Entity<Transaction>().HasData(
//             new{Id=1,ReferenceNo="TP-01",type=TopUp,Amount=500_00m,status=Success,description="topup via bank tf",}
//         );
//     }
// }