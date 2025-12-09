using Microsoft.EntityFrameworkCore;
using CityOS.Models;

namespace CityOS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<City> Cities => Set<City>();

    public DbSet<BIEntity> BIEntities => Set<BIEntity>();
    public DbSet<BIField> BIFields => Set<BIField>();
    public DbSet<BIRelation> BIRelations => Set<BIRelation>();
    public DbSet<BIEntityRow> BIEntityRows => Set<BIEntityRow>();
    

    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<BudgetRevision> BudgetRevisions => Set<BudgetRevision>();
    public DbSet<IncomeSource> IncomeSources => Set<IncomeSource>();
    public DbSet<IncomePlan> IncomePlans => Set<IncomePlan>();
    public DbSet<IncomeExecution> IncomeExecutions => Set<IncomeExecution>();
    public DbSet<DataSource> DataSources => Set<DataSource>();
}