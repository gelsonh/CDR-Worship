﻿using CDR_Worship.Models;
using CDR_Worship.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;



namespace CDR_Worship.Data
{
    public static class DataUtility
    {

        private const string? _adminRole = "Admin";
        private const string? _moderatorRole = "Moderator";


        public static string GetConnectionString(IConfiguration configuration)
        {
            // Intenta obtener la cadena de conexión directamente desde la configuración.
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Si no se encuentra, intenta obtener la cadena de conexión de la variable de entorno.
            if (string.IsNullOrEmpty(connectionString))
            {
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                if (!string.IsNullOrEmpty(databaseUrl))
                {
                    connectionString = BuildConnectionString(databaseUrl);
                }
            }

            // Si aún no se encuentra, lanza una excepción.
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            return connectionString;
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                // TrustServerCertificate = true
            };

            return builder.ToString();
        }


        public static DateTime GetPostGresDate(DateTime date)
        {
            return date.ToUniversalTime();
        }

        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            // Obtaining the necessary services based on the IServiceProvider parameter
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<AppUser>>();
            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();


            await dbContextSvc.Database.MigrateAsync();

            // Seed Application Roles
            await SeedRolesAsync(roleManagerSvc);

            await SeedDefaultChordsAsync(dbContextSvc);

            //Seed User(s)
            await SeedAppUsersAsync(userManagerSvc, configurationSvc);

            await SeedDefaultMembersAsync(dbContextSvc);

            // Seed Demo User(s)
            await SeedDemoUsersAsync(userManagerSvc, configurationSvc);
        }
  
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Admin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Moderator)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.DemoUser)));
        }

        private static async Task SeedAppUsersAsync(UserManager<AppUser> userManager, IConfiguration configuration)
        {

            string? adminEmail = configuration["AdminLoginEmail"] ?? Environment.GetEnvironmentVariable("AdminLoginEmail");
            string? adminPassword = configuration["AdminPwd"] ?? Environment.GetEnvironmentVariable("AdminPwd");

            string? moderatorEmail = configuration["ModeratorLoginEmail"] ?? Environment.GetEnvironmentVariable("ModeratorLoginEmail");
            string? moderatorPassword = configuration["ModeratorPwd"] ?? Environment.GetEnvironmentVariable("ModeratorPwd");

            try
            {
                if (!string.IsNullOrEmpty(adminEmail))
                {
                    AppUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new AppUser()
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            FirstName = "Carlos",
                            LastName = "Cruz",
                            EmailConfirmed = true,
                        };

                        await userManager.CreateAsync(adminUser, adminPassword!);
                        await userManager.AddToRoleAsync(adminUser, _adminRole!);
                    }
                }

                if (!string.IsNullOrEmpty(moderatorEmail))
                {
                    AppUser? modUser = await userManager.FindByEmailAsync(moderatorEmail);

                    if (modUser == null)
                    {
                        modUser = new AppUser()
                        {
                            UserName = moderatorEmail,
                            Email = moderatorEmail,
                            FirstName = "Gelson",
                            LastName = "Hernandez",
                            EmailConfirmed = true,
                        };

                        await userManager.CreateAsync(modUser, moderatorPassword!);
                        await userManager.AddToRoleAsync(modUser, _moderatorRole!);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Blog Users.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("*********************************");

                throw;
            }

        }

        private static async Task SeedDemoUsersAsync(UserManager<AppUser> userManager, IConfiguration configuration)
        {
        string? demoLoginEmail = configuration["DemoLoginEmail"] ?? Environment.GetEnvironmentVariable("DemoLoginEmail");
        string? demoLoginPassword = configuration["DemoLoginPassword"] ?? Environment.GetEnvironmentVariable("DemoLoginPassword");

        AppUser demoUser = new AppUser()
        {
            UserName = demoLoginEmail,
            Email = demoLoginEmail,
            FirstName = "Demo",
            LastName = "User",
            EmailConfirmed = true,
        };

        try
        {

            AppUser? appUser = await userManager.FindByEmailAsync(demoLoginEmail!);

            if (appUser == null)
            {
                await userManager.CreateAsync(demoUser, demoLoginPassword!);
                
                await userManager.AddToRoleAsync(demoUser, nameof(Roles.DemoUser));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("************* ERROR *************");
            Console.WriteLine("Error Seeding Demo Login User.");
            Console.WriteLine(ex.Message);
            Console.WriteLine("*********************************");

            throw;
        }
    }

      public static async Task SeedDefaultChordsAsync(ApplicationDbContext context)
{
    try
    {
        // Crear la lista de acordes con los nombres correctos, utilizando CDRChordMapper para obtener el nombre
        IList<Chord> chords = new List<Chord>
        {
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.C] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Cm] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Csos] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Csosm] },
            
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.D] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Dm] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Dsos] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Dsosm] },

            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.E] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Em] },

            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.F] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Fm] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Fsos] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Fsosm] },

            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.G] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Gm] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Gsos] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Gsosm] },

            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.A] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Am] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Asos] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Asosm] },

            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.B] },
            new Chord() { ChordName = CDRChordMapper.ChordNames[CDRChord.Bm] }
        };

        // Agregar los acordes a tu contexto
        await context.Chords.AddRangeAsync(chords);

        // Guardar los cambios en tu contexto
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine("*************  ERROR  *************");
        Console.WriteLine("Error Seeding Chords.");
        Console.WriteLine(ex.Message);
        Console.WriteLine("***********************************");
        throw;
    }
}

        public static async Task SeedDefaultMembersAsync(ApplicationDbContext context)
        {
            try
            {
                // Crear instancias de las personas que estarán tocando en la banda
                var leadSingers = new List<Member>
{
    new Member { MemberName = "Carlos Cruz", Role = BandMembers.LeadSinger.ToString() },
    new Member { MemberName = "Arevalo Andy", Role = BandMembers.LeadSinger.ToString() },
    new Member { MemberName = "Deyshla Jimenez", Role = BandMembers.LeadSinger.ToString() }
    // Agrega más cantantes principales según sea necesario
};

                var backingVocalists = new List<Member>
        {
            new Member { MemberName = "Carlos Cruz", Role = BandMembers.BackingVocalist.ToString() },
            new Member { MemberName = "Arevalo Andy", Role = BandMembers.BackingVocalist.ToString() },
            new Member { MemberName = "Deyshla Jimenez", Role = BandMembers.BackingVocalist.ToString() }, 
            // Agrega más coristas según sea necesario
        };

                var backingVocalistTwo = new List<Member>
        {
            new Member { MemberName = "Carlos Cruz", Role = BandMembers.BackingVocalist.ToString() },
            new Member { MemberName = "Arevalo Andy", Role = BandMembers.BackingVocalist.ToString() },
            new Member { MemberName = "Deyshla Jimenez", Role = BandMembers.BackingVocalist.ToString() }, 
            // Agrega más coristas según sea necesario
        };

                var leadGuitarists = new List<Member>
        {
            new Member { MemberName = "Carlos Cruz", Role = BandMembers.LeadGuitarist.ToString() },
            // Agrega más guitarristas principales según sea necesario
        };

                var secondGuitarists = new List<Member>
        {
            new Member { MemberName = "Gelson Hernandez", Role = BandMembers.SecondGuitarist.ToString() },
            // Agrega más guitarristas principales según sea necesario
        };

                var bassists = new List<Member>
        {
            new Member { MemberName = "Franky Resto", Role = BandMembers.Bassist.ToString() },
            // Agrega más bajistas según sea necesario
        };

                var drummers = new List<Member>
        {
            new Member { MemberName = "Derek Jimenez", Role = BandMembers.Drummer.ToString() },
            // Agrega más bateristas según sea necesario
        };

                if (!context.Members.Any())
                {
                    // Agregar los miembros a la base de datos
                    context.Members.AddRange(leadSingers);
                    context.Members.AddRange(backingVocalists);
                    context.Members.AddRange(leadGuitarists);
                    context.Members.AddRange(secondGuitarists);
                    context.Members.AddRange(bassists);
                    context.Members.AddRange(drummers);

                    // Guardar los cambios en la base de datos
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Scheduled Songs.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }
    }
}
