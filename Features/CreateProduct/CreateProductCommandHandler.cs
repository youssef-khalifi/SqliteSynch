using Demo.Data;
using Demo.Dtos;
using Demo.Features.CreateProduct;
using Demo.Interfaces;
using Demo.Models;
using Demo.Repositories.LocalRepository;
using Demo.Repositories.RemoteRepository;
using MediatR;

namespace Demo.Features;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    
    private readonly IProductRepositoryLocal _localRepository;
    private readonly IProductRepositoryRemote _remoteRepository;

    public CreateProductCommandHandler( IProductRepositoryLocal localRepository, IProductRepositoryRemote remoteRepository)
    {
        _localRepository = localRepository;
        _remoteRepository = remoteRepository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Random random = new Random();
        bool randomBool = random.Next(2) == 0;

        if (randomBool)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };
            var returnedProduct = await _localRepository.Create(product);
            return new ProductDto
            {
                Id = returnedProduct.Id,
                Name = returnedProduct.Name,
                Description = returnedProduct.Description,
                Price = returnedProduct.Price,
            };
        }
        else
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };
            var returnedProduct = await _remoteRepository.Create(product);
            return new ProductDto
            {
                Id = returnedProduct.Id,
                Name = returnedProduct.Name,
                Description = returnedProduct.Description,
                Price = returnedProduct.Price,
            };
        }
    }
}