using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Auth.Commands.LoginUser;
using VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;
using Xunit;

namespace VaccinationCard.IntegrationTests;

public class PersonIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PersonIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FullFlow_Should_CreatePerson_When_Authorized()
    {
        // 1. Tentar criar pessoa SEM token (Deve falhar)
        var newPerson = new CreatePersonCommand("Integration Test User", 25, "M");
        var failResponse = await _client.PostAsJsonAsync("/api/Persons", newPerson);
        
        failResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized); // 401

        // 2. Fazer Login para pegar o Token (O Admin já existe pelo Seed)
        var loginCommand = new LoginUserCommand("admin", "admin123");
        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginCommand);
        
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK); // 200
        
        var authData = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        var token = authData!.Token;

        // 3. Adicionar o Token no Cabeçalho (Bearer)
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // 4. Tentar criar pessoa COM token (Deve passar)
        var successResponse = await _client.PostAsJsonAsync("/api/Persons", newPerson);
        
        successResponse.StatusCode.Should().Be(HttpStatusCode.Created); // 201
        
        var createdPerson = await successResponse.Content.ReadFromJsonAsync<PersonDto>();
        createdPerson!.Name.Should().Be("Integration Test User");
    }
}