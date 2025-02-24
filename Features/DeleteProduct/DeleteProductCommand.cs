using Demo.Models;
using MediatR;

namespace Demo.Features.DeleteProduct;

public class DeleteProductCommand : IRequest<Product>
{
    public int Id { get; set; }
}