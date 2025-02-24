﻿using Demo.Models;

namespace Demo.Interfaces;

public interface IProductRepositoryRemote
{
    Task<Product> Create(Product request);
    Task<List<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<Product?> DeleteAsync(int id);
}