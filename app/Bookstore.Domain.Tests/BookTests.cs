using Bookstore.Domain.Books;

namespace Bookstore.Domain.Tests;

public class BookTests
{
    [Fact]
    public void IsInStock_ReturnsTrue_When_QuantityIsGreaterThanZero()
    {
        Book book = new("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 1);
        Assert.True(book.IsInStock);

        book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 0);
        Assert.False(book.IsInStock);
    }

    [Fact]
    public void IsLowInStock_ReturnsTrue_When_QuantityIsLessThanOrEqualToThreshold()
    {
        const int threshold = Book.LowBookThreshold;

        Book book = new("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, threshold - 1);
        Assert.True(book.IsLowInStock);

        book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, threshold);
        Assert.True(book.IsLowInStock);

        book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, threshold + 1);
        Assert.False(book.IsLowInStock);
    }

    [Fact]
    public void ReduceStockLevel_ReducesQuantityBySpecifiedAmount_When_Executed()
    {
        Book book = new("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 100);
        int amountToReduce = 50;
        int expectedQuantity = book.Quantity - amountToReduce;

        book.ReduceStockLevel(amountToReduce);

        Assert.Equal(expectedQuantity, book.Quantity);
    }

    [Fact]
    public void ReduceStockLevel_DoesNotReduceQuantityBelowZero_When_Executed()
    {
        Book book = new("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 50);
        int amountToReduce = 60;

        book.ReduceStockLevel(amountToReduce);

        Assert.True(book.Quantity == 0);
    }
}