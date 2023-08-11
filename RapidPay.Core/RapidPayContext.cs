using Microsoft.EntityFrameworkCore;
using RapidPay.Core.Entities;
using System.Reflection;

namespace RapidPay.Core
{
	public class RapidPayContext : DbContext
	{
		public DbSet<Card> Cards { get; set; }

		public DbSet<Payment> Payments { get; set; }

		public RapidPayContext(DbContextOptions<RapidPayContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
