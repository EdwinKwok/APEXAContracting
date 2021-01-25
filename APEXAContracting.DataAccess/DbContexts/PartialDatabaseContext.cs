using APEXAContracting.DataAccess.DotNetCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.DataAccess
{
    /// <summary>
    /// Connect to one MySql or Sql Server database.
    /// Note: Don't modify this class. Corporate with the auto generated FOSCommonContext class.
    /// </summary>
    public partial class DatabaseContext : BaseDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration config)
           : base(options, config)
        {
        }

    }
}
