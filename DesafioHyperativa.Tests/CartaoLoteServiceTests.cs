using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Service;
using DesafioHyperativa.Service.Exceptions;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DesafioHyperativa.Tests;

public class CartaoLoteServiceTests
{
    IRepository<CartaoLote> fakeRepository;
    ICartaoRepository fakeCartaoRepository;
    ILoteRepository fakeLoteRepository;
    ILogger<CartaoLoteService> fakeLogger;

    public CartaoLoteServiceTests()
    {
        fakeRepository = A.Fake<IRepository<CartaoLote>>();
        fakeCartaoRepository = A.Fake<ICartaoRepository>();
        fakeLoteRepository = A.Fake<ILoteRepository>();
        fakeLogger = A.Fake<ILogger<CartaoLoteService>>();
    }

    private string GetPath(string nameFile)
    {
        string rootPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        return Path.Combine(rootPath, "txt", $"{nameFile}.txt");
    }
    private byte[] GetFile(string path)
    {
        string content = File.ReadAllText(path);

        return Encoding.UTF8.GetBytes(content);
    }

    [Fact]
    public async Task ProcessarSalvarArquivo_ValidFile_CallsServiceMethods()
    {
        var service = new CartaoLoteService(fakeRepository, fakeCartaoRepository, fakeLoteRepository, fakeLogger);

        var path = GetPath("PadraoCerto");

        var content = GetFile(path);

        using (MemoryStream stream = new MemoryStream(content))
        {
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(path));

            await service.ProcessarSalvarArquivo(file);
        }
    }

    [Fact]
    public async Task ProcessarSalvarArquivo_UnValidFile_CallsServiceMethods()
    {

        var service = new CartaoLoteService(fakeRepository, fakeCartaoRepository, fakeLoteRepository, fakeLogger);

        var path = GetPath("DESAFIO-HYPERATIVA");

        var content = GetFile(path);

        using (MemoryStream stream = new MemoryStream(content))
        {
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(path));

            var exception = await Assert.ThrowsAsync<BadRequestException>(async () =>
            {
                await service.ProcessarSalvarArquivo(file);
            });

            Assert.Contains("O número de cartão da linha 6 deve ter 16 caracteres", exception.Message);
            Assert.Contains("O número de cartão da linha 8 deve ter 16 caracteres", exception.Message);
            Assert.Contains("Existe cartões duplicados nas linhas 2 (C2), 4 (C3)", exception.Message);
            Assert.Contains("Existe cartões duplicados nas linhas 9 (C8), 11 (C10)", exception.Message);
        }
    }
}