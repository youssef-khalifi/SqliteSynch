﻿namespace Demo.Dtos;

public class ProductDto
{
    public int Id  { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
}