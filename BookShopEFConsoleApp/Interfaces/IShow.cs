using System.Numerics;

namespace BookShopEFConsoleApp.Interfaces;

public interface IShow<T> where T : INumber<T>
{
    T Id { get; set; }
    string Value { get; set; }
}