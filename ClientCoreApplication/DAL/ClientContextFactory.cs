using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ClientContextFactory: IDesignTimeDbContextFactory<CustDbContext>
    {

        CustDbContext IDesignTimeDbContextFactory<CustDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustDbContext>();
            optionsBuilder.UseSqlServer(@"Server=.\;Database=Test;Trusted_Connection=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new CustDbContext(optionsBuilder.Options);
        }
    }
}
