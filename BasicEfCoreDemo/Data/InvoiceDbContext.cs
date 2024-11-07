using BasicEfCoreDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicEfCoreDemo.Data;
public class InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : DbContext(options)
{
  public DbSet<Invoice> Invoices => Set<Invoice>();
}