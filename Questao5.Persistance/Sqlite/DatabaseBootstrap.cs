using Microsoft.EntityFrameworkCore;
using Questao5.Domain.Entites;
using Questao5.Persistance.Data;
using System;
using System.Linq;

namespace Questao5.Infrastructure.Sqlite
{
    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly Questao5Context _dbContext;

        public DatabaseBootstrap(Questao5Context dbContext)
        {
            _dbContext = dbContext;
        }

        public void Setup()
        {
            // Ensure that the database is created
            _dbContext.Database.EnsureCreated();

            // Optionally, you can seed data here if needed.
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (_dbContext.ContaCorrente.Any()) return; // Database already seeded

            _dbContext.ContaCorrente.AddRange(
                new ContaCorrente
                {
                    IdContaCorrente = new Guid("B6BAFC09-6967-ED11-A567-055DFA4A16C9"),
                    Numero = 123,
                    Nome = "Katherine Sanchez",
                    Ativo = true
                },
                new ContaCorrente
                {
                    IdContaCorrente = new Guid("FA99D033-7067-ED11-96C6-7C5DFA4A16C9"),
                    Numero = 456,
                    Nome = "Eva Woodward",
                    Ativo = true
                },
                new ContaCorrente
                {
                    IdContaCorrente = new Guid("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                    Numero = 789,
                    Nome = "Tevin Mcconnell",
                    Ativo = true
                },
                new ContaCorrente
                {
                    IdContaCorrente = new Guid("F475F943-7067-ED11-A06B-7E5DFA4A16C9"),
                    Numero = 741,
                    Nome = "Ameena Lynn",
                    Ativo = false
                },
                new ContaCorrente
                {
                    IdContaCorrente = new Guid("BCDACA4A-7067-ED11-AF81-825DFA4A16C9"),
                    Numero = 852,
                    Nome = "Jarrad Mckee",
                    Ativo = false
                },
                new ContaCorrente
                {
                    IdContaCorrente = new Guid("D2E02051-7067-ED11-94C0-835DFA4A16C9"),
                    Numero = 963,
                    Nome = "Elisha Simons",
                    Ativo = false
                }
            );

            _dbContext.SaveChanges();
        }
    }
}
