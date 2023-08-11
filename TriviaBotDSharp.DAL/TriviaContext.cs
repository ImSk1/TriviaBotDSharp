using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TriviaBotDSharp.DAL.Models;

namespace TriviaBotDSharp.DAL
{
    public class TriviaContext : DbContext
    {
        public TriviaContext()
        {
            
        }
        public TriviaContext(DbContextOptions<TriviaContext> options) : base(options)
        {

        }
        public DbSet<PlayerProfile> Profiles { get; set; }
       
    }
}
