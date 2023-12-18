using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApp.Tests.Repository;

public class MyPokemonRepositoryTests
{
    private async Task<DataContext> GetDataContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        DataContext databaseContext = new DataContext(options);
        databaseContext.Database.EnsureCreated();
        if (await databaseContext.Pokemon.AnyAsync())
        {
            for (int i = 0; i < 10; i++)
            {
                databaseContext.Add(
                new Pokemon()
                {
                    Id = i,
                    Name = "Charmeleon",
                    BirthDate = new DateTime(2012, 12, 31),
                    Reviews = new List<Review>()
                        {
                            new Review { Title="Pikachu",Text = "Pickahu is the best pokemon, because it is electric", Rating = 5,
                            Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                            new Review { Title="Pikachu", Text = "Pickachu is the best a killing rocks", Rating = 5,
                            Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                            new Review { Title="Pikachu",Text = "Pickchu, pickachu, pikachu", Rating = 1,
                            Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                        },
                    PokemonCategories = new List<PokemonCategory>()
                        {
                            new PokemonCategory { Category = new Category() { Name = "Electric"}}
                        },

                }
                );
            }
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }

    [Fact]
    public async void CreatePokemon()
    {
        int ownerId = 8;
        int categoryId = 12;
        Pokemon pokemon = A.Fake<Pokemon>();
        var databaseContext = await GetDataContext();
        var repository = new PokemonRepository(databaseContext);

        //databaseContext.Add()
        var result = repository.CreatePokemon(ownerId, categoryId, pokemon);

        result.Should().BeTrue();
    }
}
