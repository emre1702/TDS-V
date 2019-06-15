dotnet ef dbcontext scaffold "Host=localhost;Port=3306;Database=TDS;Username=user;Password=pw" Pomelo.EntityFrameworkCore.MySql -o %~dp0/Entity -f
