using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApp.Tests.Controller
{
    public class PokemonControllerTests
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonControllerTests()
        {
            _pokemonRepository = A.Fake<IPokemonRepository>();
            _reviewRepository = A.Fake<IReviewRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void PokemonController_GetPokemons_ReturnOK()
        {
            //Arrange
            var pokemons = A.Fake<ICollection<PokemonDto>>();
            var pokemonList = A.Fake<List<PokemonDto>>();
            A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemons)).Returns(pokemonList);
            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.GetPokemons();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }






        [Theory]
        [InlineData(1, 10)]
        public void PokemonController_CreatePokemon_ReturnsOk(int ownerId, int catId)
        {
            PokemonDto pokemonCreate = A.Fake<PokemonDto>();
            var pokemonMap = A.Fake<Pokemon>();
            A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate)).Returns(null);
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonCreate)).Returns(pokemonMap);
            A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)).Returns(true);
            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            var result = controller.CreatePokemon(ownerId, catId, pokemonCreate);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void PokemonController_CreatePokemon_Returns422()
        {
            int ownerId = 1;
            int catId = 5;
            PokemonDto pokemonCreate = A.Fake<PokemonDto>();
            Pokemon pokemonMap = A.Fake<Pokemon>();
            var pokemon = A.Fake<Pokemon>();
            A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate)).Returns(pokemon);
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonCreate)).Returns(pokemonMap);
            A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)).Returns(true);
            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            var result = controller.CreatePokemon(ownerId, catId, pokemonCreate);


            result.Should().NotBeNull();
            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(422);
        }

    }
}
